using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;

public class LanguageSwitch : MonoBehaviour
{
    QuestionManager _qM;
    // Start is called before the first frame update
    void Awake()
    {
        _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
    }

    // Toggling between English and German, just used for testing / debugging
    public void ToggleLanguage()
    {
        if (LocalizationManager.Language == "English")
        {
            LocalizationManager.Language = "German";
        }
        else 
        {
            LocalizationManager.Language = "English";
        }
        _qM.UpdateLocalizedData();
    }
}
