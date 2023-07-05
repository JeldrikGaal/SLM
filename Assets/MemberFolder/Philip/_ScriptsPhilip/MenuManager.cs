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
    private ClickableStorage _storage;
    public InfoPageManager _infoManager;
    private LangugageStorage _langugageStorage;
    
    
    public float loadMinigameDelay = 5f;
    public GameObject mapUnfold;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;

    
    private void Awake()
    {
        _storage = ClickableStorage.Instance;

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
        //AddInfoPages();
    }
    
    void Update()
    {
        _currentPageIndex = GameManager.Instance.pageIndex;
    }

    #region BookPages
    
    [Header("Book Pages")] 
    public GameObject book;
    public Animator bookAnimator;
    public List<GameObject> pages;
    public GameObject rightCorner, leftCorner;
    private int _currentPageIndex = 0;

    public void ShowCurrentPage()
    {
        _currentPageIndex = GameManager.Instance.pageIndex;
        //cycles through all pages and enables only the currentPage
        
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == _currentPageIndex);
        }
        
        DisplayCorners();
    }
    
    public void AddInfoPages()
    {
        foreach(GameObject page in _infoManager.infoPagePrefabs)
        {
            var newPage = Instantiate(page, book.transform);
            pages.Add(newPage);
        }
    }
    
    public void AddPage(GameObject page)
    {
        _infoManager.infoPagePrefabs.Add(page);
        
        var newPage = (GameObject)Instantiate(page, book.transform);
        pages.Add(newPage);
        newPage.name = "InfoPage";
        newPage.SetActive(false);
        
        ShowCurrentPage();
    }

    public void DisplayCorners()
    {
        leftCorner.SetActive(false);
        rightCorner.SetActive(false);
 
        //enable/disable the eselsohren depending on what page you're on
        if (_currentPageIndex == 0)
        {
            leftCorner.SetActive(false);
            rightCorner.SetActive(true);
        } else if (_currentPageIndex > 0 && _currentPageIndex < pages.Count - 1)
        {
            leftCorner.SetActive(true);
            rightCorner.SetActive(true);
        } else
        {
            leftCorner.SetActive(true);
            rightCorner.SetActive(false);
        }
    }
    
    public void NextPage()
    {
        if (_currentPageIndex < pages.Count - 1)
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
    
    [Header("Options")]
    public Button ageButton;
    private TextMeshProUGUI _ageButtonText;
    private string[] _ageGroups = { "Normal", "Simplified" };
    private int _currentIndex = 0;
    
    public Button languageButton;
    private TextMeshProUGUI _languageButtonText;
    private string[] _languages = { "English (EN)", "German (DE)" };
    private string[] _localizationNames = {"English", "Simplified English", "German", "Simplified German"};

    
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
            if (_currentIndex == 0)
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
            if (_currentIndex == 0)
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
    
    [Header("Resolutions")] 
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    public TextMeshProUGUI resolutionText;
    private int currentResolutionIndex = 0;
    
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
        StartCoroutine(MapTransition(loadMinigameDelay, minigame));
    }

    private IEnumerator MapTransition(float time, string minigame)
    {
        mapUnfold.SetActive(true);
        cam2.Priority = 15;
        
        yield return new WaitForSeconds(time);
        
        SceneManager.LoadScene(minigame);
        cam2.Priority = 5;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
