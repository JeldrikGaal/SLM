using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    public GameObject mapUnfold;
    public Animator mapUnfoldAnim;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    
    [Header("Age Group Stuff")]
    public Button ageButton;
    private TextMeshProUGUI _ageButtonText;
    private string[] _ageGroups = { "Normal Mode", "Advanced Mode" };
    private int _currentIndex = 0;

    [Header("Language Stuff")] 
    public Button languageButton;
    private TextMeshProUGUI _languageButtonText;
    private string[] _languages = { "German (DE)", "English (EN)" };
    private string[] _localizationNames = {"German", "Simplified German", "English", "Simplified English"};

    [Header("Book Pages")]
    public Animator bookAnimator;
    public GameObject[] pages;
    [Tooltip("Distance that a swipe needs to cover to be registered")]
    public float swipeThreshold = 200f;
    public TrailRenderer trailRenderer;

    [Header("Resolutions")] 
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    public TextMeshProUGUI resolutionText;
    private int currentResolutionIndex = 0;
    
    private LangugageStorage _langugageStorage;

    private void Awake()
    {
        mapUnfold.SetActive(false);
        
        cam1.Priority = 10;
        cam2.Priority = 5;
        
        _currentPageIndex = GameManager.Instance.pageIndex;
        
        _ageButtonText = ageButton.GetComponentInChildren<TextMeshProUGUI>();
        _ageButtonText.text = _ageGroups[_currentIndex];

        _languageButtonText = languageButton.GetComponentInChildren<TextMeshProUGUI>();
        _languageButtonText.text = _languages[0];
        
        _langugageStorage = GameObject.FindGameObjectWithTag("LanguageStorage").GetComponent<LangugageStorage>();

        ResolutionSetup();
        ShowCurrentPage();
    }

    private void Start()
    {
        trailRenderer.Clear();
    }

    void Update()
    {
        _currentPageIndex = GameManager.Instance.pageIndex;
        DetectSwipeInput();
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
                    PreviousPage();
                }
                else if (swipeDistance < -swipeThreshold)
                {
                    NextPage();
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
            bookAnimator.Play("bookFlipLeft");
            
            _currentPageIndex++;
            GameManager.Instance.pageIndex = _currentPageIndex;
            ShowCurrentPage();
        }
    }

    public void PreviousPage()
    {
        if (_currentPageIndex > 0)
        {
            bookAnimator.Play("bookFlipRight");
            
            _currentPageIndex--;
            GameManager.Instance.pageIndex = _currentPageIndex;
            ShowCurrentPage();
        }
    }

    #endregion
    
    #region Language and Age
    
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
    
    #endregion
    
    #region Screen Resolution
    
    public void ResolutionSetup()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        
        resolutionDropdown.ClearOptions();

        //get all resolutions that match your refresh rate - avoids unnecessary resolution clutter
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        //goes through the filtered resolutions and adds them to the options list - also finds the current resolution
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        //adds all options to the dropdown menu
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //set resolution text top left
        resolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
    }

    //gets called from the dropdown value changed
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        resolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
    }
    
    #endregion
    
    
    public void LoadMinigame(string minigame)
    {
        Debug.Log("Loading Function");
        
        StartCoroutine(MapTransition(5f, minigame));

    }

    private IEnumerator MapTransition(float time, string minigame)
    {
        mapUnfold.SetActive(true);
        
        Debug.Log("Entered coroutine");
        
        cam2.Priority = 15;
        
        yield return new WaitForSeconds(time);
        
        Debug.Log("exit coroutine");
        
        SceneManager.LoadScene(minigame);

        cam2.Priority = 5;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
