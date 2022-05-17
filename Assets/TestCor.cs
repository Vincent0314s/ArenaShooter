using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCor : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Test01Cor());
        StartCoroutine(Test02Cor());
    }

    IEnumerator Test01Cor() {
        Debug.Log("FirstCor01");
        yield return new WaitForSeconds(2f);
        Debug.Log("FirstCor02");
    }

    IEnumerator Test02Cor() {
        Debug.Log("SecondCor01");
        yield return new WaitForSeconds(1f);
        Debug.Log("SecondCor02");
    }
  
}
