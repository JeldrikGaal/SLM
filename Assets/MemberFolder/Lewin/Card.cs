using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lean.Touch;
using TMPro;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
    
    private Coroutine highlightCoroutine;
    private bool isHighlighted = false;

    private readonly Vector3 highlightedScale = new Vector3(1.15f, 1.15f, 1.15f); // Adjust this to the desired scale when highlighted
    private readonly Quaternion highlightedRotation = Quaternion.Euler(0, 0, 10); // Adjust this to the desired rotation when highlighted

    public Image Picture;

    public float MoveToCenterDuration = 0.5f;
    private bool isMovingCardToCenter;
    
    public Canvas yourCanvas;

    [SerializeField] private TMP_Text _text;
    private FontManager _fontManager;
    
    

    // Get a reference to the LeanDragTranslate component
    private LeanDragTranslate leanDragTranslate;
    
    private void Awake()
    {
        // Get the LeanDragTranslate component
        leanDragTranslate = GetComponent<LeanDragTranslate>();
        _fontManager = GameObject.FindGameObjectWithTag("FontManager").GetComponent<FontManager>();
        _text.font = _fontManager.GetFont();
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

    public void SetPicture(Sprite pPicture)
    {
        Picture.sprite = pPicture;
    }
    
    // Flip the card
    public void Flip()
    {
        if ((!dragging || !draggingAvailable) && flippable)
        {
            SetCardSide(!isPictureUp);
        }
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
        if (((!dragging || !draggingAvailable) && flippable)) 
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
        }
        else
        {
            if (!dragging)
            {
                Shake();
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
        if (draggingAvailable)
        {
            SetHighlight(true);
        }
        //originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        actuallyDragging = false;
        SetHighlight(false);
        Invoke("SetDraggingFalse", 0.2f);
        // Just moving it back for now
        //transform.position = originalPosition;
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
        Quaternion startRotation = transform.rotation;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, highlightedScale, t);
            transform.rotation = Quaternion.Lerp(startRotation, highlightedRotation, t);
            yield return null;
        }

        transform.localScale = highlightedScale;
        transform.rotation = highlightedRotation;
    }

    private IEnumerator LeaveHighlightState()
    {
        isHighlighted = false;

        float duration = 0.13f;
        float startTime = Time.time;
        float endTime = startTime + duration;

        Vector3 startScale = transform.localScale;
        Quaternion startRotation = transform.rotation;

        while (Time.time <= endTime)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
            yield return null;
        }

        transform.localScale = Vector3.one;
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

    
    public void MoveToCenter()
    {
        // Use StartCoroutine to start the coroutine that handles the move
        StartCoroutine(MoveToPosition(new Vector2(Screen.width / 2, Screen.height / 2)));
    }

    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = this.transform.position;
        float startTime = Time.time;
        while (Time.time < startTime + MoveToCenterDuration)
        {
            float t = (Time.time - startTime) / MoveToCenterDuration;
            t = Mathf.SmoothStep(0.0f, 1.0f, t); // Modify t to create easing effect
            this.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        this.transform.position = targetPosition; // Ensure card is at target position at the end
    }
    
}
