using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.SimpleLocalization;
using Lean.Touch;

public class ClickableManager : MonoBehaviour
{

    #region References
    [SerializeField] private GameObject _popUp;

    [SerializeField] public QuestionManager _qM;
    [SerializeField] public Image _grayScaleImage;
    [SerializeField] public List<PopUp> _popUps = new List<PopUp>();
    [SerializeField] public List<PopUp> _halfPagePopUps = new List<PopUp>();
    
    [SerializeField] private SlideColorStripe _colorStripe;
    private Transform _grayScaleParentSafe;
    private int _grayScaleSiblingIndexSafe;
    private Clickable _currentClickable;
    private TouchHandler _tH;
    private ClickableStorage _cS;
    private VALUECONTROLER _VC;
    [SerializeField] public MG1Tutorial _tutorialManager;
    [SerializeField] private List<ClickableHolder> _declinedHolders = new List<ClickableHolder>();
    private Transform _canvas;
    
    #endregion

    public bool _tutorialBlock;
    private bool _animating;
    private LeanDragCamera _dragController;

    [SerializeField] private float _clickableCooldown;
    private float _lastHiding;

    void Start()
    {
        // Getting References
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cS = GameObject.FindGameObjectWithTag("ClickableStorage").GetComponent<ClickableStorage>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<MG1Tutorial>();
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
        foreach(ClickableHolder cH in _declinedHolders)
        {
            cH.Title = "";
            cH.Description = LocalizationManager.Localize(cH.LocalizationKey);
        }
        if (_tutorialManager.SKIPTUTORIAL || _tutorialManager.Done)
        {
            _colorStripe.Appear();
        }
        _lastHiding = Time.time;
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
            // Skip if blocked or tutorial running
            if (( !_tutorialManager.SKIPTUTORIAL && !_tutorialManager.Done && ( cH != _tutorialManager._exampleClickable.cH) ) || _tutorialBlock)
            {
                return false;
            }
            // Block if the player is currently dragging
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended) &&  Mathf.Abs(_dragController._currentDragMoveDistLong) > 5f) return false;
            }
            
            /*if (_dragController._hasBeenDragged)
            {
                //return false;
            }*/
            _currentClickable = C;

            // Johan can always be clicked ( Alpha ! )
            if (cH.LocalizationKey == "G1")
            {
                DisplayPopUp(cH,_popUps[0]);
                return true;
            }

            // First Question Decline
            if (_qM.GetCurrentQuestionId() == 0 && ( _VC.Questions[0].ObjectsToFind1[0] != cH ) )
            {
                if (C == _tutorialManager._exampleClickable || cH.LocalizationKey.Contains('S') )
                {
                    int ran2 = Random.Range(0, _halfPagePopUps.Count);
                    DisplayPopUp(cH, _halfPagePopUps[ran2]);
                    return true;
                }
                int ran = Random.Range(2, _popUps.Count);
                DisplayPopUp(_declinedHolders[0], _popUps[ran]);
                return true;
            }

            // Second Question
            else if (_qM.GetCurrentQuestionId() == 1 && ( !_VC.Questions[1].ObjectsToFind1.Contains(cH) && !cH.LocalizationKey.Contains('S') ) )
            {
                int ran = Random.Range(2, _popUps.Count);
                DisplayPopUp(_declinedHolders[1], _popUps[ran]);
                return true;
            }

            // Third Question
            else if (_qM.GetCurrentQuestionId() == 2 && ( !_VC.Questions[2].ObjectsToFind1.Contains(cH) && !cH.LocalizationKey.Contains('S') ))
            {
                int ran = Random.Range(2, _popUps.Count);
                DisplayPopUp(_declinedHolders[2], _popUps[ran]);
                return true;
            }

            // All big ones
            if (cH.Question)
            {
                DisplayPopUp(cH,_popUps[0]);
                return true;
            }
            // All small ones
            else 
            {
                int ran = Random.Range(0, _halfPagePopUps.Count);
                DisplayPopUp(cH, _halfPagePopUps[ran]);
                return true;
            }
        }
    }

    public void DisplayPopUpStatic(ClickableHolder cH, PopUp _popUpScript)
    {
        // If the popup is currently being animated in some shape or form it should not take any other input
        if (_animating) return;

        _tH.LockInput(true);

        _popUpScript.UpdateText(cH);

        _animating = true;

        // Toggling the grayscale fake effect 
        _grayScaleImage.enabled = true;
        _grayScaleImage.color = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0);
        Color fade = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0.8f);
        _grayScaleImage.DOColor(fade, _VC.PopUp_AnimSpeed);

        Vector3 safeScale = _popUpScript.transform.localScale;
        _popUpScript.transform.localScale = Vector3.zero;
        _popUpScript.gameObject.SetActive(true);
        _popUpScript.transform.DOScale(safeScale, _VC.PopUp_AnimSpeed).OnComplete(() => {
            _animating = false;
        });

        
    }

    // return if the popup is currently being shown
    public bool Showing()
    {
        foreach(PopUp p in _popUps)
        {
            if (p.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        foreach(PopUp p in _halfPagePopUps)
        {
            if (p.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
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

        if (Time.time - _lastHiding < _clickableCooldown) return;

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
                  
        _lastHiding = Time.time;

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
            if (_currentClickable != null)
            {
                if (_currentClickable._outline != null)
                {
                    _currentClickable._outline.ToggleOutline(false);
                    _currentClickable.transform.parent.transform.parent = _grayScaleParentSafe;
                    _currentClickable.transform.parent.transform.SetSiblingIndex(_grayScaleSiblingIndexSafe);
                }
            }
           
            // Toggling the grayscale fake effect 
            _grayScaleImage.enabled = false;
            

            _tutorialManager.PopUpClosing();
            _qM.ClosePopUp();
            _tH.UnlockInput();
        });
        
    }

    
}
