using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEditor;

public class IdleScript : MonoBehaviour
{
    public MenuManager menu;
    public float afkThreshold = 30f;
    private float timer = 0f;
    private bool isAFK = false;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        // Reset the timer when any finger is updated (touch or drag)
        //ResetTimer();
    }

    private void Update()
    {
        Debug.Log(isAFK);
        
        timer += Time.deltaTime;
        
        if (timer >= afkThreshold && !isAFK)
        {
            //User is afk

            
            GameManager.Instance.pageIndex = 0;
            menu.ShowCurrentPage();

            isAFK = true;
            Debug.Log("User is now AFK");
        }
    }

    private void ResetTimer()
    {
        if (isAFK)
        {
            // Perform necessary actions when the user returns from being AFK

            isAFK = false;
            Debug.Log("User is no longer AFK");
        }

        timer = 0f;
    }

}
