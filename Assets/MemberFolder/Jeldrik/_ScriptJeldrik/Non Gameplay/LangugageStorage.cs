using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class LangugageStorage : MonoBehaviour
{
    private object _current;
    public string Language = "German";

    // DontDestroyOnLoad object that stores the language set in the main menu 
    // UPDATE: may be not needed and just used wrongly to fix an unknown problem
    void Start()
    {
        if (_current == null)
        {
            _current = this.gameObject;
             DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LocalizationManager.Language = Language;
    }
}
