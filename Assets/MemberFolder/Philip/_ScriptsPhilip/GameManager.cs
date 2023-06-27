using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int pageIndex = 0;

    public bool minigame1Complete = false;
    public bool minigame2Complete = false;

    [HideInInspector] public bool toggleMG1 = false, toggleMG2 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Book");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("TestPhilip");
        }
    }
}
