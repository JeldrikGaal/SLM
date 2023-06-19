using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLanguage : MonoBehaviour
{

    private void Awake()
    {
        LocalizationManager.Read();

        //LocalizationManager.Language = "German";
    }

}
