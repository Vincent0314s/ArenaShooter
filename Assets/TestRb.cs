using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRb : MonoBehaviour
{

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(-transform.forward * 150f);
            //StartCoroutine(DecreaseForce());
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            rb.Sleep();
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            Debug.Log("Wakeup");
            rb.WakeUp();
        }
    }

    IEnumerator DecreaseForce() {
        var rbZ = Mathf.Abs(rb.velocity.z);

        while (Mathf.Abs(rb.velocity.z) > 0) {
            rbZ -= 1f;
            rb.velocity = new Vector3(0,0,rbZ);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
