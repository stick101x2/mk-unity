using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
public class AudioManager : MonoBehaviour
{
    StudioBankLoader main;
    StudioBankLoader shared;
    StudioBankLoader sfx;
 
    public static AudioManager instance;

    public SoundSet[] soundSets;
    List<Sound> list_s = new List<Sound>();
    Sound[] sounds;
    public AudioManager Setup()
    {
       
        if (!instance)
            instance = this;
        else
        {
            Destroy(this);
        }
        return instance;
    }
    public void SoundSetup()
    {
        for (int i = 0; i < soundSets.Length; i++)
        {
            for (int i2 = 0; i2 < soundSets[i].sounds.Length; i2++)
            {
                list_s.Add(soundSets[i].sounds[i2]);
            }
        }
        sounds = list_s.ToArray();

    }
    public void LoadBank(int id)
    {
        switch (id)
        {
            case 0:
                if (!main)
                    main = transform.GetChild(0).GetComponent<StudioBankLoader>();
                main.Load();
                break;
            case 1:
                if (!shared)
                    shared = transform.GetChild(1).GetComponent<StudioBankLoader>();
                shared.Load();
                break;
            case 2:
                if (!sfx)
                    sfx = transform.GetChild(2).GetComponent<StudioBankLoader>();
                sfx.Load();
                break;
            default:
                if (!main)
                    main = transform.GetChild(0).GetComponent<StudioBankLoader>();
                if (!shared)
                    shared = transform.GetChild(1).GetComponent<StudioBankLoader>();
                if (!sfx)
                    sfx = transform.GetChild(2).GetComponent<StudioBankLoader>();
                main.Load();
                shared.Load();
                sfx.Load();
                break;
        }
    }
    public void UnLoadBank(int id)
    {
        switch (id)
        {
            case 0:
                if (!main)
                    main = transform.GetChild(0).GetComponent<StudioBankLoader>();
                main.Unload();
                break;
            case 1:
                if (!shared)
                    shared = transform.GetChild(1).GetComponent<StudioBankLoader>();
                shared.Unload();
                break;
            case 2:
                if (!sfx)
                    sfx = transform.GetChild(2).GetComponent<StudioBankLoader>();
                sfx.Unload();
                break;
            default:
                if (!main)
                    main = transform.GetChild(0).GetComponent<StudioBankLoader>();
                if (!shared)
                    shared = transform.GetChild(1).GetComponent<StudioBankLoader>();
                if (!sfx)
                    sfx = transform.GetChild(2).GetComponent<StudioBankLoader>();
                main.Unload();
                shared.Unload();
                sfx.Unload();
                break;
        }
    }

    public Sound Play(string name,Vector3 pos = new Vector3())
    {

        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Dev.LogWarning("Unable To Play " + name);
            return null;
        }
        Dev.Log(s.soundEvent);
        if (s.oneShot)
        {
            FMODUnity.RuntimeManager.PlayOneShot(s.soundEvent, pos);
            return null;
        }
        else
        {
            s.ins = FMODUnity.RuntimeManager.CreateInstance(s.soundEvent);
            s.ins.start();
            return s;
        }
        
    }
    private void OnDestroy()
    {
        foreach (Sound s in sounds)
        {
            s.ins.release();
        }
    }
}

