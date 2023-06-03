using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _questionsText = new List<TMP_Text>();

    [SerializeField] private List<Image> _questionStatus = new List<Image>();
   

    [SerializeField] private List<Question> _questions = new List<Question>();

    
    private int _currentQ = -1;
    private float _timeOnCurrentQ;


    // Start is called before the first frame update
    void Start()
    {
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
        }
        
    }

    private void Update()
    {
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
    }

    // Gets called by Clickables whenever they are sucessfully clicked -> Questionmanager checks if its the currently needed object
    public void ObjectClick(ClickableHolder cH)
    {
        if (_questions[_currentQ].ObjectToFind == cH)
        {
            CompleteCurrentQuestion();
        }
    }

    private void End()
    {
        Debug.Log("Last Question Done ! ");
    }
    
}
