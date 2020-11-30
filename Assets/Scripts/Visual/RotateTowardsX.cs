using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsX : MonoBehaviour
{
    public Transform target;
  
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostition = new Vector3(target.position.x, target.position.y,target.position.z);
       transform.LookAt(targetPostition);
        

    }
}
