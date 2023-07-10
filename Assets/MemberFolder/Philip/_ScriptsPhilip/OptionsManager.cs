using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private bool eng = true, ger = false, norm = true, simp = false;

    public GameObject EngButton, GerButton, NorButton, SimpButton;

    private void Start()
    {
        //LocalizationManager.Language = "German";
    }

    public void German()
    {
        GerButton.GetComponent<OptionsButton>().SetActive();
        EngButton.GetComponent<OptionsButton>().SetInactive();

        if (NorButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "German";
        } 
        else if (SimpButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified German";
        }
    }

    public void English()
    {
        GerButton.GetComponent<OptionsButton>().SetInactive();
        EngButton.GetComponent<OptionsButton>().SetActive();

        if (NorButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "English";
        } 
        else if (SimpButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified English";
        }
    }

    public void Normal()
    {
        SimpButton.GetComponent<OptionsButton>().SetInactive();
        NorButton.GetComponent<OptionsButton>().SetActive();
        
        if (GerButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "German";
        } 
        else if (EngButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "English";
        }
    }

    public void Simple()
    {
        NorButton.GetComponent<OptionsButton>().SetInactive();
        SimpButton.GetComponent<OptionsButton>().SetActive();

        if (GerButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified German";
        } 
        else if (EngButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified English";
        }
    }
}
