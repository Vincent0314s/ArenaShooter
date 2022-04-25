using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatCamera : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,cam.transform.rotation * Vector3.up);
    }
}
