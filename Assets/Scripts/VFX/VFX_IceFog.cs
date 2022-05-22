using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_IceFog : MonoBehaviour
{
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(ConstStringCollection.ENEMY)) {
            other.GetComponent<IDebuff>().AddDebuff(Element.Ice);
            StartCoroutine(WaitForDisable());
        }
    }

    IEnumerator WaitForDisable() {
        yield return new WaitForSeconds(0.25f);
        sphereCollider.enabled = false;
    }
}
