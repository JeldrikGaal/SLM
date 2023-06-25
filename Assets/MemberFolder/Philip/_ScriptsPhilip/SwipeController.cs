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

    void SwipeHandler(LeanFinger finger)
    {
        Vector2 swipeDirection = finger.SwipeScreenDelta.normalized;

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