using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Animating,
        Paused
    }
    
    public GameState CurrentState { get; set; }
    
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

    public void LoadBook()
    {
        SceneManager.LoadScene("Book_Main");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Book_Main");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("MG1");
        }
    }
}
