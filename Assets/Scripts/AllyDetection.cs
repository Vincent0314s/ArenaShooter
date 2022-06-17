using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyDetection : MonoBehaviour
{
    [SerializeField]
    private PathObject m_PathObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals(ConstStringCollection.ALLY)) {
            m_PathObject.hasAlly = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(ConstStringCollection.ALLY))
        {
            m_PathObject.hasAlly = false;
        }
    }
}
