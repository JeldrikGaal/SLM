using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;
using UnityEngine.SceneManagement;

public class BB_Questiom : MonoBehaviour
{
    public TMP_Text textStay;
    public TMP_Text textLeave;
    public TMP_Text textQuestion;
    public Image greyBG;
    public Image arrowLeft;
    public Image arrowRight;
    private float fadeDuration = 0.3f;
    private float _greyBG_Alpha = 0.5f;
    private FontManager _fontManager;
    
    void Start()
    {
        _fontManager = GameObject.FindGameObjectWithTag("FontManager").GetComponent<FontManager>();
        
        textStay.font = _fontManager.GetFont();
        textLeave.font = _fontManager.GetFont();
        textQuestion.font = _fontManager.GetFont();
        
        textStay.text = LocalizationManager.Localize(textStay.text);
        textLeave.text = LocalizationManager.Localize(textLeave.text);
        textQuestion.text = LocalizationManager.Localize(textQuestion.text);
    }

    public void FadeIn()
    {
        SetActive(true);
        greyBG.gameObject.SetActive(true);
        StartCoroutine(Fade(0, 1));  // Fade in from alpha 0 to 1
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1, 0));  // Fade out from alpha 1 to 0
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            SetAlpha(newAlpha);

            yield return null;
        }

        // If fading out, disable the elements after the fade.
        if (endAlpha == 0)
        {
            SetActive(false);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (textStay != null)
        {
            Color c = textStay.color;
            c.a = alpha;
            textStay.color = c;
        }

        if (textLeave != null)
        {
            Color c = textLeave.color;
            c.a = alpha;
            textLeave.color = c;
        }
        
        if (textQuestion != null)
        {
            Color c = textQuestion.color;
            c.a = alpha;
            textQuestion.color = c;
        }

        if (greyBG != null)
        {
            Color c = greyBG.color;
            c.a = Mathf.Lerp(0, _greyBG_Alpha, alpha);
            greyBG.color = c;
        }

        if (arrowLeft != null)
        {
            Color c = arrowLeft.color;
            c.a = alpha;
            arrowLeft.color = c;
        }
        if (arrowRight != null)
        {
            Color c = arrowRight.color;
            c.a = alpha;
            arrowRight.color = c;
        }
    }

    private void SetActive(bool active)
    {
        greyBG.gameObject.SetActive(active);
    }

    public void ReturnToBook()
    {
        SceneManager.LoadScene("Book_Main");
    }
}
