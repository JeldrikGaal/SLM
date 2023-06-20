using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lean.Touch;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image pictureSide; 
    public Image textSide;
    private bool isPictureUp = true;
    private Vector3 originalPosition;
    private bool dragging;
    private bool draggingAvailable;
    private bool flippable;
    private bool isAnimating = false;
    private Coroutine flipAnimationCoroutine;
    private float flipDuration = 0.1f;
    private float animationProgress = 0f;
    private bool isAnimationReversed = false;
    private Coroutine flipCoroutine;
    
    public Canvas yourCanvas;

    // Get a reference to the LeanDragTranslate component
    private LeanDragTranslate leanDragTranslate;
    
    private void Awake()
    {
        // Get the LeanDragTranslate component
        leanDragTranslate = GetComponent<LeanDragTranslate>();
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
        //originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Invoke("SetDraggingFalse", 0.2f);
        // Just moving it back for now
        //transform.position = originalPosition;
    }

    void SetDraggingFalse()
    {
        dragging = false;
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
    
    public void EnableFlippable(bool shouldEnable)
    {
        flippable = shouldEnable;
    }
}
