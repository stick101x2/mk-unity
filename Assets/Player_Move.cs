using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour, IPlayer
{
     Player p;
     Player_Variables v;
    public void Setup(Player p)
    {
        this.p = p;
        v = p.var;
    }
    public void Move(ref float current_speed, float targetSpeed, float modifier = 1f,bool foward = true)
    {
        v.real_speed = transform.InverseTransformDirection(p.rid.velocity).z;
        float acceleration = foward ? v.c_accel : 1f;
        current_speed = Mathf.Lerp(current_speed, targetSpeed,  acceleration * (modifier * Time.deltaTime));

        Vector3 fowardVector = transform.forward;
        Vector3 velocity = fowardVector * v.c_speed;
        velocity.y = p.rid.velocity.y;
        p.rid.velocity = velocity;
    }

    public void Turn()
    {

    }





    //Ignore
    public void P_FixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public void P_Update()
    {
        //throw new System.NotImplementedException();
    }

    
}
