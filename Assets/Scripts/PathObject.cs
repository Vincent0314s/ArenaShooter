using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;
    public bool hasAlly;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    public void ShowPath(bool _active) {
        if (hasAlly) { 
            m_meshRenderer.enabled = false;
            m_meshCollider.enabled = false;
            return;
        }

        m_meshRenderer.enabled = _active;
    }

    public void UpdatePath() {
        m_meshRenderer.enabled = !hasAlly;
        m_meshCollider.enabled = !hasAlly;
    }
}
