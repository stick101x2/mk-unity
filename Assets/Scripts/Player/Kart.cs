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
}
