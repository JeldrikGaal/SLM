using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;
using UnityEngine.SceneManagement;

public class MG2_Info : MonoBehaviour
{
    public bool skipTutorial;
    public Image targetImage;
    public Image OrangeImage;
    public Button OrangeButton;
    public Image infoBox_1; 
    public Image infoBox_2; 
    public Image infoBox_3; 
    public Image infoBox_bb; 
    public Image infoBox_tapanywhere; 
    public Button FinalReturnBB; 
    public float duration = 1.5f; 
    public float targetAlpha = 0.75f;
    private bool InfoClosed_1;
    private bool InfoClosed_2;
    private bool InfoClosed_3;
    private bool InfoClosed_bb;
    private bool _canTapBBaway;
    public GameObject CardManager;
    
    [SerializeField] private TMP_Text _text_backtobookbutton;
    [SerializeField] private TMP_Text _text_1;
    [SerializeField] private TMP_Text _text_1_button;
    [SerializeField] private TMP_Text _text_1_headline;
    [SerializeField] private TMP_Text _text_2;
    [SerializeField] private TMP_Text _text_3;
    [SerializeField] private TMP_Text _text_bb;
    [SerializeField] private TMP_Text _text_tapanywhere;
    [SerializeField] private TMP_Text _text_finalreturnButton;
    private FontManager _fontManager;
    [SerializeField] private float _orangeFadeDuration = 0.7f;
    
    private void Start()
    {
        OrangeButton.enabled = false;
        SetOrangeAlpha(0);
        infoBox_1.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_2.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_3.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_bb.rectTransform.localScale = new Vector3(0, 0, 0);
        infoBox_tapanywhere.rectTransform.localScale = new Vector3(0, 0, 0);
        if (skipTutorial)
        {
            this.gameObject.SetActive(false);
            CardManager.GetComponent<Deck>().MoveTopCardToCenter();
            SetOrangeAlpha(1);
            OrangeButton.enabled = true;
        }
        else
        {
            this.gameObject.SetActive(true);
            AnimateAlpha(true);
            StartCoroutine(ScaleIn(infoBox_1, duration));
        }
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
        
        _text_bb.font = _fontManager.GetFont();
        _text_bb.text = LocalizationManager.Localize(_text_bb.text);
        
        _text_tapanywhere.font = _fontManager.GetFont();
        _text_tapanywhere.text = LocalizationManager.Localize(_text_tapanywhere.text);

        _text_backtobookbutton.font = _fontManager.GetFont();
        _text_backtobookbutton.text = LocalizationManager.Localize("BookButton");
    }

    public void Continue_1()
    {
        if (!InfoClosed_1)
        {
            InfoClosed_1 = true;
            StartCoroutine(ScaleOut(infoBox_1, duration));
            StartCoroutine(ShowInfo_bbWithDelay(0.6f));
        }
    }
    
