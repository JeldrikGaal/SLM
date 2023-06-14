using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ClickableStorage : MonoBehaviour
{
    static private GameObject _current = null;

    private List<ClickableHolder> _clickedQuestion = new List<ClickableHolder>();
    private List<ClickableHolder> _clickedInfo = new List<ClickableHolder>();

    private int _currentQuestionSave;
    [HideInInspector] public List<int> _questionObjectCountsSave = new List<int>();
    [HideInInspector] public List<ClickableHolder> _alreadyClickedSave = new List<ClickableHolder>();
    [HideInInspector] public List<List<ClickableHolder>> _alreadyClickedCombSave = new List<List<ClickableHolder>>();

    private void Awake()
    {
        // Ensuring theres only one clickable manager so it can persist throughout changing scenes
        if (_current == null)
        {
            _current = this.gameObject;
            DontDestroyOnLoad(gameObject);
            _questionObjectCountsSave.Add(0);
            _questionObjectCountsSave.Add(0);
            _questionObjectCountsSave.Add(0);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Adds a clickableholder object to the correct list 
    public bool AddToStorage(ClickableHolder cH)
    {
        if (_clickedInfo.Contains(cH) || _clickedQuestion.Contains(cH))
        {
            return false;
        }
        if (cH.Question)
        {
            _clickedQuestion.Add(cH);
            return true;
        }
        else
        {
            _clickedInfo.Add(cH);
            return true;
        }
    }

    public void SafeClickablesList(List<ClickableHolder> normal, List<List<ClickableHolder>> combined)
    {
        _alreadyClickedSave = normal;
        _alreadyClickedCombSave = combined;
    }

    public void SafeCurrentQuestion(int q)
    {
        _currentQuestionSave = q;
    }
    
    public int GetCurrentQuestion()
    {
        return _currentQuestionSave;
    }
    
}
