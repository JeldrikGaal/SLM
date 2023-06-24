using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public MenuManager menuManager;
    
    private Vector2 _swipeStartPosition;
    private bool _isSwiping = false;
    
    [Tooltip("Distance that a swipe needs to cover to be registered")]
    public float swipeThreshold = 200f;
    public TrailRenderer trailRenderer;


    private void Start()
    {
        trailRenderer.Clear();
    }

    void Update()
    {
        DetectSwipeInput();
    }
    
    private void DetectSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPosition = Input.mousePosition;
            _isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isSwiping)
            {
                Vector2 swipeEndPosition = Input.mousePosition;
                float swipeDistance = swipeEndPosition.x - _swipeStartPosition.x;

                if (swipeDistance > swipeThreshold)
                {
                    menuManager.PreviousPage();
                    Debug.Log("Previous Page");
                }
                else if (swipeDistance < -swipeThreshold)
                {
                    menuManager.NextPage();
                    Debug.Log("Next Page");
                }
            }
            _isSwiping = false;
        }

        if (_isSwiping)
        {
            trailRenderer.enabled = true;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 1f;
            trailRenderer.transform.position = mousePos;
        }
        else
        {
            trailRenderer.enabled = false;
            trailRenderer.Clear();
        }
    }
}
