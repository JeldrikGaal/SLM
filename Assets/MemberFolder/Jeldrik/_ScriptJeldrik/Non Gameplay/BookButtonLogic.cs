using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using UnityEngine.SceneManagement;

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
     
    

    // Start is called before the first frame update
    void Start()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
        _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<MG1Tutorial>();

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

    public void SpawnAnimation()
    {

        _colorStripe.Appear();
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
        //_tutorialManager.ResetPopUp();
        _dragController.enabled = true;
        _repeating = false;
        _tutorialManager.SKIPTUTORIAL = true;
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
        SceneManager.LoadScene("Book_Main");
        if (_qM == null)
        {
            _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
        }
        _qM.SafeSwirls();
    }

    
}
