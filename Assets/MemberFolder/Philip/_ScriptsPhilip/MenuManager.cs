using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;

public class MenuManager : MonoBehaviour
{
    [Header("Age Group Stuff")]
    public Button ageButton;
    private TextMeshProUGUI _ageButtonText;
    private string[] _ageGroups = { "Stöpsel (9+)", "Old Fart (50+)" };
    private int _currentIndex = 0;

    [Header("Language Stuff")] 
    public Button languageButton;
    private TextMeshProUGUI _languageButtonText;
    private string[] _languages = { "German (DE)", "English (EN)" };
    private string[] _localizationNames = {"German", "Simplified German", "English", "Simplified English"};

    [Header("Minigame 1")]
    public String sceneToLoad;

    [Header("Book Pages")]
    public Animator bookAnimator;
    public GameObject[] pages;
    [Tooltip("Distance that a swipe needs to cover to be registered")]
    public float swipeThreshold = 200f;
    
    private LangugageStorage _langugageStorage;

    private void Awake()
    {
        _currentPageIndex = GameManager.Instance.pageIndex;
        
        _ageButtonText = ageButton.GetComponentInChildren<TextMeshProUGUI>();
        _ageButtonText.text = _ageGroups[_currentIndex];

        _languageButtonText = languageButton.GetComponentInChildren<TextMeshProUGUI>();
        _languageButtonText.text = _languages[0];
        
        _langugageStorage = GameObject.FindGameObjectWithTag("LanguageStorage").GetComponent<LangugageStorage>();

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
        SetLocalization();
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
        SetLocalization();
    }

    public void SetLocalization()
    {
        if (_languageButtonText.text == _languages[0])
        {
            if (_currentIndex == 1)
            {
                LocalizationManager.Language = _localizationNames[0];
            }
            else
            {
                LocalizationManager.Language = _localizationNames[1];
            }
        }
        else
        {
            if (_currentIndex == 1)
            {
                LocalizationManager.Language = _localizationNames[2];
            }
            else
            {
                LocalizationManager.Language = _localizationNames[3];
            }
        }
        //Debug.Log(LocalizationManager.Language);
        _langugageStorage.Language = LocalizationManager.Language;
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
