using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfoPopUp : MonoBehaviour
{
    private TouchHandler _tH;
    private int _dummyCounter;
    private int _dummyCounterGoal;

    // Start is called before the first frame update
    void Start()
    {
        _tH = Camera.main.GetComponent<TouchHandler>();
        _dummyCounter = 0;
        _dummyCounterGoal = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // If the user does not understand to click the button and clicks more than _dummyCounterGoal times just anywhere in the game field the popup also will disappear
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
        _tH.UnlockInput();
        Destroy(this.gameObject);
    }
}
