using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModelLocationForce))]
public class ModeForceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ModelLocationForce mdf = (ModelLocationForce)target;
        if (GUILayout.Button("Defualts"))
        {
            mdf.GetDefualt();

        }
        if (GUILayout.Button("Reset"))
        {
            mdf.SetBones();
            
        }
    }
}
