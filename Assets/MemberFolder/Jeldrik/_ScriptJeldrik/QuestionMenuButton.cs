using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _questionMenuButton;
    [SerializeField] private GameObject _questionMark;
    [SerializeField] private Image _image;
    private TouchHandler _tH;

    // Start is called before the first frame update
    void Awake()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x + _tH._width * 0.05f, Camera.main.transform.position.y + _tH._height * 0.045f, transform.position.z);
    }

    public void ToggleSelf(bool active)
    {
        _image.enabled = active;
        _questionMark.SetActive(active);
    }

    public void ToggleMenu()
    {
        _questionMenuButton.SetActive(!_questionMenuButton.activeInHierarchy);
    }
}
