using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG2_Info : MonoBehaviour
{
    public Image targetImage; // The target image, set this in the inspector
    public Image infoBox; // The target image, set this in the inspector
    public float duration = 2f; // Duration of the fade animation, adjust as needed
    public float targetAlpha = 0.6f;
    private bool InfoIsShown;
    public GameObject CardManager;
    private void Start()
    {
        AnimateAlpha(true);
        AnimateInfoPopUp(true);
    }

    public void AnimateAlpha(bool fadeIn)
    {
        if (fadeIn != InfoIsShown)
        {
            // Use StartCoroutine to start the coroutine that handles the fade
            StartCoroutine(fadeIn ? FadeIn() : FadeOut());
        }
    }
    public void AnimateInfoPopUp(bool scaleIn)
    {
        if (scaleIn != InfoIsShown)
        {
            InfoIsShown = scaleIn;
            // Use StartCoroutine to start the coroutine that handles the scale
            StartCoroutine(scaleIn ? ScaleIn() : ScaleOut());
        }

    }

    IEnumerator FadeIn()
    {
        Color c = targetImage.color;
        Color cInfo = infoBox.color;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            c.a = Mathf.Lerp(0, targetAlpha, t);
            cInfo.a = Mathf.Lerp(0, 1f, t);
            targetImage.color = c;
            infoBox.color = c;
            yield return null;
        }
        c.a = targetAlpha; // Ensure alpha is set to 1 at the end
        targetImage.color = c;
        infoBox.color = cInfo;
    }

    IEnumerator FadeOut()
    {
        targetImage.enabled = true;
        Color c = targetImage.color;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            c.a = Mathf.Lerp(targetAlpha, 0, t);
            targetImage.color = c;
            infoBox.color = c;
            yield return null;
        }
        c.a = 0; // Ensure alpha is set to 0 at the end
        targetImage.color = c;
        infoBox.color = c;
        targetImage.enabled = false;
    }
    
    
    IEnumerator ScaleIn()
    {
        infoBox.enabled = true;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            infoBox.rectTransform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1), t);
            yield return null;
        }
        infoBox.rectTransform.localScale = new Vector3(1, 1, 1); // Ensure scale is set to 1 at the end
    }

    IEnumerator ScaleOut()
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            infoBox.rectTransform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t);
            yield return null;
        }
        infoBox.rectTransform.localScale = new Vector3(0, 0, 0); // Ensure scale is set to 0 at the end
        infoBox.enabled = false;
        CardManager.GetComponent<Deck>().MoveTopCardToCenter();
    }
}
