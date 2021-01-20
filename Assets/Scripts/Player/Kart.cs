using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
    public KartSettings data;
    public PositionWithOffset offset;

    [Header("Steering")]
    public Transform front_wheel_right;
    public Transform front_wheel_left;
    [Header("Moving")]
    public Transform wheel_front_R;
    public Transform wheel_front_L;
    public Transform wheel_back_R;
    public Transform wheel_back_L;
    [Header("Drifting")]
    public Transform drift_r;
    public Transform drift_l;
    [Header("Particles")]
    public ParticleGroup smokeIdle;
    public ParticleGroup smokeDrive;
    public ParticleGroup smokeBurst;
    [Space(5)]
    public ParticleGroup driftR;
    public ParticleGroup driftRBurst;
    public ParticleSystem BwheelDustR;
    [Space(5)]
    public ParticleGroup driftL;
    public ParticleGroup driftLBurst;
    public ParticleSystem BwheelDustL;
    [Space(5)]
    public Color driftOrange = Color.black;
    public Color driftBlue = Color.black;
    public Color driftPurple = Color.black;
    [Header("References")]
    public Animator anim;

}
