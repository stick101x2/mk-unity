using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kart",menuName = "Settings/Kart")]
public class KartSettings : ScriptableObject
{
    public bool doubleKart = false;
    public Stats stat;
}
