using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Func 
{
    public static bool lowQuality
    {
        get
        {
            if (QualitySettings.GetQualityLevel() == 0)
            {
                return true;
            }

            return false;
        }
    }
}
