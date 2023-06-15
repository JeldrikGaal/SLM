using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization;

public class QuestionManager : MonoBehaviour
{
    #region References    
    [Tooltip("TMP_Text Objects for each question from the menu overview")]
    [SerializeField] private List<TMP_Text> _questionsText = new List<TMP_Text>();

    [Tooltip("TMP_Text Objects for each question from the menu overview")]
    [SerializeField] private List<Image> _questionStatus = new List<Image>();

    [Tooltip("Question objects in order of needed completion")]
    private List<Question> _questions = new List<Question>();

    [Tooltip("Prefab for spawning confetti upon question completion")]
    [SerializeField] private GameObject _confetti;

    [Tooltip("Reference to the current question menu")]
    [SerializeField] private CurrentQuestion _currentQuestion;

    [Tooltip("Reference to the book transition object")]
    [SerializeField] private BookTransition _bT;

    [HideInInspector] public TouchHandler _tH;
    private ClickableStorage _cS;
    #endregion

    public List<List<int>> _questionObjectCounts = new List<List<int>>();
    private List<ClickableHolder> _alreadyClicked = new List<ClickableHolder>();
    private List<List<ClickableHolder>> _alreadyClickedComb = new List<List<ClickableHolder>>();
    private List<bool> _completedQuestions = new List<bool>();

    //private List<List<int>> _question = new List<List<int>>();

    private int _currentQ = -1;
    private float _timeOnCurrentQ;
    private bool _completed;
    private VALUECONTROLER _VC;


    private void Awake()
    {
        
    }

    void Start()
    {
        // VC
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _questions = _VC.Questions;

        foreach (Question q in _questions)
        {
            q.Text = LocalizationManager.Localize(q.LocalizationKey); 
        }

        // Loading and displaying question objects in the menu
        if (_questions.Count < 3)
        {
            Debug.LogWarning("Not enough Questions provided to properly load");
        }
        else
        {
            _questionsText[0].text = _questions[0].Text;
            _questionsText[1].text = _questions[1].Text;
            _questionsText[2].text = _questions[2].Text;
            _currentQ = 0;
            _timeOnCurrentQ = Time.time;
            _currentQuestion.ChangeCurrentQuestion(GetCurrentQuestion());
            for (int i = 0; i < 3; i++)
            {
                _questionObjectCounts.Add(new List<int>());
                _questionObjectCounts.Add(new List<int>());
                _questionObjectCounts.Add(new List<int>());
            }
            for (int i = 0; i < 3; i++)
            {
                _questionObjectCounts[i].Add(0);
                _questionObjectCounts[i].Add(0);
                _questionObjectCounts[i].Add(0);
            }
        }

        // Getting references 
        _tH = Camera.main.GetComponent<TouchHandler>();
        _cS = GameObject.Find("ClickableStorage").GetComponent<ClickableStorage>();

        _completedQuestions.Add(false);
        _completedQuestions.Add(false);
        _completedQuestions.Add(false);
        if (_cS._completedQuestionsSave.Count < 3)
        {
            _cS._completedQuestionsSave = _completedQuestions;
        }

        // Loading current Question from storage and displaying it
        LoadingCurrentQuestion();

        // Starts off disables
        this.gameObject.SetActive(false);
        
        
    }

    private void Update()
    {
        // Always being display in the middle of the screen
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        
    }

    public bool IsCurrentQuestionCompleted()
    {
        return _completedQuestions[_currentQ];
    }

    private void LoadingCurrentQuestion()
    {
        _currentQ = _cS.GetCurrentQuestion();
        _alreadyClicked = _cS._alreadyClickedSave;
        _alreadyClickedComb = _cS._alreadyClickedCombSave;
        _questionObjectCounts = _cS._questionObjectCountsSave;
        _completedQuestions = _cS._completedQuestionsSave;
        _currentQuestion.ChangeCurrentQuestion(_questions[_currentQ]);
    }

    private void SavingCurrentQuestion()
    {
        _cS.SafeCurrentQuestion(_currentQ);
        _cS.SafeClickablesList(_alreadyClicked, _alreadyClickedComb);
        _cS._questionObjectCountsSave = _questionObjectCounts;    
        _cS._completedQuestionsSave = _completedQuestions;
    }

