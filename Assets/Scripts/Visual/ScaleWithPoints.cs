using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithPoints : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float offset = 1;

    float distance;
    Vector3 scale;
    
    // Update is called once per frame
    void Update()
    {
         distance = Vector3.Distance(end.position, start.position);
         scale = new Vector3(transform.localScale.x, transform.localScale.y, distance *  offset);

        transform.localScale = scale;
    }
}
