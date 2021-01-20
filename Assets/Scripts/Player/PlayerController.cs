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
    Transform mainRot;
    Transform drift_r;
    Transform drift_l;
    //private

    float steerM; // NOW
    float steerC; // CURRENT
    float steerMod = 10;
    float debug_sAmt;
    float steerAmount = 0;
    bool jumped = false;
    float rotSpeed = 0f;
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
        v.mainRot = transform.GetChild(0);
        v.pivot = transform.Find("Rotation/Pivot");
        v.drift_r = transform.Find("Rotation/Drift_R");
        v.drift_l = transform.Find("Rotation/Drift_L");

        mainRot = v.mainRot;
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

    void AntiGravity()
    {
        if(v.antiGravity)
        {
            p.EnableGravity(false);
            p.AddForce(transform.up * v.antiGravityForce, ForceMode.VelocityChange);

            Vector3 vel = p.GetVelocity();
            vel += -transform.up * v.antiGravityForce;
            p.SetVelocity(vel);

            if (!v.isGrounded || !v.usingAntiGravity)
                v.antiGravity = false;
        }
        else
        {
            if (v.usingAntiGravity && v.isGrounded)
                v.antiGravity = true;
            p.EnableGravity(true);
        }
            
    }
    void Smoke()
    {
        if (input.accelDown)
        {
            p.main.c_kart.smokeBurst.Play();

            input.accelDown = false;
        }
        if (input.accelHeld || input.dccelHeld)
        {
            if (p.main.c_kart.smokeIdle.IsPlaying())
                p.main.c_kart.smokeIdle.Stop();
        }
        else
        {
            if (!p.main.c_kart.smokeIdle.IsPlaying())
                p.main.c_kart.smokeIdle.Play();
        }
    }
    void Move()
    {
        Smoke();
        float isDrift = !v.isDrifting == true ? 1 : 0;
        if(input.accelHeld &&!input.dccelHeld)
        {
            if(v.isGrounded)
            move.Move(ref v.c_speed,v.c_max_speed - (steerC * handling) * isDrift, 1f * traction);
        }
        else if(input.dccelHeld && !input.accelHeld)
        {
            if (v.isGrounded)
                move.Move(ref v.c_speed, -(v.c_max_speed * 0.675f - (steerC * handling) * isDrift),2f * traction);
        }else if (v.isGrounded)
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
    float yRot;
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
           
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(0, v.driftTilt * v.driftDirection, 0), v.modelturnSpeed * Time.fixedDeltaTime);

            p.AddForce(mainRot.right * -v.outerwardDriftForce * v.driftDirection, ForceMode.Acceleration);
        }
        else
        {
            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(0,  0, 0), v.modelturnSpeed * Time.fixedDeltaTime);
        }
        float turnSpeedModifier = Mathf.Abs(v.real_speed) / v.c_max_speed + 1f;
        // Debug.Log(turnSpeedModifier);

        
        steerAmount = input.accelHeld && input.dccelHeld && Mathf.Abs(v.c_speed) < 1f ? v.c_max_speed * v.steerDirection : v.real_speed / turnSpeedModifier * v.steerDirection;
        steerAmount *= traction;
        steerM = Mathf.Abs(steerAmount) < 0.1f ? 0 : Mathf.Sign(steerAmount);

        if (v.isGrounded)
            steerC = Mathf.Lerp(steerC, steerAmount, Time.fixedDeltaTime * steerMod);
        else
        {
            steerC = Mathf.Lerp(steerC, steerAmount, Time.fixedDeltaTime * 0.333f);
        }


        Vector3 last = mainRot.localEulerAngles;
        last.x = 0;
        last.z = 0;

        steerDirVector = new Vector3(mainRot.localEulerAngles.x, mainRot.localEulerAngles.y + steerC, mainRot.localEulerAngles.z);
        steerDirVector.x = 0;
        steerDirVector.z = 0;

        mainRot.Rotate(Vector3.up, (steerC* 4f) * Time.fixedDeltaTime,Space.Self);
        //   Dev.Log("SAmt: " + steerAmount);
        debug_sAmt = steerAmount;
        yRot = mainRot.localEulerAngles.y;
       // debug_sDVect = steerDirVector;
    }
    
    void Drift()
    {
        if (input.driftDown)
        {
            if(v.isGrounded)
                Hop();
            input.driftDown = false;
        }
        if(v.isDrifting)
        {
            if (!input.driftHeld || v.c_speed < (v.c_max_speed * 0.4f) * traction)
                EndDrift(); 
        }
        
        
        if (input.driftHeld && v.isGrounded && v.c_speed > (v.c_max_speed * 0.4f) * traction&&v.isDrifting)
        {
            float lastDrift = v.drift;
            float s = Func.Remap(Mathf.Abs(v.steerDirection), 0.5f, 1.5f, -1f, 1f) * v.driftDirection;
            if (s != 0)
                v.drift = s;
            if (v.driftTimer >= 1f)
            {
                if (lastDrift > 0 && v.drift < 0 || lastDrift < 0 && v.drift > 0)
                {
                    v.driftTimer = 0;
                    v.driftCount++;
                    if (v.driftCount == 2)
                        Drifted(1);
                    if (v.driftCount == 4)
                        Drifted(2);
                    if (v.driftCount == 6)
                        Drifted(3);
                }
            }

            if (Mathf.Abs(v.drift) > 0)
            {
                v.driftTimer += Time.fixedDeltaTime * 3f;
                if (v.driftTimer > 1f)
                    v.driftTimer = 1f;
            }
            else
                v.driftTimer = 0;

            if (!p.main.c_kart.BwheelDustL.isPlaying)
            {
                p.main.c_kart.BwheelDustL.Play();
                p.main.c_kart.BwheelDustR.Play();

                if (v.driftCount >= 4)
                {
                    p.main.c_kart.driftL.Play();
                    p.main.c_kart.driftR.Play();
                }
            }

            

        }
        if (!v.isGrounded)
        {
            DisableDriftEffects();
        }
    }
    void DisableDriftEffects()
    {
        p.main.c_kart.BwheelDustL.Stop();
        p.main.c_kart.BwheelDustR.Stop();
        p.main.c_kart.driftL.Stop();
        p.main.c_kart.driftR.Stop();
    }
    void Hop()
    {
        AudioManager.instance.Play("Hop", transform.position);
        p.main.KartJump();
        jumped = true;
    }
    void StartDrift()
    {
        if (Mathf.Abs(v.steerDirection) < 0.5f)
            return;
        if (!input.driftHeld)
            return;
        //do hop
        v.isDrifting = true;
        move.Timer(true);
        v.driftDirection = Mathf.Sign(v.steerDirection);
        v.driftCount = 0;
        v.driftTimer = 0;
    }
    public void JumpStart()
    {
        jumped = true;
    }
    public void JumpEnd()
    {
        jumped = false;
        StartDrift();
    }

    void Drifted(int power)
    {
        p.main.c_kart.driftL.Stop();
        p.main.c_kart.driftR.Stop();
        switch (power)
        {
            case 1:
                p.main.c_kart.driftLBurst.SetColor(p.main.c_kart.driftOrange);
                p.main.c_kart.driftRBurst.SetColor(p.main.c_kart.driftOrange);
               
                break;
            case 2:
                p.main.c_kart.driftLBurst.SetColor(p.main.c_kart.driftBlue);
                p.main.c_kart.driftRBurst.SetColor(p.main.c_kart.driftBlue);
                p.main.c_kart.driftL.SetColor(p.main.c_kart.driftBlue);
                p.main.c_kart.driftR.SetColor(p.main.c_kart.driftBlue);
                p.main.c_kart.driftL.Play();
                p.main.c_kart.driftR.Play();
                break;
            default:
                p.main.c_kart.driftLBurst.SetColor(p.main.c_kart.driftPurple);
                p.main.c_kart.driftRBurst.SetColor(p.main.c_kart.driftPurple);
                p.main.c_kart.driftL.SetColor(p.main.c_kart.driftPurple);
                p.main.c_kart.driftR.SetColor(p.main.c_kart.driftPurple);
                p.main.c_kart.driftL.Play();
                p.main.c_kart.driftR.Play();
                break;
        }
        p.main.c_kart.driftLBurst.Play();
        p.main.c_kart.driftRBurst.Play();
        //  Dev.Log("Drifted");
    }

    void EndDrift()
    {
        move.Timer(false);
        DisableDriftEffects();
        v.driftDirection = 0;
        v.isDrifting = false;
    }
    Transform backleft;
    Transform backright;
    Transform frontleft;
    Transform frontright;

    RaycastHit backleftRay;
    RaycastHit backRightRay;
    RaycastHit frontLeftRay;
    RaycastHit frontRightRay;

    Vector3 upDir;
    Vector3 newUp;

    bool blrCheck;
    bool brrCheck;
    bool frrCheck;
    bool flrCheck;

    Vector3 blrPoint;
    Vector3 brrPoint;
    Vector3 frrPoint;
    Vector3 flrPoint;
    Vector3 blr;
    Vector3 brr;
    Vector3 frr;
    Vector3 flr;
    void FourPointNormalRotation()
    {

        int offGroundCount = 0;

        bool hitBL = Physics.Raycast(backleft.position + transform.up, -transform.up, out backleftRay,float.PositiveInfinity, v.terrian);
        bool hitBR = Physics.Raycast(backright.position + transform.up, -transform.up, out backRightRay, float.PositiveInfinity, v.terrian);
        bool hitFL = Physics.Raycast(frontleft.position + transform.up, -transform.up, out frontLeftRay, float.PositiveInfinity, v.terrian);
        bool hitFR = Physics.Raycast(frontright.position + transform.up, -transform.up, out frontRightRay, float.PositiveInfinity, v.terrian);

        
        blr = Vector3.up;
        brr = Vector3.up;
        frr = Vector3.up;
        flr = Vector3.up;
        
        blrCheck= backleftRay.distance > 2.1f || Mathf.Approximately(backleftRay.distance, 0f);
        brrCheck= backRightRay.distance > 2.1f || Mathf.Approximately(backRightRay.distance, 0f);
        frrCheck= frontLeftRay.distance > 2.1f || Mathf.Approximately(frontLeftRay.distance, 0f);
        flrCheck= frontRightRay.distance > 2.1f || Mathf.Approximately(frontRightRay.distance, 0f);

        if (blrCheck)
        {
            
            offGroundCount++;
            Debug.DrawRay(backleftRay.point, transform.up, Color.red);
        }
        else
            Debug.DrawRay(backleftRay.point, transform.up, Color.green);
        if (brrCheck)
        {
            
            offGroundCount++;
            Debug.DrawRay(backRightRay.point, transform.up, Color.red);
        }                                     
        else                                  
            Debug.DrawRay(backRightRay.point, transform.up, Color.green);
        if (frrCheck)
        {
            
            offGroundCount++;
            Debug.DrawRay(frontLeftRay.point, transform.up, Color.red);
        }                                     
        else                                  
            Debug.DrawRay(frontLeftRay.point, transform.up, Color.green);
        if (flrCheck)                         
        {                                     
                                              
            offGroundCount++;                 
            Debug.DrawRay(frontRightRay.point,transform.up, Color.red);
        }                                     
        else                                  
            Debug.DrawRay(frontRightRay.point,transform.up, Color.green);

        frr = frontRightRay.normal;
        flr = frontLeftRay.normal;
        brr = backRightRay.normal;
        blr = backleftRay.normal;

        blrPoint = frontRightRay.point;
        brrPoint = frontLeftRay.point;
        frrPoint = backRightRay.point;
        flrPoint = backleftRay.point;
        // Get the vectors that connect the raycast hit points

        Vector3 a = blrPoint - brrPoint;
        Vector3 b = frrPoint - blrPoint;
        Vector3 c = flrPoint - frrPoint;
        Vector3 d = blrPoint - flrPoint;

        // Get the normal at each corner

        Vector3 crossBA = Vector3.Cross(b, a);
        Vector3 crossCB = Vector3.Cross(c, b);
        Vector3 crossDC = Vector3.Cross(d, c);
        Vector3 crossAD = Vector3.Cross(a, d);

        // Calculate composite normal
        upDir = (crossBA + crossCB + crossDC + crossAD).normalized;
        
        if (offGroundCount < 4)
        {
            transform.up = Vector3.Lerp(transform.up, upDir, 6f * Time.deltaTime);
            v.isGrounded = !jumped ? true : false;
            
        }
        else
        {
            
            transform.up = Vector3.Lerp(transform.up, Vector3.up, rotSpeed * Time.deltaTime);
            v.isGrounded = false;
        }
        Rot();
        //   transform.eulerAngles = new Vector3(transform.eulerAngles.x,yRot, transform.eulerAngles.z);

    }
    void Rot()
    {
        if(v.isGrounded)
        {
            rotSpeed = 0f;
        }else
        {
            rotSpeed += Time.deltaTime * v.returnSpeed;
            if (rotSpeed > 10f)
                rotSpeed = 10f;
        }
    }
    float y;
    void GroundNormalRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hit, 2f, v.terrian))
        {
            Quaternion lerped = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, v.groundNormalRotateSpeed * Time.deltaTime);
           /* eular = lerped.eulerAngles;

            rot = eular;
            rot.y = 0f;

            qat = Quaternion.Euler(rot);*/

            transform.rotation = lerped;
            v.isGrounded = !jumped ? true : false;
        }
        else
        {
            /*
            float y = transform.eulerAngles.y;
            Vector3 lerped = Vector3.Lerp(transform.up, Vector3.up, rotSpeed * Time.deltaTime);
           // lerped.y = y;

            transform.rotation = Quaternion.Euler(lerped);*/

            if (v.isGrounded)
            {
                Vector3 localEularAngles = mainRot.localEulerAngles;
                localEularAngles += transform.eulerAngles;
                localEularAngles.x = 0f;
                localEularAngles.z = 0f;

                mainRot.localEulerAngles = localEularAngles;
            }
            v.isGrounded = false;
            transform.up = Vector3.Lerp(transform.up, Vector3.up, rotSpeed * Time.deltaTime);
        }
        Rot();
    }
    void DriverA()
    {
        p.main.DriverTurn(input.x,p.var.isDrifting);
    }
    void YVelocityLimiter()
    {
        p.LimitYVelocity(-v.maxYVelocity);
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
            p.YVelocityLimiter();
            p.Move();
            p.Steer();
            if (p.v.antiGravity)
                p.GroundNormalRotation();
            else
                p.GroundNormalRotation();

            p.Drift();
            p.AntiGravity();
            //visual
            p.Tires();
            p.DriverA();
        }
        public void OnExit(IState nextState) //Leave State
        {

        }
    }

}


