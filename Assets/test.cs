using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Camera cam;
    public Transform c;

    public Color selected;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider col = hit.collider as MeshCollider;
        c.position = hit.point;
        if (rend == null
        || rend.sharedMaterial == null
        || rend.sharedMaterial.mainTexture == null
        || col == null
        ) return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        Vector2 tiling = rend.material.mainTextureScale;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        selected = tex.GetPixel(Mathf.FloorToInt(pixelUV.x * tiling.x), Mathf.FloorToInt(pixelUV.y * tiling.y));

        //tex.Resize(256, 256);
    }
}
