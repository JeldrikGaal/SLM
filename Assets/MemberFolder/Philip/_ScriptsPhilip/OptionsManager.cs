using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private bool eng = true, ger = false, norm = true, simp = false;

    //scribbles
    public GameObject GermanSimple, EnglishSimple, GermanNormal, EnglishNormal, GermanSimple2, EnglishSimple2, GermanNormal2, EnglishNormal2;

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
            DisableScribbles();
            GermanNormal.SetActive(true);
            GermanNormal2.SetActive(true);
        } 
        else if (SimpButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified German";
            DisableScribbles();
            GermanSimple.SetActive(true);
            GermanSimple2.SetActive(true);
        }
    }

    public void English()
    {
        GerButton.GetComponent<OptionsButton>().SetInactive();
        EngButton.GetComponent<OptionsButton>().SetActive();

        if (NorButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "English";
            DisableScribbles();
            EnglishNormal.SetActive(true);
            EnglishNormal2.SetActive(true);
        } 
        else if (SimpButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified English";
            DisableScribbles();
            EnglishSimple.SetActive(true);
            EnglishSimple2.SetActive(true);
        }
    }

    public void Normal()
    {
        SimpButton.GetComponent<OptionsButton>().SetInactive();
        NorButton.GetComponent<OptionsButton>().SetActive();
        
        if (GerButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "German";
            DisableScribbles();
            GermanNormal.SetActive(true);
            GermanNormal2.SetActive(true);
        } 
        else if (EngButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "English";
            DisableScribbles();
            EnglishNormal.SetActive(true);
            EnglishNormal2.SetActive(true);
        }
    }

    public void Simple()
    {
        NorButton.GetComponent<OptionsButton>().SetInactive();
        SimpButton.GetComponent<OptionsButton>().SetActive();

        if (GerButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified German";
            DisableScribbles();
            GermanSimple.SetActive(true);
            GermanSimple2.SetActive(true);
        } 
        else if (EngButton.GetComponent<OptionsButton>().active)
        {
            LocalizationManager.Language = "Simplified English";
            DisableScribbles();
            EnglishSimple.SetActive(true);
            EnglishSimple2.SetActive(true);
        }
    }

    private void DisableScribbles()
    {
        GermanNormal.SetActive(false);
        GermanSimple.SetActive(false);
        EnglishNormal.SetActive(false);
        EnglishSimple.SetActive(false);
        GermanNormal2.SetActive(false);
        GermanSimple2.SetActive(false);
        EnglishNormal2.SetActive(false);
        EnglishSimple2.SetActive(false);
    }
}
