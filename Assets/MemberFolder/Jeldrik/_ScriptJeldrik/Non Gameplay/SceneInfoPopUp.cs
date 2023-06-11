using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneInfoPopUp : MonoBehaviour
{
    private TouchHandler _tH;
    private int _dummyCounter;
    private int _dummyCounterGoal;

    // Start is called before the first frame update
    void Start()
    {
        // Getting references
        _tH = Camera.main.GetComponent<TouchHandler>();

        // Dummy counter set up
        _dummyCounter = 0;
        _dummyCounterGoal = 5;

        // Animate Popup to appear
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.6f).SetEase(Ease.InSine);
    }

    // Update is called once per frame
    void Update()
    {
        DummyCounterLogic();
        
    }

    // If the user does not understand to click the button and clicks more than _dummyCounterGoal times just anywhere in the game field the popup also will disappear
    private void DummyCounterLogic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dummyCounter++;
            CheckDummyCounter();
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                _dummyCounter++;
                CheckDummyCounter();
            }
        }
    }

    // Checks if the dummy counter has beed reached
    private void CheckDummyCounter()
    {
        if (_dummyCounter >= _dummyCounterGoal)
        {
            EndPopUp();
        }
    }

    // Destroys the popup and unlocks the other input in the touch handler
    public void EndPopUp()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => 
        {
            _tH.UnlockInput();
            Destroy(this.gameObject);
        });
        
    }
}
