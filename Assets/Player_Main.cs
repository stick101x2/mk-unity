using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    public Kart c_kart;
    public Driver driver_a;
    public Driver driver_b;
    [Header("Stats")]
    public float f_speed;
    public float f_weight;
    public float f_acceleration;
    public float f_handling;
    public float f_drift;
    public float f_offroad;
    [Header("Wheels")]
    public float steerSpeed = 5;
    public float turnAngleOffset = 45f;

    public float speedModifier = 1;

    public void GetFinalStats()
    {
        if (c_kart.data.doubleKart)
        {
           // Debug.Log((50f / 100f));
            f_speed = (c_kart.data.stat.speed + (driver_a.data.bonusStat.speed + driver_b.data.bonusStat.speed)) / 100f + 0.5f;
            f_weight = (c_kart.data.stat.weight + (driver_a.data.bonusStat.weight + driver_b.data.bonusStat.weight)) / 100f + 0.5f;
            f_acceleration = (c_kart.data.stat.acceleration + (driver_a.data.bonusStat.acceleration + driver_b.data.bonusStat.acceleration)) / 100f + 0.5f;
            f_handling = (c_kart.data.stat.handling + (driver_a.data.bonusStat.handling + driver_b.data.bonusStat.handling)) / 100f + 0.5f;
            f_drift = (c_kart.data.stat.drift + (driver_a.data.bonusStat.drift + driver_b.data.bonusStat.drift)) / 100f + 0.5f;
            f_offroad = (c_kart.data.stat.offroad + (driver_a.data.bonusStat.offroad + driver_b.data.bonusStat.offroad)) / 100f + 0.5f;
        }
        else
        {
            f_speed = (c_kart.data.stat.speed + (driver_a.data.bonusStat.speed)) / 100f + 0.5f;
            f_weight = (c_kart.data.stat.weight + (driver_a.data.bonusStat.weight)) / 100f + 0.5f;
            f_acceleration = (c_kart.data.stat.acceleration + (driver_a.data.bonusStat.acceleration)) / 100f + 0.5f;
            f_handling = (c_kart.data.stat.handling + (driver_a.data.bonusStat.handling)) / 100f + 0.5f;
            f_drift = (c_kart.data.stat.drift + (driver_a.data.bonusStat.drift)) / 100f + 0.5f;
            f_offroad = (c_kart.data.stat.offroad + (driver_a.data.bonusStat.offroad)) / 100f + 0.5f;
        }
    }

    public void WheelsSteer(float dir)
    {
        Vector3 fw = c_kart.front_wheel_left.localEulerAngles;

        if(dir > 0.1f)
        {
            fw.y += steerSpeed * Time.deltaTime;
        }
        else if (dir < -0.1f)
        {
            fw.y -= steerSpeed * Time.deltaTime;
        }
        else
        {
            if (fw.y > 91)
                fw.y -= steerSpeed * Time.deltaTime;
            if (fw.y < 89)
                fw.y += steerSpeed * Time.deltaTime;
            if (fw.y < 91 && fw.y > 89)
                fw.y = 90;
        }

        fw.y = Mathf.Clamp(fw.y, 0 + turnAngleOffset, 180 - turnAngleOffset);

        c_kart.front_wheel_right.localEulerAngles = fw;
        c_kart.front_wheel_left.localEulerAngles = fw;
    }
    public void WheelsMove(float speed)
    {
        float turnSpeed = -90 * Time.deltaTime * speed * speedModifier;

        c_kart.wheel_back_L.Rotate(new Vector3(0, 0, turnSpeed));
        c_kart.wheel_back_R.Rotate(new Vector3(0, 0, turnSpeed));
        c_kart.wheel_front_L.Rotate(new Vector3(0, 0, turnSpeed));
        c_kart.wheel_front_R.Rotate(new Vector3(0, 0, turnSpeed));
    }
}
/*
 //modifier, final = ((kart_stats + driver_stat) / 100 + 0.5)
    public int speed;
    public int weight;
    public int acceleration; 
    public int handling; 
    public int drift; 
    public int offroad;  
*/
