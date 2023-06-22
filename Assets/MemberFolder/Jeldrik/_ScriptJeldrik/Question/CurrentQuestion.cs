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

    [SerializeField] private GameObject _forwardButton;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private Image _image;

    private QuestionManager _qM;
    private Question _currentQ;
    private TouchHandler _tH;
    #endregion

    void Awake()
    {
        _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
        _tH = Camera.main.transform.GetComponent<TouchHandler>();
        ToggleBackButton(false);
        ToggleForwardButton(false);

    }

    void Update()
    {
        // Positioning object on screen
        //transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - Camera.main.orthographicSize * 0.9f, transform.position.z);
        ButtonLogic();
    }

    public void ToggleSelf(bool toggle)
    {
        gameObject.SetActive(toggle);

    }

    // Change currently display question and display new one with small animation
    public void ChangeCurrentQuestion(Question _q)
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
        UpdateQuestionCounter(_currentQ);
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
    public void UpdateQuestionCounter(Question _q)
    {
        if (_q.AmountNeeded[0] <= 1)
        {
            _questionCounter.text = "";
        }
        else
        {
            if (_q.AmountNeeded.Count > 1)
            {
                _questionCounter.text = _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0].ToString() + "/" + _currentQ.AmountNeeded[0].ToString() + "\n"
                    + _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][1].ToString() + "/" + _currentQ.AmountNeeded[1].ToString() + "\n";
            }
            else
            {
                _questionCounter.text = _qM._questionObjectCounts[_qM.GetCurrentQuestionId()][0].ToString() + "/" + _currentQ.AmountNeeded[0].ToString();
            }
            
        }
        
    }
}
