using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ModelLocationForce : MonoBehaviour
{
	public Transform[] bones;
	public bool active = true;
	public bool edit = false;
	
	public bool position;
	public bool scale;
	public bool rotation;
	public Vector3[] bonePosition;
	public Vector3[] boneScale;
	public Quaternion[] boneRotation;
	bool gotdefs;
	// Start is called before the first frame update
	/*
    void Start()
    {
		
		if (!Application.isPlaying)
			return;
		GetDefualt();
	}*/

	public void GetDefualt()
	{
		bonePosition = new Vector3[bones.Length];
		boneScale = new Vector3[bones.Length];
		boneRotation = new Quaternion[bones.Length];
		for (int i = 0; i < bones.Length; i++) // goes through each transform in bones array 
		{
			//	Transform bone = bones[i];  // gets refence to current bone index
			// sets bonePosition, boneScale, boneRotation to bone
			bonePosition[i] = bones[i].localPosition;
			boneScale[i] = bones[i].localScale;
			boneRotation[i] = bones[i].localRotation;
		}
		if (bones.Length > 0)
			active = true;
	}
	public void SetBones()
	{
		for (int i = 0; i < bones.Length; i++)
		{
			//Transform bone = bones[i];
			if (position)
				bones[i].localPosition = bonePosition[i];
			if (scale)
				bones[i].localScale = boneScale[i];
			if (rotation)
				bones[i].localRotation = boneRotation[i];
		}
	}
    // Update is called once per frame
    void LateUpdate()
    {
		if (!Application.isPlaying  &&!edit)
			return;
		if (active)
		{
			for (int i = 0; i < bones.Length; i++)
			{
				//Transform bone = bones[i];
				if(position)
					bones[i].localPosition = bonePosition[i];
				if(scale)
					bones[i].localScale = boneScale[i];
				if(rotation)
					bones[i].localRotation = boneRotation[i];
			}
		}
		
	}
}
