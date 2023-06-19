using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class LangugageStorage : MonoBehaviour
{
    private object _current;
    public string Language = "German";

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
