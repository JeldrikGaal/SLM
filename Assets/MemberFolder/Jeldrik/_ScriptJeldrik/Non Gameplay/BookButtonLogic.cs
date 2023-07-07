using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.SimpleLocalization;
using System;

public class BookButtonLogic : MonoBehaviour
{

    [SerializeField] private float _animationTime;
    [SerializeField] private float _scaleModifier;
    [SerializeField] private float _repeatTime;
    [SerializeField] private float _blockTime;
    [SerializeField] private SlideColorStripe _colorStripe;
    [SerializeField] private MG1Tutorial _tutorialManager;
    [SerializeField] private QuestionManager _qM;
    private float _repeatStartTime;
    private float _repeatTimer;
    private bool _repeating;
    private Vector3 _originalScale;
    private TouchHandler _tH;
    private LeanDragCamera _dragController;
    
    
    // Next Question Question References 
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private Image _continueArrow;
    [SerializeField] private GameObject _goOnButton;
    [SerializeField] private TMP_Text _continueButtonText;
    [SerializeField] private TMP_Text _goOnButtonText;
    [SerializeField] private Image _goOnArrow;
    [SerializeField] private TMP_Text _nextQuestionQuestionText;
    private Image _grayScaleImage;
    private ClickableManager _cM;

     
    

    // Start is called before the first frame update
    void Start()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cM = GameObject.Find("ClickableManager").GetComponent<ClickableManager>();
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
        _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<MG1Tutorial>();
        _grayScaleImage = _cM._grayScaleImage;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (_repeating && Time.time - _repeatStartTime > _repeatTime) 
        {
            RepeatAnim();
            _repeatStartTime = Time.time;
        }

        if (_repeating)
        {
            if (Input.touchCount > 0 && Time.time - _repeatStartTime > _blockTime)
            {
                End();
            }
            if (Input.GetMouseButtonDown(0) && Time.time - _repeatStartTime > _blockTime)
            {
                End();  
            }
        }
    }


    public void AskQuestion(float time = 0.75f)
    {
        _continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _goOnButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _continueButton.GetComponent<Button>().onClick.AddListener(DisableStuff);
        _goOnButton.GetComponent<Button>().onClick.AddListener(OpenBook);

        // Enable grayScaleImage to make text more prominent
        _grayScaleImage.enabled = true;
        _grayScaleImage.color = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0);
        Color fade = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0.8f);
        _grayScaleImage.DOColor(fade,time);

        // Enabling object
        _continueButton.SetActive(true);
        _goOnButton.SetActive(true);
        _nextQuestionQuestionText.gameObject.SetActive(true);
        // Setting Text 
        _nextQuestionQuestionText.text = LocalizationManager.Localize("Book.Question");
        _goOnButtonText.text = LocalizationManager.Localize("Book.Yes");
        _continueButtonText.text = LocalizationManager.Localize("Book.No");
        _tH.LockInput();        

        SlideColorStripe.ChangeAlpha(_continueButtonText, 0);
        SlideColorStripe.ChangeAlpha(_continueArrow,0);
        SlideColorStripe.ChangeAlpha(_goOnButtonText,0);
        SlideColorStripe.ChangeAlpha(_goOnArrow,0);
        SlideColorStripe.ChangeAlpha(_nextQuestionQuestionText,0);

        SlideColorStripe.DOAlpha(_continueButtonText,1, time);
        SlideColorStripe.DOAlpha(_continueArrow,1, time);
        SlideColorStripe.DOAlpha(_goOnButtonText,1, time);
        SlideColorStripe.DOAlpha(_goOnArrow,1, time);
        SlideColorStripe.DOAlpha(_nextQuestionQuestionText,1, time);

    }

    public void DisableStuff()
    {
        float time = 0.75f;
        Color fade = new Color(_grayScaleImage.color.r, _grayScaleImage.color.g, _grayScaleImage.color.b, 0);
        _grayScaleImage.DOColor(fade, time);

        SlideColorStripe.DOAlpha(_continueButtonText, 0, time);
        SlideColorStripe.DOAlpha(_continueArrow, 0, time);
        SlideColorStripe.DOAlpha(_goOnButtonText, 0, time);
        SlideColorStripe.DOAlpha(_goOnArrow, 0, time);
        SlideColorStripe.DOAlpha(_nextQuestionQuestionText, 0, time);

        _tH.LockInput();   
    }


    public void SpawnAnimation()
    {

        _colorStripe.OrangeAppear();
        _tutorialManager.MovePopUp();
        _tutorialManager.EnablePopUp(0.75f, "TutorialText3");

        _originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(_originalScale * _scaleModifier, _animationTime));
        seq.Append(transform.DOScale(_originalScale * (_scaleModifier - 1), _animationTime));
        seq.Append(transform.DOScale(_originalScale, _animationTime));

        _repeatStartTime = Time.time + Mathf.Max((_repeatTime - _animationTime * 3), 1f);
        _repeating = true;
    }

    public void End()
    {        
        _tH.UnlockInput();
        _tutorialManager.DisablePopUp(0.75f);
        _repeating = false;

        Invoke("InvokeStartTutorial", 1f);
    }

    private void InvokeStartTutorial()
    {
        _tutorialManager.StartTutorial();
    }

    private void RepeatAnim()
    {
        _originalScale = transform.localScale;
        transform.DOScale(_originalScale * _scaleModifier, _animationTime).OnComplete(() => {
            transform.DOScale(_originalScale, _animationTime);
        });
    }

    public void OpenBook()
    {
        DisableStuff();
        _tutorialManager.Done = true;
        SceneManager.LoadScene("Book_Main");
        if (_qM == null)
        {
            _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
        }
        _qM.SafeSwirls();
    }

    
}
