using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingObject : MonoBehaviour
{
    BoxCollider boxCollider;
    MeshRenderer meshRender;

    private void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRender = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.layer.Equals(1 << other.gameObject.layer)) {
            meshRender.material.color = new Color(0.5f,0,0,0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        meshRender.material.color = new Color(0, 0.5f, 0, 0.5f);
    }
}
