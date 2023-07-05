using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEditor;

public class IdleScript : MonoBehaviour
{
    public MenuManager menu;
    public float afkThreshold = 30f;
    public float returnThreshold = 30f;
    private float timer = 0f;
    private float afkTimer = 0f;
    private bool isAFK = false;
    private bool popup = false;

    public GameObject afkWindow;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerUpdate;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerUpdate;
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        // Reset the timer when any finger is updated (touch or drag)
        ResetTimer();
        isAFK = false;

        if (popup)
        {
            HidePopup();
            StartReturnTimer();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.pageIndex == 0) return;
        
        if (!isAFK)
        {
            timer += Time.deltaTime;
            
            //Debug.Log("not afk");
            if (timer >= afkThreshold)
            {
                ShowPopup();
                isAFK = true;
                StartReturnTimer();
            }
        }
        else if (popup)
        {
            //Debug.Log("went afk");
            afkTimer += Time.deltaTime;
            
            if (afkTimer >= returnThreshold)
            {
                ReturnToMainPage();
            }
        }
    }
    
    private void ShowPopup()
    {
        if (!popup)
        {
            afkWindow.SetActive(true);
            menu.ShowCurrentPage();
            ResetTimer();
            popup = true;
        }
    }

    private void HidePopup()
    {
        if (popup)
        {
            afkWindow.SetActive(false);
            popup = false;
        }
    }
    
    private void StartReturnTimer()
    {
        afkTimer = 0f;
    }
    
    private void ReturnToMainPage()
    {
        if (isAFK)
        {
            afkWindow.SetActive(false);
            isAFK = false;
            if (GameManager.Instance.pageIndex > 0) { menu.PreviousPage(); }
            GameManager.Instance.pageIndex = 0;
            menu.ShowCurrentPage();
            ResetTimer();
        }
    }


    private void ResetTimer()
    {
        timer = 0f;
    }

}
