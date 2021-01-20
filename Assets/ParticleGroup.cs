using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGroup : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> systems;
    [SerializeField]
    private ParticleSystem top;
    public void SetColor(Color color)
    {
        for (int i = 0; i < systems.Count; i++)
        {
            ParticleSystem.MainModule main = systems[i].main;
            main.startColor = color;
        }
    }
    public void Play()
    {
        if (!top)
            top = GetComponent<ParticleSystem>();
        top.Play(true);
    }
    public void Stop()
    {
        if (!top)
            top = GetComponent<ParticleSystem>();
        top.Stop(true);
    }

    public bool IsPlaying()
    {
        return top.isPlaying;
    }

}
