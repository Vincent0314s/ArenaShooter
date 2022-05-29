using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public bool hasAlly;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ShowPath(bool _active) {
        if (hasAlly) { 
            meshRenderer.enabled = false;
            return;
        }

        meshRenderer.enabled = _active;
    }

    public void UpdatePath() {
        meshRenderer.enabled = !hasAlly;
    }
}
