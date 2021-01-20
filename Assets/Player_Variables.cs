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

    public float maxYVelocity = 100f;
    [Header("Turning")]
    public float steerDirection;
    public float turnSpeed = 8;
    public float modelturnSpeed = 8;
    public float returnSpeed = 2f;
    [Header("Drift")]
    public float hopForce;
    public float driftDirection;
    public float driftTilt = 20;
    public float outerwardDriftForce;
    public bool isDrifting;
    public int driftCount;
    public float driftTimer;
    public float drift;
    [Header("Ground")]
    public LayerMask terrian;
    public bool isGrounded;
    public float groundNormalRotateSpeed = 7.5f;
    [Header("Boost")]
    public float boostTimer;
    [Header("Transforms")]
    public Transform mainRot;
    public Transform pivot;
    public Transform drift_r;
    public Transform drift_l;
    [Header("AntiGravity")]
    public bool antiGravity;
    public float antiGravityForce = -5f;
    [Header("Bool")]
    public bool usingAntiGravity;







    public void Setup(Player p)
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
