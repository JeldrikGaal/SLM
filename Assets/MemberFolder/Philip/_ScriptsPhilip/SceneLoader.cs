using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.SimpleLocalization;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    private AsyncOperation asyncLoad;

    private void Start()
    {
        LoadScene();
    }

    public void Awake()
    {
        //Debug.Log("SceneLoader Awake");
        //localizationManagerRef.GetComponent<LocalizationSync>().Sync();
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private System.Collections.IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ActivateScene()
    {
        asyncLoad.allowSceneActivation = true;
    }

    public void SetPageIndex(int page)
    {
        GameManager.Instance.pageIndex = page;
        
    }

    public void CompleteMG2()
    {
        GameManager.Instance.minigame2Complete = true;
    }
    
    public void CompleteMG1()
    {
        GameManager.Instance.minigame1Complete = true;
    }
}
