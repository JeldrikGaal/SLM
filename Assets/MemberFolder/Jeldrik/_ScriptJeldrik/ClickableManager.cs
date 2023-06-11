using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickableManager : MonoBehaviour
{

    #region References
    [SerializeField] private GameObject _popUp;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private QuestionManager _qM;
    private Clickable _currentClickable;
    private TouchHandler _tH;   
    #endregion

    // Colors
    private Color _disabled;
    private Color _highlight;


    void Start()
    {
        _disabled = new Color(1, 1, 1, 0);
        _highlight = new Color(1, 0.93f, 0.14f, 0.6f);
        _tH = Camera.main.GetComponent<TouchHandler>();
    }

    void Update()
    {
        
    }

    // Try to display a new Popup. If one is already being displayed return false otherwise displays new one and returns true
    public bool TryDisplayPopUp(ClickableHolder cH, Clickable C)
    {
        if (Showing())
        {
            return false;
        }
        else
        {
            _currentClickable = C;
            DisplayPopUp(cH);
            return true;
        }
    }

    // return if the popup is currently being shown
    public bool Showing()
    {
        return _popUp.activeInHierarchy;
    }

    // Retrieves the information from a give 'clickableholder' object and displays everything accordingly in the popup
    private void DisplayPopUp(ClickableHolder cH)
    {
        _popUp.SetActive(true);
        // Setting the popup in the middle of the screen
        _popUp.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _popUp.transform.position.z);

        // Setting all the info in the popup
        _titleText.text = cH.Title;
        _descriptionText.text = cH.Description;
        _image.sprite = cH.Image;

        // Highlighting the clicked object
        _currentClickable.SetColor(_highlight);

        // Animate Scale
        Vector3 safeScale = _popUp.transform.localScale;
        _popUp.transform.localScale = Vector3.zero;
        _popUp.transform.DOScale(safeScale, 0.35f).OnComplete(() =>
        {
            _qM.ObjectClick(cH);
        });

        

        _tH.LockInput();
        
    }

    // Hides the popup 
    public void HidePopUp()
    {
        Vector3 safeScale = _popUp.transform.localScale;
        _popUp.transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            _popUp.SetActive(false);
            _popUp.transform.localScale = safeScale;
            _currentClickable.SetColor(_disabled);
            _tH.UnlockInput();
        });
        
    }

    
}
