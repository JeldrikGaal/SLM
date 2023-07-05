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

    [SerializeField] private QuestionManager _qM;
    [SerializeField] private Image _grayScaleImage;
    [SerializeField] private List<PopUp> _popUps = new List<PopUp>();
    [SerializeField] private SlideColorStripe _colorStripe;
    private Transform _grayScaleParentSafe;
    private int _grayScaleSiblingIndexSafe;
    private Clickable _currentClickable;
    private TouchHandler _tH;
    private ClickableStorage _cS;
    private VALUECONTROLER _VC;
    [SerializeField] public MG1Tutorial _tutorialManager;
    private Transform _canvas;
    #endregion

    private bool _animating;

    void Start()
    {
        // Getting References
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cS = GameObject.FindGameObjectWithTag("ClickableStorage").GetComponent<ClickableStorage>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<MG1Tutorial>();
        if (_tutorialManager.SKIPTUTORIAL || _tutorialManager.Done)
        {
            _colorStripe.Appear();
        }
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
            if (cH.Question)
            {
                if (!_tutorialManager.SKIPTUTORIAL && !_tutorialManager.Done && cH != _tutorialManager._exampleClickable.cH)
                {
                    Debug.Log("You shall not click this !");
                    return false;
                }
                DisplayPopUp(cH,_popUps[0]);
            }
            else 
            {
                int ran = Random.Range(1, _popUps.Count);
                DisplayPopUp(cH,_popUps[ran]);
            }
            
            return true;
        }
    }

    // return if the popup is currently being shown
    public bool Showing()
    {
        return _popUp.activeInHierarchy;
    }

    // Returns the gameobject of the currently selected clickable
    public GameObject GetCurrentClickable()
    {
        return _currentClickable.transform.gameObject;
    }

    // Retrieves the information from a give 'clickableholder' object and displays everything accordingly in the popup
    private void DisplayPopUp(ClickableHolder cH, PopUp _popUpScript)
    {
        // If the popup is currently being animated in some shape or form it should not take any other input
        if (_animating) return;

        _tH.LockInput(true);

        _popUpScript.UpdateText(cH);

        // Showing the outline of the clicked object
        if (_currentClickable._outline != null)
        {
            _currentClickable._outline.ToggleOutline(true);
        }

        _animating = true;

        // Toggling the grayscale fake effect 
        _grayScaleImage.enabled = true;
        _grayScaleImage.color = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0);
        Color fade = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0.8f);
        _grayScaleImage.DOColor(fade, _VC.PopUp_AnimSpeed);
        _grayScaleParentSafe = _currentClickable.transform.parent.transform.parent;
        _grayScaleSiblingIndexSafe = _currentClickable.transform.parent.transform.GetSiblingIndex();
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
            //_popUpScript.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _popUpScript.transform.position.z);

            // Animate Scale
            Vector3 safeScale = _popUpScript.transform.localScale;
            _popUpScript.transform.localScale = Vector3.zero;
            _popUpScript.gameObject.SetActive(true);
            _qM.MakeSwirl(cH);

            // Tell the tutorial manager an object has been clicked
            if(!_tutorialManager.SKIPTUTORIAL && !_tutorialManager.Done) _tutorialManager.PopUpOpening();

            _popUpScript.transform.DOScale(safeScale, _VC.PopUp_AnimSpeed).OnComplete(() =>
            {
                // Handle further logic of object having been clicked
                _qM.ObjectClick(cH);
                StoreClicked(cH);
                _animating = false;

            });
        });
        
        
    }

    // Store information about a clickable having been clicked to later transfer to the book
    public void StoreClicked(ClickableHolder cH)
    {
        _cS.AddToStorage(cH);
    }

    // Hides the popup 
    public void HidePopUp(PopUp _popUpScript)
    {
        // If the popup is currently being animated in some shape or form it should not take any other input
        if (_animating) return;
                  
        Vector3 safeScale = _popUpScript.transform.localScale;
        Color fade = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0);
        _grayScaleImage.DOColor(fade, _VC.PopUp_AnimSpeed);
        _animating = true;
        _popUpScript.transform.DOScale(Vector3.zero, _VC.PopUp_AnimSpeed).OnComplete(() =>
        {
            _popUpScript.gameObject.SetActive(false);
            _popUpScript.transform.localScale = safeScale;
            _animating = false;

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
            _currentClickable.transform.parent.transform.SetSiblingIndex(_grayScaleSiblingIndexSafe);

            _tutorialManager.PopUpClosing();
            _qM.ClosePopUp();
            _tH.UnlockInput();
        });
        
    }

    
}
