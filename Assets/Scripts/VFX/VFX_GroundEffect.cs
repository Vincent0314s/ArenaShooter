using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_GroundEffect : MonoBehaviour
{
    public Element element;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(ConstStringCollection.ENEMY))
        {
            other.GetComponent<IDebuff>().AddDebuff(element);
        }
    }
}
