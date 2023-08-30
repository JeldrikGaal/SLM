using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using DG.Tweening;
using Lean.Touch;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MG1Tutorial : MonoBehaviour
{
    #region References
    
    [SerializeField] public Clickable _exampleClickable;
    [SerializeField] private SceneInfoPopUp _sceneInfoPopUp;
    [SerializeField] private GameObject _currentQuestion;
    // List starting from top going clockwise
    [SerializeField] private List<Image> _moveArrows;

    [SerializeField] private GameObject _tutorialPopUp;
    [SerializeField] private GameObject _progressionSystem;
    [SerializeField] private SlideColorStripe _colorStripe;
    private TouchHandler _tH;
    private Transform _moveArrowsHolder;
    private LeanDragCamera _dragController;
    

    private bool _running;
    public bool _runningStep2;
    private bool _effectRunning ;
    private float _effectWaitTime = 3f;
    private float _effectStartTime;
    public bool Done;
    public bool infoShowed1;
    public bool infoShowed2;
    public bool SKIPTUTORIAL;   
    [SerializeField] private BookButtonLogic _bookButton;
    private Vector3 _lastPopUpPos;
    private ClickableManager _cM;
    #endregion

    public static MG1Tutorial Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            //DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        _progressionSystem.SetActive(false);
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
        _moveArrowsHolder = _moveArrows[0].transform.parent;
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();

        DOTween.SetTweensCapacity(1000,100);

        Invoke("InvokeStart", 0.75f);
    }

    private void InvokeStart()
    {
        if (SKIPTUTORIAL || Done) 
        { 
            Appear();
            _tH.UnlockInput();
            _progressionSystem.SetActive(true);
        }
        else 
        {
            _currentQuestion.gameObject.SetActive(false);
            //_bookButton.SpawnAnimation();
            ShowSceneInfo();
        }
    }

    public void Appear() 
    {
        _colorStripe.Appear();
    }

    public void Update()
    {

        // Handle displaying the bouncing effect for the clickable of the first tutorial part
        if (_effectRunning)
        {
            if (Time.time - _effectStartTime > _effectWaitTime)
            {
                EffectForClickable();
            }
        }

        // Handle the logic for waiting for the player to drag the camera around enough
        if (_runningStep2) 
        {
            if (_dragController._currentDragMoveDist > 2)
            {
               FadeMoveArrows(0.75f);
               DisablePopUp(0.75f);
               Invoke("EndTutorial", 0.75f);
            }
        } 
    }

    #region Tutorial Steps
    public void StartTutorial()
    {
        _running = true;
        _effectRunning = true;
        
        // Disabling touch input
        _dragController.enabled = false;
        
        _colorStripe.OrangeAppear();
        
        // Enabling and setting text for info popup
        EnablePopUp(1.5f, "TutorialText1");

        EffectForClickable();
    }

    private void Step2() 
    {
        _runningStep2 = true;
        
        _currentQuestion.gameObject.SetActive(false);

        // Enabling the info popup and setting the text
        EnablePopUp(0.75f, "TutorialText2");
        EnableMoveArrows();
        EffectForMoveArrows(0.6f, 60, 3f);
        
        _currentQuestion.gameObject.SetActive(false);

    }
    public void EndTutorial()
    {
        DisablePopUp(0.75f);
        if ( _runningStep2)
        {
            Invoke("RealEnd", 0.75f);
        }        
        _running = false;
        _runningStep2 = false;
        this.enabled = false;
    }
    #endregion

    public void PopUpOpening() 
    {
        _effectRunning = false;
        DisablePopUp(0.75f);
        
    }

    public void PopUpClosing()
    {
        if (_running)
        {
            _colorStripe.InstantDisappear();
            Step2();
        }
        if (infoShowed1 && !Done)
        {
            Appear();
            infoShowed2 = true;
           
        }
        if (infoShowed2 && !Done)
        {
            //Appear();
            Done = true;
        }
    }


    public void EnablePopUp(float time, string localizationKey)
    {
        _tutorialPopUp.SetActive(true);
        Image img = _tutorialPopUp.GetComponent<Image>();
        TMP_Text tex = _tutorialPopUp.GetComponentInChildren<TMP_Text>();
        Color c = new Color(img.color.r, img.color.g, img.color.b, 0f);
        Color c2 = new Color(tex.color.r, tex.color.g, tex.color.b, 1f);
        img.color = c;
        _tutorialPopUp.GetComponentInChildren<TMP_Text>().text = LocalizationManager.Localize(localizationKey);
        c = new Color(img.color.r, img.color.g, img.color.b, 1f);
        img.DOColor(c, time);
        tex.DOColor(c2, time);
    }

    public void DisablePopUp(float time)
    {
        Image img = _tutorialPopUp.GetComponent<Image>();
        TMP_Text tex = _tutorialPopUp.GetComponentInChildren<TMP_Text>();
        Color c = new Color(img.color.r, img.color.g, img.color.b, 0f);
        Color c2 = new Color(tex.color.r, tex.color.g, tex.color.b, 0f);
        tex.DOColor(c2, time);	
        img.DOColor(c, time).OnComplete(()=>
        {
            _tutorialPopUp.SetActive(false);
        });
        
    }

    public void MovePopUp()
    {
        _lastPopUpPos = _tutorialPopUp.transform.localPosition;
        _tutorialPopUp.transform.localPosition = new Vector3(650, 0 ,0);
    }
    public void MovePopUp(Vector3 pos)
    {
        _lastPopUpPos = _tutorialPopUp.transform.localPosition;
        _tutorialPopUp.transform.localPosition = pos;
    }

    public void ResetPopUp()
    {
         _tutorialPopUp.transform.localPosition = _lastPopUpPos;
    }

    public void RealEnd()
    {
        _tH.UnlockInput();
        Done = true;
        _cM._tutorialBlock = false;
        _currentQuestion.gameObject.SetActive(true);
        _colorStripe.CompleteSlide(_colorStripe._q);
        //_tutorialManager.ResetPopUp();
        _dragController.enabled = true;
        SKIPTUTORIAL = true;
        _progressionSystem.gameObject.SetActive(true);
    }

    private void ShowSceneInfo()
    {   
        infoShowed1 = true;
        _sceneInfoPopUp.gameObject.SetActive(true);
        _dragController.enabled = false;
    }

    private void EnableMoveArrows()
    {
        for (int i = 0; i < _moveArrows.Count; i++)
        {
            _moveArrows[i].GetComponent<Image>().enabled = true;
        }
        _cM._tutorialBlock = true;
    }

    // Fade out the Arrows once the user has moved the camera enough
    public void FadeMoveArrows(float time) 
    {
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < _moveArrows.Count; i++)
        {
            Image img = _moveArrows[i].GetComponent<Image>();
            Color c = new Color(img.color.r, img.color.g, img.color.b, 0);
            seq.Insert(0, img.DOColor(c, time));
        }
    }

    // Makes the arrows bounce in scale and move themselve and the camera one after another starting from the top and going clockwise
    public void EffectForMoveArrows(float effect, float distance, float duration) 
    {

        _colorStripe.Disappear();

        // Creating the needed sequence objects for the arrows, the holder object and the camera
        Sequence seq = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();
        Sequence seq3 = DOTween.Sequence();

        // Bounce the arrows scale
        for (int i = 0; i < _moveArrows.Count; i++)
        {
            Vector3 orgScale = _moveArrows[i].transform.localScale;
            seq.Append(_moveArrows[i].transform.DOScale(orgScale * (1 + effect), duration * 0.25f * 0.5f));
            //seq.Append(_moveArrows[i].transform.DOScale(orgScale * (1 - effect), duration * 0.25f * 0.33f));
            seq.Append(_moveArrows[i].transform.DOScale(orgScale, duration * 0.25f * 0.5f));
        }

        // Saving original positions to move back and forth
        Vector3 orgPos = _moveArrowsHolder.transform.localPosition;
        Vector3 orgPosCam = Camera.main.transform.position;

        // Moving the arrows and the camera together
        float camMod = 0.8f;
        for (int i = 0; i < _moveArrows.Count; i++)
        {
            // If its an even number its either top or bottom 
            if (i%2 == 0)
            {
                // Checking if its bottom or top and changing the sign of the distance accordingly 
                float d = i == 0 ? distance : -distance;

                // Appending all tweens for arrow and cam
                seq2.Append(_moveArrowsHolder.DOLocalMoveY(orgPos.y + d ,duration * 0.25f * 0.5f));
                seq2.Append(_moveArrowsHolder.DOLocalMoveY(orgPos.y, duration * 0.25f * 0.5f));    
                seq3.Append(Camera.main.transform.DOLocalMoveY(orgPosCam.y + d * camMod ,duration * 0.25f * 0.5f));
                seq3.Append(Camera.main.transform.DOLocalMoveY(orgPosCam.y, duration * 0.25f * 0.5f));  
            }
            else 
            {
                // Checking if its right or left  and changing the sign of the distance accordingly 
                float d = i == 1 ? distance : -distance;

                // Appending all tweens for arrow and cam
                seq2.Append(_moveArrowsHolder.DOLocalMoveX(orgPos.x + d ,duration * 0.25f * 0.5f));
                seq2.Append(_moveArrowsHolder.DOLocalMoveX(orgPos.x, duration * 0.25f * 0.5f)); 
                seq3.Append(Camera.main.transform.DOLocalMoveX(orgPosCam.x + d * camMod ,duration * 0.25f * 0.5f));
                seq3.Append(Camera.main.transform.DOLocalMoveX(orgPosCam.x, duration * 0.25f * 0.5f));       
            }
        } 
        seq3.OnComplete(() => {
            // Enabling touch controls
            _dragController.enabled = true;
        });
    }

    public void EffectForClickable()
    {
        // Setting the start of the effect being played
        _effectStartTime = Time.time;
        // Saving the original scale to later retrieve it 
        Vector3 orgScale = _exampleClickable.transform.localScale;
        // Turning the outline on so it looks better e
        _exampleClickable.transform.GetComponent<MakeOutline>().ToggleOutline(true);

        // Creating Dotween sequence that lets the object pulsate to indicate it needs to be clicked
        Sequence seq = DOTween.Sequence();

        seq.Append(_exampleClickable.transform.DOScale(orgScale * 1.25f, 0.5f));
        seq.Append(_exampleClickable.transform.DOScale(orgScale * 0.88f, 0.5f));
        seq.Append(_exampleClickable.transform.DOScale(orgScale, 0.5f));

    }
}
