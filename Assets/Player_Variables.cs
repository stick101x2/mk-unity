using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Variables : MonoBehaviour,IPlayer
{
    [Header("Moving")]
    public float c_speed;
    public float c_accel;

    public float c_max_speed;

    public float real_speed;
    public float real_max_speed;
    [Header("Turning")]
    public float steerDirection;
    public float driftDirection;
    public float driftTilt = 20;
    public float turnSpeed = 8;
    public float outerwardDriftForce;
    public bool isDrifting;
    [Header("Ground")]
    public LayerMask terrian;
    public bool isGrounded;
    public float groundNormalRotateSpeed = 7.5f;
    
    










    public void Setup(Player p)
    {
        c_max_speed = 50 * p.main.f_speed; //50 is defualt
        real_max_speed = c_max_speed;
        c_accel = p.main.f_acceleration;
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
