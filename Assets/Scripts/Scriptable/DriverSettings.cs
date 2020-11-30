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
    public int speed;
    public int weight;
    public int acceleration; 
    public int handling; 
    public int drift; 
    public int offroad; 
}
