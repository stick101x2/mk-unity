using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSRainbow : MonoBehaviour
{
    public Material space;
    public float speed = 0.001f;
    Vector2 space_moveSpeed;

    private void Awake()
    {
        Vector2 random = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));
        space_moveSpeed = new Vector2(Random.Range(-1f, 1f)* speed, Random.Range(-1f, 1f) * speed);
        space.mainTextureOffset = random;
    }
    // Update is called once per frame
    void Update()
    {
        space.mainTextureOffset += space_moveSpeed * Time.deltaTime;

    }
}
