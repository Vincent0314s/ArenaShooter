using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSmo : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);

        // Smoothly move the camera towards that target position
    }
}
