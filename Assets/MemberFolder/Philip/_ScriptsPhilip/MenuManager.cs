using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Age Group Stuff")]
    public Button ageButton;
    private TextMeshProUGUI _ageButtonText;
    private string[] _ageGroups = { "Stöpsel (9+)", "Chad (20-24)", "Old Fart (50+)" };
    private int _currentIndex = 0;

    [Header("Language Stuff")] 
    public Button languageButton;
    private TextMeshProUGUI _languageButtonText;
    private string[] _languages = { "German (DE)", "English (EN)" };

    [Header("Minigame 1")]
    public String sceneToLoad;

    [Header("Book Pages")]
    public Animator bookAnimator;
    public GameObject[] pages;
    [Tooltip("Distance that a swipe needs to cover to be registered")]
    public float swipeThreshold = 200f;
    

    private void Awake()
    {
        _currentPageIndex = GameManager.Instance.pageIndex;
        
        _ageButtonText = ageButton.GetComponentInChildren<TextMeshProUGUI>();
        _ageButtonText.text = _ageGroups[_currentIndex];

        _languageButtonText = languageButton.GetComponentInChildren<TextMeshProUGUI>();
        _languageButtonText.text = _languages[0];
        
        ShowCurrentPage();
    }
    
    void Update()
    {
        DetectSwipeInput();
        //GameManager.Instance.pageIndex = _currentPageIndex;
    }

    #region BookPages

    private int _currentPageIndex = 0;
    private Vector2 _swipeStartPosition;
    private bool _isSwiping = false;

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
                    if (GameManager.Instance.pageIndex != 1)
                    {
                        PreviousPage();
                    }
                }
                else if (swipeDistance < -swipeThreshold)
                {
                    NextPage();
                }
            }

            _isSwiping = false;
        }
    }
    
    //cycles through all pages and enables only the currentPage
    private void ShowCurrentPage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == _currentPageIndex);
        }
    }
    
    public void NextPage()
    {
        if (_currentPageIndex < pages.Length - 1)
        {
            _currentPageIndex++;
            GameManager.Instance.pageIndex = _currentPageIndex;
            ShowCurrentPage();
            
            bookAnimator.Play("bookFlipLeft");
        }
    }

    public void PreviousPage()
    {
        if (_currentPageIndex > 0)
        {
            _currentPageIndex--;
            GameManager.Instance.pageIndex = _currentPageIndex;
            ShowCurrentPage();

            bookAnimator.Play("bookFlipRight");
        }
    }

    #endregion
    
    
    public void ToggleAgeGroup()
    {
        _currentIndex = (_currentIndex + 1) % _ageGroups.Length;
        _ageButtonText.text = _ageGroups[_currentIndex];
    }

    public void ToggleLanguage()
    {
        if (_languageButtonText.text == _languages[0])
        {
            _languageButtonText.text = _languages[1];
        }
        else
        {
            _languageButtonText.text = _languages[0];
        }
    }
    
    
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
