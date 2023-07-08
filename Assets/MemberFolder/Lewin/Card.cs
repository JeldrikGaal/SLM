using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lean.Touch;
using TMPro;
using Assets.SimpleLocalization;
using Unity.VisualScripting;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float cardScale = 0.5f;
    public Image pictureSide; 
    public Image textSide;
    private bool isPictureUp = true;
    private Vector3 originalPosition;
    private bool dragging;
    private bool actuallyDragging;
    private bool draggingAvailable;
    private bool flippable;
    private bool isAnimating = false;
    private Coroutine flipAnimationCoroutine;
    private float flipDuration = 0.1f;
    private float animationProgress = 0f;
    private bool isAnimationReversed = false;
    private Coroutine flipCoroutine;
    private bool _currentlyTraceable = true;
    
    private Coroutine highlightCoroutine;
    private bool isHighlighted = false;

    private Vector3 highlightedScale = new Vector3(1.15f, 1.15f, 1.15f); // Adjust this to the desired scale when highlighted
    private Quaternion highlightedRotation = Quaternion.Euler(0, 0, 10); // Adjust this to the desired rotation when highlighted

    public Image _backIamge;
    public Image _frontImage;

    public float MoveToCenterDuration = 0.5f;
    private bool isMovingCardToCenter;
    
    public Canvas yourCanvas;

    [SerializeField] private TMP_Text _text;
    private FontManager _fontManager;

    // Get a reference to the LeanDragTranslate component
    private LeanDragTranslate leanDragTranslate;
    
    
    private CardData cardData;
    private Graphic graphicComponent_pictureSide;
    private Graphic graphicComponent_textSide;

    private Deck DeckRef;
    public Image DropInfo;
    private DragOverManager dragOverManager;
    private bool _droppable;
    private GameObject _otherCard;

    public bool _placedOnTargetPile;
    private bool _animatingToOtherCardAtm;
    private TPC LastTPC;

    private bool _shouldScreenShakeOnDrop;
    private float _currentY;
    
    private Vector3 initialScale; // store the original scale
    private Coroutine pulseCoroutine; // store the coroutine reference
    private bool _isPulsing = false;
    public bool tutWaitForDrag = false;
    private string _regularText;
    private string _boldText;
    private Coroutine boldCoroutine = null;

    public float DurationUntilTurnBold = 15;
    
    public void Init(int ID, CardDatabase pCardDatabase, bool pDraggable, bool pFlippable, Canvas pCardCanvas, Deck pDeck, DragOverManager pDragOverManager)
    {
        cardData = pCardDatabase.cards[ID];
        SetPicture(cardData.BackSprite, cardData.FrontSprite);
        _text.fontSize = getFontSizeBasedOnLanguage();
        SetCardSide(true);
        EnableDragging(pDraggable);
        EnableFlippable(pFlippable);
        yourCanvas = pCardCanvas;
        _boldText = LocalizationManager.Localize(cardData.LocalizationKey);
        _regularText = RemoveBoldTags(_boldText);
        _text.text = _regularText;
        DeckRef = pDeck;
        dragOverManager = pDragOverManager;
        if (cardData.FrontSprite.name == "Ceramic Card Front")
        {
            this.GetComponent<LeanDragTranslate>().Damping = 5;
            _shouldScreenShakeOnDrop = true;
        }
        
    }

    private float getFontSizeBasedOnLanguage()
    {
        if (LocalizationManager.Language == "English") return GetFloatFromCSV(cardData.fontSizes, 0);
        if (LocalizationManager.Language == "German") return GetFloatFromCSV(cardData.fontSizes, 1);
        if (LocalizationManager.Language == "Simplified English") return GetFloatFromCSV(cardData.fontSizes, 2);
        if (LocalizationManager.Language == "Simplified German") return GetFloatFromCSV(cardData.fontSizes, 3);
        return 5;
    }
    
    private void Awake()
    {
        graphicComponent_pictureSide = pictureSide.GetComponent<Graphic>();
        graphicComponent_textSide = pictureSide.GetComponent<Graphic>();
        leanDragTranslate = GetComponent<LeanDragTranslate>();
        _fontManager = GameObject.FindGameObjectWithTag("FontManager").GetComponent<FontManager>();
        _text.font = _fontManager.GetFont();
        transform.localScale = new Vector3(cardScale, cardScale, cardScale);
        textSide.rectTransform.localScale = new Vector3(0, 1, 1);
        ShowDropInfo(false, null);
        _currentY = transform.position.y;
        initialScale = transform.localScale; // store the initial scale in Awake

    }

    private void OnEnable()
    {
        // Hook into the OnFingerTap event
        //LeanTouch.OnFingerTap += HandleFingerTap;
    }

    private void OnDisable()
    {
        // Unhook from the OnFingerTap event
        //LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    public void HandleFingerTap(LeanFinger finger)
    {

    }

    public void SetPicture(Sprite pPic, Sprite pFront)
    {
        _frontImage.sprite = pFront;
        _backIamge.sprite = pPic;
    }
    
    // Flip the card
    public void Flip()
    {
        if ((!dragging || !draggingAvailable) && flippable)
        {
            SetCardSide(!isPictureUp);
        }
    }

    void DevCardZero()
    {
        //pictureSide.transform.localScale = new Vector3(0, 0, 0);
    }
    public void SetCardSide(bool picture)
    {
        isPictureUp = picture;
        pictureSide.enabled = picture;
        textSide.enabled = !picture;
        // Enable/disable all children of pictureSide and textSide
        for (int i = 0; i < pictureSide.transform.childCount; i++)
        {
            pictureSide.transform.GetChild(i).gameObject.SetActive(isPictureUp);
        }

        for (int i = 0; i < textSide.transform.childCount; i++)
        {
            textSide.transform.GetChild(i).gameObject.SetActive(!isPictureUp);
        }
    }

    public void FlipAnimated()
    {
        if (((!dragging || !draggingAvailable) && flippable && !_animatingToOtherCardAtm)) 
        {
            if (flipCoroutine != null)
            {
                StopCoroutine(flipCoroutine);
            }

            if (isPictureUp)
            {
                flipCoroutine = StartCoroutine(FlipDown());
            }
            else
            {
                flipCoroutine = StartCoroutine(FlipUp());
            }

            if (_isPulsing)
            {
                TogglePulse(false, cardScale, 0.3f);
                DeckRef.MG2_Info_Component.Continue_2();
            }
        }
        else
        {
            if (!dragging)
            {
                //Shake();
            }
        }
    }
    
    private IEnumerator FlipDown()
    {
    isPictureUp = false;
    float startTime = Time.time;
    float endTime = startTime + flipDuration;

    // Animate the pictureSide scale to 0
    while (Time.time <= endTime)
    {
        float t = (Time.time - startTime) / flipDuration;
        pictureSide.rectTransform.localScale = new Vector3(1 - t, 1, 1);
        yield return null;
    }

    // Ensure that the pictureSide scale is set to 0 and deactivate it
    pictureSide.rectTransform.localScale = Vector3.zero;
    pictureSide.enabled = false;

    for (int i = 0; i < pictureSide.transform.childCount; i++)
    {
        pictureSide.transform.GetChild(i).gameObject.SetActive(false);
    }
    // Activate textSide
    textSide.enabled = true;

    for (int i = 0; i < textSide.transform.childCount; i++)
    {
        textSide.transform.GetChild(i).gameObject.SetActive(true);
    }
    // Reset animation parameters and animate the textSide scale to 1
    startTime = Time.time;
    endTime = startTime + flipDuration;

    while (Time.time <= endTime)
    {
        float t = (Time.time - startTime) / flipDuration;
        textSide.rectTransform.localScale = new Vector3(t, 1, 1);
        yield return null;
    }

    // Ensure that the textSide scale is set to 1
    textSide.rectTransform.localScale = Vector3.one;
    }

    private IEnumerator FlipUp()
    {
    isPictureUp = true;
    
    float startTime = Time.time;
    float endTime = startTime + flipDuration;

    // Animate the textSide scale to 0
    while (Time.time <= endTime)
    {
        float t = (Time.time - startTime) / flipDuration;
        textSide.rectTransform.localScale = new Vector3(1 - t, 1, 1);
        yield return null;
    }

    // Ensure that the textSide scale is set to 0 and deactivate it
    textSide.rectTransform.localScale = Vector3.zero;
    textSide.enabled = false;

    for (int i = 0; i < textSide.transform.childCount; i++)
    {
        textSide.transform.GetChild(i).gameObject.SetActive(false);
    }
    // Activate pictureSide
    pictureSide.enabled = true;

    for (int i = 0; i < pictureSide.transform.childCount; i++)
    {
        pictureSide.transform.GetChild(i).gameObject.SetActive(true);
    }
    // Reset animation parameters and animate the pictureSide scale to 1
    startTime = Time.time;
    endTime = startTime + flipDuration;

    while (Time.time <= endTime)
    {
        float t = (Time.time - startTime) / flipDuration;
        pictureSide.rectTransform.localScale = new Vector3(t, 1, 1);
        yield return null;
    }

    // Ensure that the pictureSide scale is set to 1
    pictureSide.rectTransform.localScale = Vector3.one;
    }
    
   private IEnumerator FlipAnimation()
   {
    isAnimating = true;
    float duration = 0.5f; // Set your desired animation duration here

    Image sideUp = isPictureUp ? pictureSide : textSide;
    Image sideDown = isPictureUp ? textSide : pictureSide;

    // Record the starting time and the progress at the start of the animation
    float startTime = Time.time;
    float startProgress = isAnimationReversed ? (1 - animationProgress) : animationProgress;
    float endTime = startTime + duration * (1 - startProgress);

    // Animate the scale of sideUp to 0
    while (Time.time <= endTime && !isAnimationReversed)
    {
        animationProgress = startProgress + (Time.time - startTime) / duration;
        sideUp.rectTransform.localScale = new Vector3(1 - animationProgress, 1, 1);
        yield return null;
    }

    // If the animation was reversed, animate the scale of sideUp back to 1
    if (isAnimationReversed)
    {
        sideUp.rectTransform.localScale = Vector3.one;
        isAnimating = false;
        yield break;
    }

    // If the animation completed normally, deactivate sideUp and flip the card
    sideUp.rectTransform.localScale = Vector3.zero;
    sideUp.enabled = false;
        
    for (int i = 0; i < sideUp.transform.childCount; i++)
    {
        sideUp.transform.GetChild(i).gameObject.SetActive(false);
    }
    isPictureUp = !isPictureUp;

    sideUp = isPictureUp ? pictureSide : textSide;
    sideDown = isPictureUp ? textSide : pictureSide;
    sideDown.enabled = true;

    for (int i = 0; i < sideDown.transform.childCount; i++)
    {
        sideDown.transform.GetChild(i).gameObject.SetActive(true);
    }

    // Reset the starting time, progress and end time for the second part of the animation
    startTime = Time.time;
    startProgress = isAnimationReversed ? (1 - animationProgress) : animationProgress;
    endTime = startTime + duration * startProgress;

    // Animate the scale of sideDown from 0 to 1
    while (Time.time <= endTime && !isAnimationReversed)
    {
        animationProgress = startProgress + (Time.time - startTime) / duration;
        sideDown.rectTransform.localScale = new Vector3(animationProgress, 1, 1);
        yield return null;
    }

    // If the animation was reversed, animate the scale of sideDown back to 0
    if (isAnimationReversed)
    {
        sideDown.rectTransform.localScale = Vector3.zero;
        sideDown.enabled = false;
        isPictureUp = !isPictureUp;
    }
    else
    {
        // If the animation completed normally, ensure the scale of sideDown is set to 1
        sideDown.rectTransform.localScale = Vector3.one;
    }

    isAnimating = false;
    isAnimationReversed = false;
    }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        actuallyDragging = true;
        CheckForDragging();
    }

    public void CheckForDragging()
    {
        if (draggingAvailable && actuallyDragging)
        {
            SetHighlight(true);
            transform.SetAsLastSibling();
            dragOverManager.SetActiveDraggingCard(this);
            graphicComponent_pictureSide.raycastTarget = false;
            graphicComponent_textSide.raycastTarget = false;
            DeckRef.SetPileCollidersTraceable(true);
            if (tutWaitForDrag)
            {
                tutWaitForDrag = false;
                DeckRef.MG2_Info_Component.Continue_3();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        actuallyDragging = false;
        if (draggingAvailable) SetHighlight(false);
        Invoke("SetDraggingFalse", 0.2f);
        // Just moving it back for now
        //transform.position = originalPosition;
        graphicComponent_pictureSide.raycastTarget = true;
        graphicComponent_textSide.raycastTarget = true;
        DeckRef.SetPileCollidersTraceable(false);
        dragOverManager.SetActiveDraggingCard(null);
        if (_droppable && _currentlyTraceable)
        {
            Card otherCardScript = _otherCard.GetComponent<Card>();
            if (otherCardScript.cardData.MatchingID == cardData.ID && otherCardScript.cardData.MatchingID != -1)
            {
                //if (!otherCardScript.isPictureUp) otherCardScript.FlipAnimated();
                otherCardScript.EnableDragging(false);
                otherCardScript.EnableFlippable(false);
                _placedOnTargetPile = true;
                LastTPC.TopCardGO = gameObject;
                LastTPC.cardCount++;
                EnableDragging(false);
                
                if (boldCoroutine != null)
                {
                    StopCoroutine(boldCoroutine);
                    boldCoroutine = null;
                }
                _text.text = _regularText;
                
                if (_shouldScreenShakeOnDrop) DeckRef.FinalParentGO.GetComponent<ShakeUI>().StartShake();
                StartCoroutine(MoveToOtherCard(LastTPC.BottomCardGO, 0.15f));
                if (DeckRef.DeckTopCard != null)
                {
                    DeckRef.MoveTopCardToCenter();
                }
                else
                {
                    //DeckRef.NextDeck();
                    DeckRef.MoveTopCardToCenter();
                }
            }
            else
            {
                //DeckRef.AnimateVignetteAlpha(0.18f, 0.4f);
                SetTraceable(false);
                Shake();
                MoveToCenterDuration = 1.2f;
                StartCoroutine(ReturnCardAfterTimeWithDelay(0.8f));
            }

            
            ShowDropInfo(false, null);
        }
        if (draggingAvailable && _shouldScreenShakeOnDrop) DeckRef.FinalParentGO.GetComponent<ShakeUI>().StartShake();
    }

    private IEnumerator ReturnCardAfterTimeWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveToCenter();
    }
    
    
    void SetDraggingFalse()
    {
        if (!actuallyDragging)
        {
            dragging = false;  
        }
    }

    // Add the new function to enable/disable the LeanDragTranslate component
    public void EnableDragging(bool shouldEnable)
    {
        draggingAvailable = shouldEnable;
        if (leanDragTranslate != null)
        {
            leanDragTranslate.enabled = shouldEnable;
        }
        else
        {
            Debug.LogError("LeanDragTranslate component not found on " + gameObject.name);
        }
    }
    public void MakeInteractable()
    {
        EnableDragging(true);
        EnableFlippable(true);
    }
    public void MakeInteractableAfterTime()
    {
        Invoke("MakeInteractable", MoveToCenterDuration);
    }
    public void EnableFlippable(bool shouldEnable)
    {
        flippable = shouldEnable;
    }
    public void SetHighlight(bool pHighlight)
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        if (pHighlight)
        {
            highlightCoroutine = StartCoroutine(EnterHighlightState());
        }
        else
        {
            highlightCoroutine = StartCoroutine(LeaveHighlightState());
        }
    }
    private IEnumerator EnterHighlightState()
    {
        isHighlighted = true;

        float duration = 0.13f;
        float startTime = Time.time;
        float endTime = startTime + duration;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = highlightedScale * cardScale; // This line has been changed
        Quaternion startRotation = transform.rotation;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.rotation = Quaternion.Lerp(startRotation, highlightedRotation, t);
            yield return null;
        }

        transform.localScale = targetScale;
        transform.rotation = highlightedRotation;
    }
    private IEnumerator LeaveHighlightState()
    {
        isHighlighted = false;

        float duration = 0.13f;
        float startTime = Time.time;
        float endTime = startTime + duration;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one * cardScale; // This line has been changed
        Quaternion startRotation = transform.rotation;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
            yield return null;
        }

        transform.localScale = targetScale;
        transform.rotation = Quaternion.identity;
    }
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }
    private IEnumerator ShakeCoroutine()
    {
        int numberOfShakes = 5; // Adjust this as needed
        float shakeDuration = 0.1f; // Adjust this as needed
        float shakeMagnitude = 10f; // Adjust this as needed

        int direction = 1; // This will be used to alternate the direction of the rotation

        for (int i = 0; i < numberOfShakes; i++)
        {
            Quaternion startRotation = transform.rotation;
            float randomRotation = Random.Range(5f, shakeMagnitude) * direction; // Multiply by direction
            Quaternion endRotation = Quaternion.Euler(0, 0, randomRotation);
            float startTime = Time.time;
            while (Time.time < startTime + shakeDuration)
            {
                float t = (Time.time - startTime) / shakeDuration;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }

            direction *= -1; // Switch the direction for the next shake

            yield return null; // Ensure at least one frame passes
        }

        // Smoothly return to original rotation
        Quaternion finalStartRotation = transform.rotation;
        Quaternion finalEndRotation = Quaternion.identity;
        float finalStartTime = Time.time;
        while (Time.time < finalStartTime + shakeDuration)
        {
            float t = (Time.time - finalStartTime) / shakeDuration;
            transform.rotation = Quaternion.Lerp(finalStartRotation, finalEndRotation, t);
            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }
    
    private IEnumerator ActivateBoldTextWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateBoldText();
    }

    private void ActivateBoldText()
    {
        if (!_placedOnTargetPile) _text.text = _boldText;
    }
    
    public void MoveToCenter()
    {
        // Get the current y position of the card
        StartCoroutine(MoveToPosition(new Vector2(Screen.width / 2, DeckRef.startObject.transform.position.y)));
        boldCoroutine = StartCoroutine(ActivateBoldTextWithDelay(DurationUntilTurnBold));
    }
    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        SetTraceable(false);
        Vector2 startPosition = this.transform.position;
        Quaternion startRotation = this.transform.rotation; // Current rotation
        Quaternion endRotation = Quaternion.Euler(0, 0, 0); // Target rotation
        float startTime = Time.time;
        while (Time.time < startTime + MoveToCenterDuration)
        {
            float t = (Time.time - startTime) / MoveToCenterDuration;
            t = Mathf.SmoothStep(0.0f, 1.0f, t); // Modify t to create easing effect
            this.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t); // Interpolate rotation
            yield return null;
        }
        this.transform.position = targetPosition; // Ensure card is at target position at the end
        this.transform.rotation = endRotation; // Ensure card rotation is 0 at the end
        SetTraceable(true);
    }

    public void ShowDropInfo(bool show, GameObject pOtherHitbox)
    {
        _droppable = show;
        Color color = DropInfo.color;
        color.a = show ? 1f : 0f;
        DropInfo.color = color;
        //DropInfo.transform.GetChild(0).localScale = show ? Vector3.one : Vector3.zero;
        if (show)
        {
            LastTPC = pOtherHitbox.transform.parent.gameObject.GetComponent<TPC>();
            _otherCard = LastTPC.TopCardGO;
        }
    }
    IEnumerator MoveToOtherCard(GameObject otherCard, float duration)
    {
        _animatingToOtherCardAtm = true;
        Vector3 offset = new Vector3(-5, -5, 0);
        Vector3 startPosition = this.transform.position; // Starting position
        Vector3 endPosition = new Vector3(otherCard.transform.position.x, otherCard.transform.position.y - (Screen.height* 0.035f), 0) + offset * LastTPC.cardCount; // Target position

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration; // normalized time
            Vector3 nextPosition = Vector3.Lerp(startPosition, endPosition, t); // Linear interpolation
            // here we only update the x and y position
            this.transform.position = new Vector3(nextPosition.x, nextPosition.y, this.transform.position.z);
            yield return null;
        }

        // Ensure final position is accurate
        this.transform.position = new Vector3(endPosition.x, endPosition.y, this.transform.position.z);
        _animatingToOtherCardAtm = false;
    }

    public void SetTraceable(bool pTraceable)
    {
        _currentlyTraceable = pTraceable;
        pictureSide.raycastTarget = pTraceable;
        textSide.raycastTarget = pTraceable;
        _text.raycastTarget = pTraceable;
    }

    public void FlipIfPictureSideIsUp()
    {
        if (isPictureUp) FlipAnimated();
    }

    public void TogglePulse(bool shouldPulse, float targetScale, float duration)
    {
        if (pulseCoroutine != null) // if pulseCoroutine is not null, stop it
        {
            _isPulsing = false;
            StopCoroutine(pulseCoroutine);
            SetHighlight(false);
        }

        if (shouldPulse)
        {
            _isPulsing = true;
            Color color = DropInfo.color;
            color.a = shouldPulse ? 1f : 0f;
            DropInfo.color = color;
            pulseCoroutine = StartCoroutine(Pulse(targetScale, duration));
        }
        else
        {
            _isPulsing = false;
            Color color = DropInfo.color;
            color.a = shouldPulse ? 1f : 0f;
            DropInfo.color = color;
            // make sure to animate back to the original scale
            StartCoroutine(ScaleToTarget(initialScale, duration));
        }
    }

    // coroutine to create the pulse effect
    private IEnumerator Pulse(float targetScale, float duration)
    {
        while (true)
        {
            yield return ScaleToTarget(Vector3.one * targetScale, duration);
            yield return ScaleToTarget(initialScale, duration);
        }
    }

    // coroutine to scale the object to a target scale over a certain duration
    private IEnumerator ScaleToTarget(Vector3 target, float duration)
    {
        float time = 0;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
    }
    
    public float GetFloatFromCSV(string csv, int index)
    {
        string[] split = csv.Split(',');

        if(index < split.Length)
        {
            if(float.TryParse(split[index], out float value))
            {
                return value;
            }
            else
            {
                return 0f;
            }
        }
        else
        {
            return 0f;
        }
    }

    public string RemoveBoldTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
    
        // Remove both opening and closing bold tags
        return input.Replace("<b>", "").Replace("</b>", "");
    }
    
    
    public void FinalHighlight(bool pHighlight)
    {
        highlightedRotation = Quaternion.Euler(0, 0, 0);
        highlightedScale = new Vector3(1.2f, 1.2f, 1.2f);
        Color color = DropInfo.color;
        color.a = pHighlight ? 1f : 0f;
        DropInfo.color = color;    
        
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        if (pHighlight)
        {
            highlightCoroutine = StartCoroutine(EnterHighlightState());
        }
        else
        {
            highlightCoroutine = StartCoroutine(LeaveHighlightState());
        }
    }

}
