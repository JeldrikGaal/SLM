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
    [SerializeField] private Image _grayScaleImage;
    private Transform _grayScaleParentSafe;
    private Clickable _currentClickable;
    private TouchHandler _tH;
    private ClickableStorage _cS;
    private VALUECONTROLER _VC;

    private Transform _canvas;
    #endregion

    void Start()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cS = GameObject.FindGameObjectWithTag("ClickableStorage").GetComponent<ClickableStorage>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
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

    public GameObject GetCurrentClickable()
    {
        return _currentClickable.transform.gameObject;
    }

    // Retrieves the information from a give 'clickableholder' object and displays everything accordingly in the popup
    private void DisplayPopUp(ClickableHolder cH)
    {
        // Setting all the info in the popup
        _titleText.text = cH.Title;
        _descriptionText.text = cH.Description;
        _image.sprite = cH.Image;

        // Showing the outline of the clicked object
        if (_currentClickable._outline != null)
        {
            _currentClickable._outline.ToggleOutline(true);
        }

        // Toggling the grayscale fake effect 
        _grayScaleImage.enabled = true;
        _grayScaleParentSafe = _currentClickable.transform.parent.transform.parent;
        _currentClickable.transform.parent.transform.parent = _grayScaleImage.transform;

        // Animate Camera so highlighted object is in the right spot
        Vector3 pos = _currentClickable.transform.position;
        Vector3 goal = Vector3.zero;
        float x = 0;

        // See if the object is too close to the right border of the screen and decide on which side to put it
        if (pos.x < (_tH._width + _canvas.position.x) * 0.75f )
        {
            // Position on the left of the camera
             x = Mathf.Max(_tH._camLimits.x, Mathf.Min(pos.x + (Camera.main.orthographicSize * _tH._aspect) * _VC.Clickable_Pos * 0.01f, _tH._camLimits.y));
        }
        else
        {
            // Position on the right of the camera
            x = Mathf.Max(_tH._camLimits.x, Mathf.Min(pos.x - (Camera.main.orthographicSize * _tH._aspect) * _VC.Clickable_Pos * 0.01f, _tH._camLimits.y));
        }
        
        float y = Mathf.Max(- _tH._camLimits.y, Mathf.Min(pos.y, _tH._camLimits.y));
        goal = new Vector3(x, y, Camera.main.transform.position.z);
        
        // Actually move the camera
        Camera.main.transform.DOMove(goal, _VC.Camera_ClickMoveTime).OnComplete(() =>
        {
            // Setting the popup in the middle of the screen
            _popUp.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _popUp.transform.position.z);

            // Animate Scale
            Vector3 safeScale = _popUp.transform.localScale;
            _popUp.transform.localScale = Vector3.zero;
            _popUp.SetActive(true);
            _popUp.transform.DOScale(safeScale, _VC.PopUp_AnimSpeed).OnComplete(() =>
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
        _popUp.transform.DOScale(Vector3.zero, _VC.PopUp_AnimSpeed).OnComplete(() =>
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

            // Toggling the grayscale fake effect 
            _grayScaleImage.enabled = false;
            _currentClickable.transform.parent.transform.parent = _grayScaleParentSafe;

            _tH.UnlockInput();
        });
        
    }

    
}