    public int GetCurrentQuestionId()
    {
        return _currentQ;
    }

    public int GetQuestionAmount()
    {
        return _questions.Count;
    }

    public void Forward()
    {

        _currentQ++;
        //_completed = false;
        ResetCounts();
        _currentQuestion.ChangeCurrentQuestion(_questions[_currentQ]);
        SavingCurrentQuestion();
    }

    public void Backward()
    {
        _currentQ--;
        //_completed = true;
        ResetCounts();
        _currentQuestion.ChangeCurrentQuestion(_questions[_currentQ]);
        
    }

    // Mark a questions as completed and proceed to the next one
    private void CompleteCurrentQuestion()
    {
        
        //if (_currentQ <= 2) _questionStatus[_currentQ].color = Color.green;
        

        if (_questions[_currentQ].End)
        {
            End();
            return;
        }

        //_currentQ++;
        _completed = true;
        SavingCurrentQuestion();
        _completedQuestions[_currentQ] = true;
        //_currentQuestion.ChangeCurrentQuestion(_questions[_currentQ]);
    }

    // Gets called by Clickables whenever they are sucessfully clicked -> Questionmanager checks if its the currently needed object
    public void ObjectClick(ClickableHolder cH)
    {
        // Check if cH is relevant to current questions
        if (CheckQuestionContaining(cH))
        {
            // Skip if object has been clicked before
            if (_alreadyClicked.Contains(cH)) return;

            // Track already clicked objects
            _alreadyClicked.Add(cH);
            _questionObjectCounts[_currentQ][GetIDForClicked(cH)] += 1;

            _currentQuestion.UpdateQuestionCounter(_questions[_currentQ]);

            // Only complete question if enough objects have been clicked
            if (CheckQuestionCompletition())
            {
                GameObject temp = Instantiate(_confetti, Camera.main.transform);
                temp.transform.name = "QM";
                temp.transform.localScale *= _VC.Confetti_Size;
                Destroy(temp, 4);
                CompleteCurrentQuestion();
            }
        }       
    }

    // 
    public int GetIDForClicked(ClickableHolder cH)
    {
        if (_questions[_currentQ].ObjectsToFind1.Contains(cH))
        {
            return 0;
        }
        if (_questions[_currentQ].ObjectsToFind2.Contains(cH))
        {
            return 1;
        }
        if (_questions[_currentQ].ObjectsToFind3.Contains(cH))
        {
            return 2;
        }
        return -1;
    }

    // return bool if the cH already has been clicked
    private bool CheckAlreadyClicked(ClickableHolder cH)
    {
        if (_alreadyClickedComb[0].Contains(cH) || _alreadyClickedComb[0].Contains(cH) 
            || _alreadyClickedComb[2].Contains(cH))
        {
            return true;
        }
        return false;
    }

    // Return bool if the current question contains a cH in its reqiurements
    private bool CheckQuestionContaining(ClickableHolder cH)
    {
        if (_questions[_currentQ].ObjectsToFind1.Contains(cH) || _questions[_currentQ].ObjectsToFind2.Contains(cH)
            || _questions[_currentQ].ObjectsToFind3.Contains(cH))
        {
            return true;
        }
        return false;
    }

    // Returns bool if the current questions has been completed 
    public bool CheckQuestionCompletition()
    {
        for (int i = 0; i < GetCurrentQuestion().AmountNeeded.Count; i++)
        {
            if (_questionObjectCounts[_currentQ][i] < GetCurrentQuestion().AmountNeeded[i] && GetCurrentQuestion().AmountNeeded[i] > 0)
            {
                return false;
            }
        }
        return true;
    }

    private void ResetCounts()
    {
        /*_questionObjectCounts[0] = 0;
        _questionObjectCounts[1] = 0;
        _questionObjectCounts[2] = 0;*/
    }

    // Handles completing all questions
    private void End()
    {
        _bT.OpenBook();
        /*Debug.Log("==== Last Question Done ! ====");

        Camera c = Camera.main;
        DOTween.To(() => c.orthographicSize, x => c.orthographicSize = x, 0f, 2f).OnComplete(() =>
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        });*/
    }

    // Return the currently selected Question
    public Question GetCurrentQuestion()
    {
        return _questions[_currentQ];
    }
}
