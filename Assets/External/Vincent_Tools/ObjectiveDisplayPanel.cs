using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveDisplayPanel : MonoBehaviour
{
    public bool isFadingIn;
    public float timeToFadeOut = 3f;
    public float fadeOutDuration = 1f;

    public float timeToFadeIn = 1f;
    public float fadeInDration = 1f;

    private void OnEnable()
    {
        if (!isFadingIn)
        {
            GetComponent<Image>().canvasRenderer.SetAlpha(1);
            GetComponentInChildren<Text>().canvasRenderer.SetAlpha(1);

            StartCoroutine(FadeOut());
        }
        else {
            GetComponent<Image>().canvasRenderer.SetAlpha(0);

            StartCoroutine((FadeIn()));
        }
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(timeToFadeIn);
        GetComponent<Image>().CrossFadeAlpha(1, fadeInDration, false);
    }

    IEnumerator FadeOut() {
        yield return new WaitForSeconds(timeToFadeOut);
        GetComponent<Image>().CrossFadeAlpha(0, fadeOutDuration, false);
        GetComponentInChildren<Text>().CrossFadeAlpha(0, fadeOutDuration, false);
        yield return new WaitForSeconds(fadeOutDuration);
        gameObject.SetActive(false);
    }

   
}
