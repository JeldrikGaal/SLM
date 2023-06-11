using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CurrentQuestion : MonoBehaviour
{

    #region References
    [SerializeField] private TMP_Text _questionTitle;
    [SerializeField] private TMP_Text _questionText;

    private QuestionManager _qM;
    private Question _currentQ;
    private TouchHandler _tH;
    #endregion

    void Awake()
    {
        _qM = GameObject.FindGameObjectWithTag("QuestionMenu").GetComponent<QuestionManager>();
        _tH = Camera.main.transform.GetComponent<TouchHandler>();
        
    }

    void Update()
    {
        // Positioning object on screen
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - _tH._height * 0.05f, transform.position.z);
        
    }

    public void ToggleSelf(bool toggle)
    {
        gameObject.SetActive(toggle);

    }

    // Change currently display question and display new one with small animation
    public void ChangeCurrentQuestion(Question _q)
    {
        _currentQ = _q;

        // Small animation 
        _questionTitle.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            // Setting new content
            _questionTitle.text = _currentQ.Text;
            _questionText.text = _currentQ.Hint;
        });
        _questionTitle.transform.DOScale(Vector3.one, 0.5f);


    }
}
