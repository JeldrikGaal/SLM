using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CurrentQuestion : MonoBehaviour
{

    #region References
    [SerializeField] private TMP_Text _questionTitle;
    [SerializeField] private TMP_Text _questionTextImage;
    [SerializeField] private TMP_Text _questionCounter;
    [SerializeField] private TMP_Text _questionCounterAnim;

    [SerializeField] private GameObject _forwardButton;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private Image _image;
    [SerializeField] private SlideColorStripe _slideColorStripe;
    [SerializeField] private ProgressionSystem _progression;

    private QuestionManager _qM;
    private Question _currentQ;
    private ClickableManager _clickableManager;
    private TouchHandler _tH;

   

    [HideInInspector] public bool _sliding;
    #endregion

    void Awake()
    {
        _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
        _tH = Camera.main.transform.GetComponent<TouchHandler>();
        _clickableManager = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        ToggleBackButton(false);
        ToggleForwardButton(false);

    }

    void Start()
    {
        //gameObject.SetActive(false);
        _questionCounterAnim.text = "";
    }
    void Update()
    {
        // Positioning object on screen
        //transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - Camera.main.orthographicSize * 0.9f, transform.position.z);
        if(!_sliding) ButtonLogic();
    }

    public void ToggleSelf(bool toggle)
    {
        gameObject.SetActive(toggle);

    }

    public void ChangeCurrentQuestion(Question _q)
    {
        _slideColorStripe.CompleteSlide(_q);
    }

    // Change currently display question and display new one with small animation
    public void ChangeCurrentQuestionInternal(Question _q, bool questionCounter = true)
    {
        _currentQ = _q;

        // Display an image next to the question if the question requires it -> for that activate the proper text boxxes and fill them with the needed text
        if (_currentQ.Image != null)
        {
            _image.enabled = true;
            _image.sprite = _currentQ.Image;
            _questionTitle.text = "";
            _questionTextImage.text = _currentQ.Text;
        }
        else
        {
            // Setting new content
            _image.enabled = false;
            _questionTextImage.text = "";
            _questionTitle.text = _currentQ.Text;
        }
        UpdateQuestionCounter(_currentQ, questionCounter);
        _progression.ShowOrder(_progression._orders[_qM.GetCurrentQuestionId()], true);
    }

    public void ToggleForwardButton(bool toggle)
    {
        _forwardButton.SetActive(toggle);
    }

    public void ToggleBackButton(bool toggle)
    {
        _backButton.SetActive(toggle);
    }

    public void Forward()
    {
        _qM.Forward();
    }

    public void Backward()
    {
        _qM.Backward();
    }

    // Show and hide forward and backward buttons 
    private void ButtonLogic()
    {
        if (_sliding) return;
        if (_qM.GetCurrentQuestionId() == 0 && _qM.IsCurrentQuestionCompleted())
        {
            ToggleForwardButton(true);
            ToggleBackButton(false);
        }
        else
        {
            ToggleForwardButton(false);
        }
        if (_qM.GetCurrentQuestionId() > 0 ) 
        {
            if (_qM.IsCurrentQuestionCompleted())
            {
                ToggleForwardButton(true);
            }
            ToggleBackButton(true);
        }
        if (_qM.GetCurrentQuestionId() == _qM.GetQuestionAmount() - 1)
        {
            ToggleForwardButton(false);
            ToggleBackButton(true);
        }
    }
     

    // Updates the question counter
    public void UpdateQuestionCounter(Question _q, bool spawnEffect = false)
    {
        if (_q.ObjectsToFind1.Count <= 1)
        {
            _questionCounter.text = "";
        }
        else
        {
            //_questionCounter.text = _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0].ToString() + "/" + _currentQ.AmountNeeded[0].ToString();
            _questionCounter.text =   _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0] + "/" + _q.ObjectsToFind1.Count.ToString();
            
            if (spawnEffect)
            {
                _questionCounterAnim.text = _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0] + "/" + _q.ObjectsToFind1.Count.ToString();
                CounterAnimation(4, 0.8f);
            } 
            else 
            {
                _questionCounterAnim.text = "";
            }
            /*if (_q.AmountNeeded.Count > 1)
            {
                _questionCounter.text = _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0].ToString() + "/" + _currentQ.AmountNeeded[0].ToString() + "\n"
                    + _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][1].ToString() + "/" + _currentQ.AmountNeeded[1].ToString() + "\n";
            }
            else
            {
                
            }*/
            
        }
        
    }

    // Function to show an animated effect to make it clear the user found and object
    public void CounterAnimation(float scale, float time)
    {
        _questionCounterAnim.transform.position = Camera.main.WorldToScreenPoint(_clickableManager.GetCurrentClickable().transform.position);
        _questionCounterAnim.color = new Color(_questionCounterAnim.color.r, _questionCounterAnim.color.g, _questionCounterAnim.color.b, 1f);
        _questionCounterAnim.transform.localScale = Vector3.one;
        _questionCounterAnim.transform.DOScale(new Vector3(scale * 0.5f,scale * 0.5f,scale * 0.5f), time * 0.5f).SetEase(Ease.Linear).OnComplete(()=>{

            _questionCounterAnim.transform.DOScale(new Vector3(scale, scale, scale),  time * 0.5f).SetEase(Ease.Linear);
            Color c = new Color(_questionCounterAnim.color.r, _questionCounterAnim.color.g, _questionCounterAnim.color.b, 0f);
            //_questionCounter.DOColor(c,  time * 0.5f);
            //_questionCounter.color = c;
            DOTween.To(() => _questionCounterAnim.color, x => _questionCounterAnim.color = x, c, time * 0.5f);
        });
    }
}
