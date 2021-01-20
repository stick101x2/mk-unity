using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Method : MonoBehaviour
{
    PlayerController p;

    private void Awake()
    {
        p = GetComponentInParent<PlayerController>();
    }
    void JumpStart()
    {
        p.JumpStart();
    }
    void JumpEnd()
    {
        p.JumpEnd();
    }
}
