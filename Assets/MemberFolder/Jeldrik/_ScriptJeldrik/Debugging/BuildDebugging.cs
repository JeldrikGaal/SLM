using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildDebugging : MonoBehaviour
{
    [SerializeField] private TMP_Text _debuggingText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Displaying debugging info used in testing the mobile build 
    public void ShowDebugInfo()
    {
        float _width = (float)Screen.width / 2.0f;
        float _height = (float)Screen.height / 2.0f;
        float _aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        Vector2 _camLimits = new Vector2(_width - Camera.main.orthographicSize  * _aspect, _height - Camera.main.orthographicSize);
        _debuggingText.text = (_width.ToString() + '\n' +  _height.ToString()  + '\n' + Camera.main.orthographicSize  + '\n' + _aspect  + '\n' + _camLimits.ToString());
    }
}
