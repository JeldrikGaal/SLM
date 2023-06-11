using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestionMenuButton : MonoBehaviour
{
    #region References
    [SerializeField] private GameObject _questionMenuButton;
    [SerializeField] private GameObject _questionMark;
    [SerializeField] private Image _image;
    private TouchHandler _tH;
    #endregion

    void Awake()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
    }

    void Update()
    {
        // Setting button to be in the upper right corner
        transform.position = new Vector3(Camera.main.transform.position.x + _tH._width * 0.05f, Camera.main.transform.position.y + _tH._height * 0.045f, transform.position.z);
    }

    // Toggles the button object it self
    public void ToggleSelf(bool active)
    {
        _image.enabled = active;
        _questionMark.SetActive(active);
        
    }

    // Toggles the question menu and adds a small animation
    public void ToggleMenu()
    {
        
        if (_questionMenuButton.activeInHierarchy)
        {
            _questionMenuButton.transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
            {
                _tH.UnlockInput();
                _questionMenuButton.SetActive(false);
                
            });
        }
        else
        {
            _questionMenuButton.transform.localScale = Vector3.zero;
            _questionMenuButton.SetActive(true);
            _questionMenuButton.transform.DOScale(Vector3.one, 0.35f).OnComplete(() => _tH.LockInput());
            
        }

        // Prevents wrong input particle from spawning
        _tH.BlockWrongInputPart();
        
    }
}
