using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
[CreateAssetMenu(fileName = "New Driver", menuName = "Settings/Driver")]
public class DriverSettings : ScriptableObject
{
    public Stats bonusStat;
}

[System.Serializable]
public class Stats
{
    //modifier, final = ((kart_stats + driver_stat) / 100 + 0.5)
    public int speed; // modifies your max speed
    public int weight; // force when bumping other racers, also influcenes max speed
    public int acceleration; // how fast you gain speed
    public int handling; //how much speed is lost during turns
    public int drift; // changes angle of drift
    public int offroad; // traction how sharp your turns are
}
