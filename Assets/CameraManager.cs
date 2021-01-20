using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public Transform target;
    public Transform lookat;
    public CinemachineVirtualCamera camera;

    private Transform listener;
    // Start is called before the first frame update
    void Start()
    {

        if (target == null)
        {
            enabled = false;
            return;
        }

        camera.LookAt = lookat;
        camera.Follow = target;
    }
}
