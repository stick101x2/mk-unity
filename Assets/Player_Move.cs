using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour, IPlayer
{
     Player p;
     Player_Variables v;
    public Transform test;
    float timerD;
    float timerS;
    bool drift;
    public float speed;
    Vector3 fowardVector;
    Vector3 f;
    public void Setup(Player p)
    {
        this.p = p;
        v = p.var;
        test.position = transform.position;
    }
    public void Move(ref float current_speed, float targetSpeed, float modifier = 1f,bool foward = true)
    {
     
        v.real_speed = v.mainRot.InverseTransformDirection(p.GetVelocity()).z;
        float acceleration = foward ? v.c_accel : 1f;
        current_speed = Mathf.Lerp(current_speed, targetSpeed,  acceleration * (modifier * Time.deltaTime));
 
     
        if (v.isDrifting)
        {   if (v.driftDirection > 0)
                fowardVector = Vector3.Lerp(fowardVector, v.drift_r.forward, timerD) ;
            else
                fowardVector = Vector3.Lerp(fowardVector, v.drift_l.forward, timerD);
        }
        else
            fowardVector = Vector3.Lerp(fowardVector, v.mainRot.forward, timerS);

       
        Vector3 velocity = fowardVector * v.c_speed;
        if (v.antiGravity)
        {
            velocity += -transform.up * v.antiGravityForce;
            f = velocity;
            p.SetVelocity(f);
        }
        else
            p.SetVelocityWithoutY(velocity);
    }
    public void Timer(bool up)
    {
        drift = up;

        if(up)
        {
            fowardVector = v.mainRot.forward;

            timerD = 0f;
            timerS = 1f;
        }
        else
        {
            if (v.driftDirection > 0)
                fowardVector = v.drift_r.forward;
            else
                fowardVector = v.drift_l.forward;

            timerD = 1f;
            timerS = 0f;
        }
    }
    //Ignore
    public void P_FixedUpdate()
    {
        if(drift)
        {
            timerD += Time.fixedDeltaTime * speed;
            timerS -= Time.fixedDeltaTime * speed;
            if (timerD >= 1)
                timerD = 1f;
            if (timerS <= 0)
                timerS = 0f;
        }
        else 
        {
            timerD -= Time.fixedDeltaTime * speed;
            timerS += Time.fixedDeltaTime * speed;
            if (timerD <= 0)
                timerD = 0f;
            if (timerS >= 1f)
                timerS = 1f;
        }
    }

    public void P_Update()
    {
        //throw new System.NotImplementedException();
    }

    
}
