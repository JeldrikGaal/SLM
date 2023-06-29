using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeDebugger : MonoBehaviour
{
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("MG1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("MG2");
        }
    }

    public void Book()
    {
        SceneManager.LoadScene("Book_Main");
    }
}
