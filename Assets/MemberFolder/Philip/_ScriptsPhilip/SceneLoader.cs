using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    private AsyncOperation asyncLoad;

    private void Start()
    {
        LoadScene();
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
}
