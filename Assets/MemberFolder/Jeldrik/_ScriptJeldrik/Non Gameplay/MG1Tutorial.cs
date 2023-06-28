using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class MG1Tutorial : MonoBehaviour
{
    #region References
    
    [SerializeField] private Clickable _exampleClickable;
    [SerializeField] private SceneInfoPopUp _sceneInfoPopUp;
    [SerializeField] private GameObject _currentQuestion;
    private LeanDragCamera _dragController;

    private bool _running;
    private float _effectWaitTime = 3f;
    private float _effectStartTime;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
        StartTutorial();
    }

    public void Update()
    {
        /*if (_exampleClickable._clicked)
        {
            EndTutorial();
        }*/

        if (_running)
        {
            if (Time.time - _effectStartTime > _effectWaitTime)
            {
                EffectForClickable();
            }
        }
    }

    public void StartTutorial()
    {
        _dragController.enabled = false;
        _running = true;
        EffectForClickable();
    }
    public void EndTutorial()
    {
        _dragController.enabled = true;
        _running = false;
        _currentQuestion.SetActive(true);

        this.enabled = false;
    }

    public void PopUpClosing()
    {
        if (_running)
        {
            _sceneInfoPopUp.gameObject.SetActive(true);
        }
        EndTutorial();
        
    }

    public void EffectForClickable()
    {
        _effectStartTime = Time.time;
        Vector3 orgScale = _exampleClickable.transform.localScale;

        Sequence seq = DOTween.Sequence();

        seq.Append(_exampleClickable.transform.DOScale(orgScale * 1.25f, 0.5f));
        seq.Append(_exampleClickable.transform.DOScale(orgScale * 0.88f, 0.5f));
        seq.Append(_exampleClickable.transform.DOScale(orgScale, 0.5f));

    }
}
