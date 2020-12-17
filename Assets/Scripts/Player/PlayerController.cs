using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer
{
    Player p;

    //references
    Player_Move move;
    Player_Input input;
    Player_Variables v;
    //vars
    Transform pivot;
    Transform drift_r;
    Transform drift_l;
    //private

    public float steerM; // NOW
    public float steerC; // CURRENT
    public float steerMod = 10;
    public float debug_sAmt;
    //p
    float steerAmount = 0;

    float weight;
    float handling;
    float traction; // will be multipled by surface traction
    // Start is called before the first frame update
    public void Setup(Player p)
    {
        this.p = p;

        move = GetComponent<Player_Move>();
        v = p.var;
        input = p.input;
        v.pivot = transform.GetChild(0);
        v.drift_r = transform.GetChild(1);
        v.drift_l = transform.GetChild(2);

        pivot = v.pivot;

        v.drift_l.rotation = p.main.c_kart.drift_l.rotation;
        v.drift_r.rotation = p.main.c_kart.drift_r.rotation;

        drift_r = v.drift_r;
        drift_l = v.drift_l;

        backleft = p.main.c_kart.wheel_back_L;
        backright= p.main.c_kart.wheel_back_R;
        frontleft= p.main.c_kart.wheel_front_L;
        frontright= p.main.c_kart.wheel_front_R;

        v.c_max_speed = 50 * p.main.f_speed * p.main.f_weight; //50 is defualt
        v.real_max_speed = v.c_max_speed;
        v.c_accel = p.main.f_acceleration;

        handling = p.main.f_handling;
        traction = p.main.f_offroad; 

        p.AddState("normal", new Normal_State(this));
    }

    // Update is called once per frame
    public void P_Update()
    {
        
    }
    public void P_FixedUpdate()
    {
        
    }

    void Move()
    {
        float isDrift = !v.isDrifting == true ? 1 : 0;
        if(input.accelHeld &&!input.dccelHeld)
        {
            move.Move(ref v.c_speed,v.c_max_speed - (steerC * handling) * isDrift, 1f * traction);
        }
        else if(input.dccelHeld && !input.accelHeld)
        {
            move.Move(ref v.c_speed, -(v.c_max_speed * 0.675f - (steerC * handling) * isDrift),2f * traction);
        }else 
        {
            if (input.accelHeld && input.dccelHeld)
                move.Move(ref v.c_speed, 0, 4f);
            else
                move.Move(ref v.c_speed, 0, 1.5f * traction);
        }
    }
    void Tires()
    {
        p.main.WheelsSteer(input.x);
        if(v.c_speed < v.c_max_speed*0.5f)
            p.main.WheelsMove(v.real_speed);
        else
            p.main.WheelsMove(v.c_speed);
    }
    Vector3 steerDirVector;
    void Steer()
    {
        v.steerDirection = input.x;

        

        if(v.isDrifting)
        {
            if (v.driftDirection < 0)
            {
                v.steerDirection = input.x < 0 ? -1.5f : -0.5f;
                if (Mathf.Abs(input.x) < 0.1f)
                    v.steerDirection = -1;
            }
            else
            {
                v.steerDirection = input.x > 0 ? 1.5f : 0.5f;
                if (Mathf.Abs(input.x) < 0.1f)
                    v.steerDirection = 1;
            }
           
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(0, v.driftTilt * v.driftDirection, 0), v.turnSpeed * Time.deltaTime);

            p.rid.AddForce(transform.right * -v.outerwardDriftForce * v.driftDirection, ForceMode.Acceleration);
        }
        else
        {
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(0,  0, 0), v.turnSpeed * Time.deltaTime);
        }
        float turnSpeedModifier = Mathf.Abs(v.real_speed) / v.c_max_speed + 1f;
        // Debug.Log(turnSpeedModifier);
       

        steerAmount = input.accelHeld && input.dccelHeld && Mathf.Abs(v.c_speed) < 1f ? v.c_max_speed * v.steerDirection : v.real_speed / turnSpeedModifier * v.steerDirection;
        steerAmount *= traction;
        steerM = Mathf.Abs(steerAmount) < 0.1f ? 0 : Mathf.Sign(steerAmount);



        steerC =Mathf.Lerp(steerC, steerAmount, Time.deltaTime * steerMod);
        

        steerDirVector = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerC, transform.eulerAngles.z);

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, steerDirVector, v.turnSpeed * 0.5f * Time.deltaTime);
        //   Dev.Log("SAmt: " + steerAmount);
       debug_sAmt = steerAmount;
       // debug_sDVect = steerDirVector;
    }
    
    void Drift()
    {
        if (input.driftDown && Mathf.Abs(v.steerDirection) > 0.1f)
        {
            StartDrift();
            input.driftDown = false;
        }
        if (input.driftHeld && v.isGrounded && v.c_speed > (v.c_max_speed * 0.4f) * traction)
        {
            float lastDrift = v.drift;
            v.drift = Func.Remap(Mathf.Abs(v.steerDirection), 0.5f, 1.5f, -1f, 1f) * v.driftDirection;

            if (v.driftTimer >= 1f)
            {
                if (lastDrift > 0 && v.drift <= 0 || lastDrift < 0 && v.drift >= 0)
                { 
                    v.driftTimer = 0;
                    v.driftCount++;
                    Dev.Log("Drifted");
                }
            }

            if (Mathf.Abs(v.drift) > 0)
            {
                v.driftTimer += Time.deltaTime * 3f;
                if (v.driftTimer > 1f)
                    v.driftTimer = 1f;
            }
            else
                v.driftTimer = 0;
        }
    }


    void StartDrift()
    {
        //do hop
        v.driftDirection = Mathf.Sign(v.steerDirection);
        v.isDrifting = true;
        v.driftCount = 0;
        v.driftTimer = 0;
    }

    void Drifting()
    {
        
    }

    void EndDrift()
    {
        v.driftDirection = 0;
        v.isDrifting = false;
    }
    Transform backleft;
    Transform backright;
    Transform frontleft;
    Transform frontright;

    RaycastHit bl;
    RaycastHit br;
    RaycastHit fl;
    RaycastHit fr;

    Vector3 upDir;
    Vector3 newUp;
    void FourPointNormalRotation()
    {  
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, -transform.up);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, -transform.up, out hit, 1f, v.terrian))
            v.isGrounded = true;
        else
        {
            Debug.Log("not grounded");
            v.isGrounded = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, Vector3.up) * transform.rotation, 2f * Time.deltaTime);
            return;
        }

        bool hitbl = Physics.Raycast(backleft.position + Vector3.up, Vector3.down, out bl);
        bool hitbr = Physics.Raycast(backright.position + Vector3.up, Vector3.down, out br);
        bool hitfl = Physics.Raycast(frontleft.position + Vector3.up, Vector3.down, out fl);
        bool hitfr = Physics.Raycast(frontright.position + Vector3.up, Vector3.down, out fr);

        Vector3 a = br.point - bl.point;
        Vector3 b = fr.point - br.point;
        Vector3 c = fl.point - fr.point;
        Vector3 d = br.point - fl.point;

        Vector3 crossBA = Vector3.Cross(b, a);
        Vector3 crossCB = Vector3.Cross(c, b);
        Vector3 crossDC = Vector3.Cross(d, c);
        Vector3 crossAD = Vector3.Cross(a, d);

         newUp = (crossBA + crossCB + crossDC + crossAD).normalized;
        upDir = transform.up;
        upDir.x = newUp.x;
        upDir.z = newUp.z;

        //transform.up = Vector3.Lerp(transform.up, newUp, v.groundNormalRotateSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, newUp) * transform.rotation, v.groundNormalRotateSpeed * Time.deltaTime);
    }
    void GroundNormalRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up *0.1f, -transform.up , out hit, 0.75f, v.terrian))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7.5f * Time.deltaTime);
            v.isGrounded = true;
        }
        else
            v.isGrounded = false;
    }
    // ===== States ====== 


    public class Normal_State : IState // regular movement (moving and jumping)
    {
        PlayerController p;
        public Normal_State(PlayerController player)
        {
            p = player;
        }

        public void OnEnter(IState lastState) //Start State
        {

        }
        public void OnUpdate() //Update
        {

        }
        public void OnFixedUpdate() //FixedUpdate
        {
            p.Move();
            p.Tires();
            p.Steer();
            p.GroundNormalRotation();
            p.Drift();
        }
        public void OnExit(IState nextState) //Leave State
        {

        }
    }

}


