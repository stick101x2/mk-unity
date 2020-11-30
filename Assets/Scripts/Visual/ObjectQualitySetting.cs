using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectQualitySetting : MonoBehaviour
{
    public Renderer main;
    public Renderer lod;
    private void Awake()
    {
        if (Application.isPlaying)
        {
            SetQuality();
        }
    }


#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            SetQuality();
        }
    }
#endif

    void SetQuality()
    {
        if (Func.lowQuality)
        {
            main.enabled = false;
            lod.enabled = true;
        }
        else
        {
            main.enabled = true;
            lod.enabled = false;
        }
    }
}