    private IEnumerator ShowInfo_bbWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowInfo_bb();
    }

    private void ShowInfo_bb()
    {
        OrangeButton.enabled = true;
        StartCoroutine(ScaleIn(infoBox_bb, duration));
        orangeTriggerFade(true);
        StartCoroutine(StartButtonScaleLoop(1));
        StartCoroutine(ShowInfo_tapanywhereWithDelay(2f));
    }
    
    private IEnumerator ShowInfo_tapanywhereWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowInfo_tapanywhere();
    }

    private void ShowInfo_tapanywhere()
    {
        StartCoroutine(ScaleIn(infoBox_tapanywhere, 0.2f));
        _canTapBBaway = true;
    }

    public void tappedAnywhere()
    {
        if (_canTapBBaway)
        {
            _canTapBBaway = false;
            InfoClosed_bb = true;
            StartCoroutine(ScaleOut(infoBox_tapanywhere, 0.2f));
            StartCoroutine(ScaleOut(infoBox_bb, duration));
            Continue_tut();
        }
    }
    
    
    public void Continue_tut()
    {
        StartCoroutine(ShowInfo_2WithDelay(1.3f));
        StartCoroutine(ShowCardFlip_2WithDelay(1.9f));
        CardManager.GetComponent<Deck>().MoveTopCardToCenter();
    }
    
    private IEnumerator ShowInfo_2WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowInfo_2();
    }

    private void ShowInfo_2()
    {
        StartCoroutine(ScaleIn(infoBox_2, duration));
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
            StartCoroutine(ScaleOut(infoBox_2, duration));
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
        StartCoroutine(ScaleIn(infoBox_3, duration));
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
            StartCoroutine(ScaleOut(infoBox_3, duration));
            AnimateAlpha(false);
            CardManager.GetComponent<Deck>().RotateTargetCards();
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
    
    
    IEnumerator ScaleIn(Image PopUp, float dur)
    {
        float startTime = Time.time;
        while (Time.time < startTime + dur)
        {
            float t = (Time.time - startTime) / dur;
            PopUp.rectTransform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1), t);
            yield return null;
        }
        PopUp.rectTransform.localScale = new Vector3(1, 1, 1); // Ensure scale is set to 1 at the end
    }

    IEnumerator ScaleOut(Image PopUp, float dur)
    {
        float startTime = Time.time;
        while (Time.time < startTime + dur)
        {
            float t = (Time.time - startTime) / dur;
            PopUp.rectTransform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), t);
            yield return null;
        }
        PopUp.rectTransform.localScale = new Vector3(0, 0, 0); // Ensure scale is set to 0 at the end
        PopUp.enabled = false;
        
    }
    
    public void SetOrangeAlpha(float alpha)
    {
        // Ensure alpha is between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // Set alpha of OrangeImage
        Color c = OrangeImage.color;
        c.a = alpha;
        OrangeImage.color = c;

        // Set alpha of OrangeButton
        c = OrangeButton.image.color;
        c.a = alpha;
        OrangeButton.image.color = c;

        // Set alpha of _text_backtobookbutton
        c = _text_backtobookbutton.color;
        c.a = alpha;
        _text_backtobookbutton.color = c;
    }

    public void orangeTriggerFade(bool fadeIn)
    {
        Debug.Log("so");
        StartCoroutine(orangeFadeAnimation(fadeIn ? 0f : 1f, fadeIn ? 1f : 0f));
    }

    private IEnumerator orangeFadeAnimation(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0;

        while (timeElapsed < _orangeFadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / _orangeFadeDuration);
            SetOrangeAlpha(alpha);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetOrangeAlpha(endAlpha);
    }
    
    public void ScaleButtonUpAndDown(Button button, float targetScale, float duration)
    {
        if (button == null) return;

        StartCoroutine(AnimateButtonScale(button, targetScale, duration));
    }

    private IEnumerator AnimateButtonScale(Button button, float targetScale, float duration)
    {
        var originalScale = button.transform.localScale;
        var targetVector = new Vector3(targetScale, targetScale, targetScale);

        // Animate scale up
        float time = 0;
        while (time <= duration)
        {
            float t = time / duration; // normalized time
            button.transform.localScale = Vector3.Lerp(originalScale, targetVector, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Animate scale down
        time = 0;
        while (time <= duration)
        {
            float t = time / duration; // normalized time
            button.transform.localScale = Vector3.Lerp(targetVector, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure it's back to original scale
        button.transform.localScale = originalScale;
    }
    
    private IEnumerator StartButtonScaleLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        ScaleButtonOnce();
    }

    private void ScaleButtonOnce()
    {
        if (!InfoClosed_bb)
        {
            ScaleButtonUpAndDown(OrangeButton, OrangeButton.transform.localScale.x * 2.3f, 0.75f);
            StartCoroutine(StartButtonScaleLoop(3));
        }
        
    }
    
    public void ReturnToBook()
    {
        SceneManager.LoadScene("Book_Main");
    }
    
    IEnumerator FadeInFinalReturnBB(float duration)
    {
        Button button = FinalReturnBB;
        Color buttonColor = button.image.color;
        Color textColor = button.GetComponentInChildren<TextMeshProUGUI>().color;

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;

            buttonColor.a = normalizedTime;
            textColor.a = normalizedTime;

            button.image.color = buttonColor;
            button.GetComponentInChildren<TextMeshProUGUI>().color = textColor;

            yield return null;
        }

        buttonColor.a = 1;
        textColor.a = 1;

        button.image.color = buttonColor;
        button.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
    }

    public void StartFadeInFinalReturnBB(float duration)
    {
        FinalReturnBB.gameObject.SetActive(true);
        _text_finalreturnButton.font = _fontManager.GetFont();
        _text_finalreturnButton.text = LocalizationManager.Localize(_text_finalreturnButton.text);
        FinalReturnBB.transform.SetAsLastSibling();
        StartCoroutine(FadeInFinalReturnBB(duration));
    }
    
    private IEnumerator ShowFinalButtonAfterTime()
    {
        yield return new WaitForSeconds(3f);
        ShowFinalButton();
    }

    private void ShowFinalButton()
    {
        StartFadeInFinalReturnBB(1f);
    }

    public void ShowFinalButtonAdjusted()
    {
        StartCoroutine(ShowFinalButtonAfterTime());
    }

    
}
