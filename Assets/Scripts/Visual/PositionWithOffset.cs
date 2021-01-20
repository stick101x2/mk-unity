using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionWithOffset : MonoBehaviour
{
    public Transform target;
    public Transform set;
    public Vector3 offset;
    public bool autoSetOffset = true;
    // Start is called before the first frame update
    void Start()
    {
        if (!autoSetOffset)
            return;
        offset.x = target.position.x - transform.position.x;
        offset.y = target.position.y - transform.position.y;
        offset.z = target.position.z - transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        set.position = target.position + offset;
    }
}
