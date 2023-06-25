using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    public string _localizationKey;

    public void Start()
    {
        Localize();
        LocalizationManager.LocalizationChanged += Localize;
    }

    public void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= Localize;
    }

    private void Localize()
    {
        GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(_localizationKey);
    }
}
