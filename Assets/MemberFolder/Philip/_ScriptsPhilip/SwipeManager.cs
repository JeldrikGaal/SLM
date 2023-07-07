using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public MenuManager menuManager;
    public float minSwipeDistance = 50f;

    private Vector2 swipeStartPosition;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                DetectSwipe(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            swipeStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DetectSwipe(Input.mousePosition);
        }
    }

    private void DetectSwipe(Vector2 swipeEndPosition)
    {
        Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            float swipeAngle = Mathf.Atan2(swipeDelta.y, swipeDelta.x) * Mathf.Rad2Deg;

            if (swipeAngle < 0)
            {
                swipeAngle += 360f;
            }

            if (swipeAngle > 45f && swipeAngle < 135f)
            {
                // Upward swipe
            }
            else if (swipeAngle > 225f && swipeAngle < 315f)
            {
                // Downward swipe
            }
            else if (swipeAngle >= 135f && swipeAngle <= 225f)
            {
                // Left swipe
                menuManager.NextPage();
            }
            else
            {
                // Right swipe
                menuManager.PreviousPage();
            }
        }
    }
}
