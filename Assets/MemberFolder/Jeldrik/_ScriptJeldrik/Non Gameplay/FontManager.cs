using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontManager : MonoBehaviour
{

    [SerializeField] private TMP_FontAsset _font;

    public TMP_FontAsset GetFont()
    {
        return _font;
    }

    void Start()
    {
        // get all tmp text objects
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>(true);
        
        // set all tmp text objects to the correct font
        foreach (TMP_Text text in texts)
        {
            text.font = _font;
        }
    }

    void Update()
    {
        
    }
}
