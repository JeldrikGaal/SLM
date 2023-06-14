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
    private ClickableStorage _cS;
    #endregion

    // Colors
    private Color _disabled;
    private Color _highlight;

    private bool _main;

    void Start()
    {
        _disabled = new Color(1, 1, 1, 0);
        _highlight = new Color(1, 0.93f, 0.14f, 0.6f);
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cS = GameObject.FindGameObjectWithTag("ClickableStorage").GetComponent<ClickableStorage>();
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
        // Setting all the info in the popup
        _titleText.text = cH.Title;
        _descriptionText.text = cH.Description;
        _image.sprite = cH.Image;

        // Highlighting the clicked object
        //_currentClickable.SetColor(_highlight);

        // Showing the outline of the clicked object
        if (_currentClickable._outline != null)
        {
            _currentClickable._outline.ToggleOutline(true);
        }

        // Animate Camera so highlighted object is in the right spot
        Vector3 pos = _currentClickable.transform.position;
        Vector3 goal = Vector3.zero;
        float x = 0;

        Debug.Log((pos.x, _tH._width * 0.65f));
        // See if the object is too close to the right border of the screen and decide on which side to put it
        if (pos.x < _tH._width * 0.65f)
        {
            // Position on the left of the camera
             x = Mathf.Max(-_tH._camLimits.x, Mathf.Min(pos.x + (Camera.main.orthographicSize * _tH._aspect) * 0.6f, _tH._camLimits.x));
        }
        else
        {
            // Position on the right of the camera
            x = Mathf.Max(-_tH._camLimits.x, Mathf.Min(pos.x - (Camera.main.orthographicSize * _tH._aspect) * 0.6f, _tH._camLimits.x));
        }
        
        float y = Mathf.Max(- _tH._camLimits.y, Mathf.Min(pos.y, _tH._camLimits.y));
        goal = new Vector3(x, y, Camera.main.transform.position.z);
        
        // Actually move the camera
        Camera.main.transform.DOMove(goal, 0.5f).OnComplete(() =>
        {
            // Setting the popup in the middle of the screen
            _popUp.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _popUp.transform.position.z);

            // Animate Scale
            Vector3 safeScale = _popUp.transform.localScale;
            _popUp.transform.localScale = Vector3.zero;
            _popUp.SetActive(true);
            _popUp.transform.DOScale(safeScale, 0.35f).OnComplete(() =>
            {
                // Handle further logic of object having been clicked
                _qM.ObjectClick(cH);
                StoreClicked(cH);

            });
        });
        _tH.LockInput(true);
        
    }

    // Store information about a clickable having been clicked to later transfer to the book
    public void StoreClicked(ClickableHolder cH)
    {
        _cS.AddToStorage(cH);
    }

    // Hides the popup 
    public void HidePopUp()
    {
        Vector3 safeScale = _popUp.transform.localScale;
        _popUp.transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            _popUp.SetActive(false);
            _popUp.transform.localScale = safeScale;

            // Change color of clickable box
            //_currentClickable.SetColor(_disabled);

            // Hide outline
            if (_currentClickable._outline != null)
            {
                _currentClickable._outline.ToggleOutline(false);
            }

            _tH.UnlockInput();
        });
        
    }

    
}
