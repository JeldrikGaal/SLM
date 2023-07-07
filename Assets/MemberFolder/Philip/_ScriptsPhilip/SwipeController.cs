using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class SwipeController : MonoBehaviour
{
    public MenuManager menuManager;
    private bool _isSwiping;
    public TrailRenderer tr;

    public float minSwipeDistance = 50f;

    private Vector2 swipeStartPos;
    
    void OnEnable()
    {
        LeanTouch.OnFingerSwipe += SwipeHandler;
        LeanTouch.OnFingerUpdate += HoldHandler;
        LeanTouch.OnFingerUp += ReleaseHandler;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= SwipeHandler;
        LeanTouch.OnFingerUpdate -= HoldHandler;
        LeanTouch.OnFingerUp -= ReleaseHandler;
    }

    private void Update()
    {
        throw new NotImplementedException();
    }
    
    void SwipeHandler(LeanFinger finger)
    {
        Vector2 swipeDirection = finger.SwipeScreenDelta.normalized;
        float swipeDistance = finger.SwipeScreenDelta.magnitude;

        if (Mathf.Abs(swipeDistance) > minSwipeDistance)
        {
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                if (swipeDirection.x > 0)
                {
                    menuManager.PreviousPage();
                }
                else
                {
                    menuManager.NextPage();
                }
            }
        }
    }
    
    void HoldHandler(LeanFinger finger)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 1f;
        tr.transform.position = worldPosition;
        tr.enabled = true;

        //disable tr when in editor mode since it thinks we are pressing the screen continuously
        #if (UNITY_EDITOR)
            //tr.enabled = false;
        #endif
    }
    
    void ReleaseHandler(LeanFinger finger)
    {
        tr.enabled = false;
    }
}