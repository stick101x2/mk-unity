using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    AudioManager audio;
    // Start is called before the first frame update
    void Awake()
    {
        audio = FindObjectOfType<AudioManager>().Setup();
        audio.UnLoadBank(-1);
        audio.LoadBank(-1);
        audio.SoundSetup();
       // audio.Play("Main");
    }
}
