using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Sound Set", menuName = "Settings/SoundSet")]
public class SoundSet : ScriptableObject
{
    public Sound[] sounds;
}
[System.Serializable]
public class Sound
{
    public FMOD.Studio.EventInstance ins;
    public string name;
    [FMODUnity.EventRef]
    public string soundEvent;
    public bool oneShot;
}
