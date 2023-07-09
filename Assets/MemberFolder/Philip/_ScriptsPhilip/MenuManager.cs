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
    public GameObject initialPage;

    public SceneLoader sceneLoader;

    public float mapDelay = 2f;
    
    public float loadMinigameDelay = 5f;
    public Animator mapUnfoldAnim;
    public AnimationClip mapUnfold;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera MG2cam;

    
    private void Awake()
    {
        GameManager.Instance.CurrentState = GameManager.GameState.Idle;
        
        _storage = ClickableStorage.Instance;
        
        cam1.Priority = 10;
        cam2.Priority = 5;
        
        _currentPageIndex = GameManager.Instance.pageIndex;

        ResolutionSetup();
        ShowCurrentPage();
    }

    private void Start()
    {
        StartCoroutine("InitialPage");
    }

    private IEnumerator InitialPage()
    {
        yield return new WaitForSeconds(.5f);
        
        initialPage.SetActive(false);
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
        if (GameManager.Instance.CurrentState == GameManager.GameState.Animating) return;
        
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
        if (GameManager.Instance.CurrentState == GameManager.GameState.Animating) return;
        
        if (_currentPageIndex > 0)
        {
            bookAnimator.Play("bookFlipRight");
            
            _currentPageIndex--;
            GameManager.Instance.pageIndex = _currentPageIndex;
            ShowCurrentPage();
        }
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

    public void LoadMinigame1()
    {
        //if (GameManager.Instance.CurrentState == GameManager.GameState.Animating) return;
         
        StartCoroutine(MapTransition(loadMinigameDelay));
    }

    public void LoadMinigame2()
    {
        StartCoroutine(MG2Transition());
    }

    private IEnumerator MG2Transition()
    {
        MG2cam.Priority = 20;
        
        yield return new WaitForSeconds(4f);


        SceneManager.LoadScene("MG2");
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MG2");

        //Wait until the scene finishes loading
        //while (!asyncLoad.isDone)
        {
        //    yield return null;
        }
    }

    private IEnumerator MapTransition(float time)
    {
        GameManager.Instance.CurrentState = GameManager.GameState.Animating;

        mapUnfoldAnim.Play("mapunfoldAnim");
        yield return new WaitForSeconds(mapUnfold.length - 6.7f);

        cam2.Priority = 15;

        yield return new WaitForSeconds(mapDelay);

        //SceneManager.LoadScene("MG1");
        sceneLoader.ActivateScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        //Clickable Storage reset
        GameObject storage = GameObject.FindWithTag("ClickableStorage");
        Destroy(storage);
        
        GameManager.Instance.Reset();

        for (int i = pages.Count - 1; i >= 3; i--)
        {
            pages.RemoveAt(i);
        }
        
        ShowCurrentPage();
        
    }
    
}
