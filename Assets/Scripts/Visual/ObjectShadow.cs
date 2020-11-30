using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectShadow : MonoBehaviour
{
    public Behaviour shadowDecal;
    private void Update()
    {

        if(QualitySettings.GetQualityLevel() == 0 && !shadowDecal.enabled)
        {
            shadowDecal.enabled = true;
        }
        else if (QualitySettings.GetQualityLevel() != 0 && shadowDecal.enabled)
        {
            shadowDecal.enabled = false;
        }
    }
}
