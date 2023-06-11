using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    #region References    
    [Tooltip("TMP_Text Objects for each question from the menu overview")]
    [SerializeField] private List<TMP_Text> _questionsText = new List<TMP_Text>();

    [Tooltip("TMP_Text Objects for each question from the menu overview")]
    [SerializeField] private List<Image> _questionStatus = new List<Image>();

    [Tooltip("Question objects in order of needed completion")]
    [SerializeField] private List<Question> _questions = new List<Question>();

    [Tooltip("Prefab for spawning confetti upon question completion")]
    [SerializeField] private GameObject _confetti;

    [Tooltip("Reference to the current question menu")]
    [SerializeField] private CurrentQuestion _currentQuestion;

    [HideInInspector] public TouchHandler _tH;

    #endregion

    private int _currentQ = -1;
    private float _timeOnCurrentQ;

    // Start is called before the first frame update
    void Start()
    {
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
        }

        // Getting references 
        _tH = Camera.main.GetComponent<TouchHandler>();
        
        // Starts off disables
        this.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        // Always being display in the middle of the screen
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }

    // Mark a questions as completed and proceed to the next one
    private void CompleteCurrentQuestion()
    {
        
        _questionStatus[_currentQ].color = Color.green;
        

        if (_questions[_currentQ].End)
        {
            End();
            return;
        }

        _currentQ++;
        _currentQuestion.ChangeCurrentQuestion(_questions[_currentQ]);
    }

    // Gets called by Clickables whenever they are sucessfully clicked -> Questionmanager checks if its the currently needed object
    public void ObjectClick(ClickableHolder cH)
    {
        if (_questions[_currentQ].ObjectToFind == cH)
        {
            GameObject temp = Instantiate(_confetti, Camera.main.transform);
            Destroy(temp, 4);
            CompleteCurrentQuestion();
        }
    }

    // Handles completing all questions
    private void End()
    {
        Debug.Log("==== Last Question Done ! ====");

        Camera c = Camera.main;
        DOTween.To(() => c.orthographicSize, x => c.orthographicSize = x, 0f, 2f).OnComplete(() =>
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        });

        
    }

    // Return the currently selected Question
    public Question GetCurrentQuestion()
    {
        return _questions[_currentQ];
    }
}
