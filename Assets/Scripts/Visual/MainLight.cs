using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MainLight : MonoBehaviour
{
    public Light normal;
    public Light lod;
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
            normal.enabled = false;
            lod.enabled = true;
        }
        else
        {
            normal.enabled = true;
            lod.enabled = false;
        }
    }
}
