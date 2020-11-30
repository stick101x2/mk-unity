using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QualitySet : MonoBehaviour
{
    public Renderer[] shadow_casters = new Renderer[0];
    public Renderer self;
    private void Awake()
    {
        self = GetComponent<Renderer>();
        if (Application.isPlaying)
        {
            SetQuality();
        }
    }
  

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor &&!Application.isPlaying)
        {
            SetQuality();
        }
    }
#endif

    void SetQuality()
    {
        if (shadow_casters.Length <= 0)
            return;

        if (Func.lowQuality)
        {
            LowQuality();
        }
        else
        {
            NormalQuality();
        }
    }

    void LowQuality()
    {
        for (int i = 0; i < shadow_casters.Length; i++)
        {
            shadow_casters[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        self.enabled = true;
        self.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    void NormalQuality()
    {
        for (int i = 0; i < shadow_casters.Length; i++)
        {
            shadow_casters[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        self.enabled = false;
    }
}
