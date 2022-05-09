using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_StaticElectric : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(ConstStringCollection.ENEMY))
        {
            other.GetComponent<IDebuff>().AddDebuff(Element.BlackLightning);
        }
    }
}
