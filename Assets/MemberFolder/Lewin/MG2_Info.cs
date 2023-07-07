using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;

public class MG2_Info : MonoBehaviour
{
    public Image targetImage;
    public Image OrangeImage;
    public Button OrangeButton;
    public Image infoBox_1; 
    public Image infoBox_2; 
    public Image infoBox_3; 
    public float duration = 1.5f; 
    public float targetAlpha = 0.75f;
    private bool InfoClosed_1;
    private bool InfoClosed_2;
    private bool InfoClosed_3;
    public GameObject CardManager;
    
    [SerializeField] private TMP_Text _text_backtobookbutton;
    [SerializeField] private TMP_Text _text_1;
    [SerializeField] private TMP_Text _text_1_button;
    [SerializeField] private TMP_Text _text_1_headline;
    [SerializeField] private TMP_Text _text_2;
    [SerializeField] private TMP_Text _text_3;
    private FontManager _fontManager;
    
    private void Start()
    {
        OrangeImage.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_1.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_2.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_3.rectTransform.localScale = new Vector3(0, 0, 0);
        AnimateAlpha(true);
        StartCoroutine(ScaleIn(infoBox_1));
        _fontManager = GameObject.FindGameObjectWithTag("FontManager").GetComponent<FontManager>();
        
        _text_1.font = _fontManager.GetFont();
        _text_1.text = LocalizationManager.Localize(_text_1.text);
        
        _text_1_button.font = _fontManager.GetFont();
        _text_1_button.text = LocalizationManager.Localize(_text_1_button.text);
        
        _text_1_headline.font = _fontManager.GetFont();
        _text_1_headline.text = LocalizationManager.Localize(_text_1_headline.text);
        
        _text_2.font = _fontManager.GetFont();
        _text_2.text = LocalizationManager.Localize(_text_2.text);
        
        _text_3.font = _fontManager.GetFont();
        _text_3.text = LocalizationManager.Localize(_text_3.text);
    }

    public void Continue_1()
    {
        if (!InfoClosed_1)
        {
            InfoClosed_1 = true;
            StartCoroutine(ScaleOut(infoBox_1));
            CardManager.GetComponent<Deck>().MoveTopCardToCenter();
            StartCoroutine(ShowInfo_2WithDelay(0.6f));
            StartCoroutine(ShowCardFlip_2WithDelay(1.6f));
        }
        
    }
    
    private IEnumerator ShowInfo_2WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowInfo_2();
    }

    private void ShowInfo_2()
    {
        StartCoroutine(ScaleIn(infoBox_2));
    }
    
    
    private IEnumerator ShowCardFlip_2WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowCardFlip_2();
    }

    private void ShowCardFlip_2()
    {
        CardManager.GetComponent<Deck>().CurrentDraggableCard.transform.SetAsLastSibling();
        CardManager.GetComponent<Deck>().CurrentDraggableCard.EnableDragging(false);
        CardManager.GetComponent<Deck>().CurrentDraggableCard.TogglePulse(true, CardManager.GetComponent<Deck>().CurrentDraggableCard.cardScale * 1.15f, 0.75f);
    }
    
    public void Continue_2()
    {
        if (!InfoClosed_2)
        {
            InfoClosed_2 = true;
            StartCoroutine(ScaleOut(infoBox_2));
            StartCoroutine(ShowCardDrag_3WithDelay(1.2f));
        }
    }
    
    private IEnumerator ShowCardDrag_3WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowCardDrag_3();
        StartCoroutine(MakeCardDragPossible_3WithDelay(0.8f));
    }

    private void ShowCardDrag_3()
    {
        StartCoroutine(ScaleIn(infoBox_3));
    }
    
    
    private IEnumerator MakeCardDragPossible_3WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MakeCardDragPossible_3();
    }

    private void MakeCardDragPossible_3()
    {
        CardManager.GetComponent<Deck>().CurrentDraggableCard.EnableDragging(true);
        CardManager.GetComponent<Deck>().CurrentDraggableCard.tutWaitForDrag = true;
        CardManager.GetComponent<Deck>().CurrentDraggableCard.CheckForDragging();
    }

    public void Continue_3()
    {
        if (!InfoClosed_3)
        {
            InfoClosed_3 = true;
            StartCoroutine(ScaleOut(infoBox_3));
            AnimateAlpha(false);
        }
    }
    
    
    
    
    
    

    
    
    public void AnimateAlpha(bool fadeIn)
    {
        StartCoroutine(fadeIn ? FadeIn() : FadeOut());
    }
    public void AnimateInfoPopUp(bool scaleIn)
    {
        
    }

    IEnumerator FadeIn()
    {
        Color c = targetImage.color;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            c.a = Mathf.Lerp(0, targetAlpha, t);
            targetImage.color = c;
            //infoBox.color = c;
            yield return null;
        }
        c.a = targetAlpha; // Ensure alpha is set to 1 at the end
        //targetImage.color = c;
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
            yield return null;
        }
        c.a = 0; // Ensure alpha is set to 0 at the end
        targetImage.color = c;
        targetImage.enabled = false;
    }
    
    
    IEnumerator ScaleIn(Image PopUp)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            PopUp.rectTransform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1), t);
            yield return null;
        }
        PopUp.rectTransform.localScale = new Vector3(1, 1, 1); // Ensure scale is set to 1 at the end
    }

    IEnumerator ScaleOut(Image PopUp)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            PopUp.rectTransform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t);
            yield return null;
        }
        PopUp.rectTransform.localScale = new Vector3(0, 0, 0); // Ensure scale is set to 0 at the end
        PopUp.enabled = false;
        
    }
}
