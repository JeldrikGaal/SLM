using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Assets.SimpleLocalization;
using TMPro;

public class SceneInfoPopUp : MonoBehaviour
{
    private VALUECONTROLER _VC;
    private TouchHandler _tH;
    private int _dummyCounter;
    private int _dummyCounterGoal;

    private GameObject _canvas;
    private GameObject _reminderRef;
    private bool _spawnedReminder;

    private float _spawnTime;
    [SerializeField] private TMP_Text _text;
    [SerializeField] GameObject _reminderPrefab;


    // Start is called before the first frame update
    void Start()
    {
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();

        // Getting references
        _tH = Camera.main.GetComponent<TouchHandler>();

        // Dummy counter set up
        _dummyCounter = 0;
        _dummyCounterGoal = 5;

        transform.position = new Vector3( Camera.main.transform.position.x,  Camera.main.transform.position.y, 0);

        // Animate Popup to appear
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), _VC.SceneInfo_AnimSpeed).SetEase(Ease.InSine);

        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _spawnTime = Time.time;

        _text.text = LocalizationManager.Localize("PopUpText");
    }

    // Update is called once per frame
    void Update()
    {
        //DummyCounterLogic();
        
        if (Time.time - _spawnTime > _VC.SceneInfo_BlockTime)
        {
            // TODO: also include touch input
            if (Input.GetMouseButtonDown(0))
            {
                EndPopUp();
            }
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    EndPopUp();
                }
            }
        }


        if (Time.time - _spawnTime > _VC.SceneInfo_ReminderTime && !_spawnedReminder)
        {
            _reminderRef = Instantiate(_reminderPrefab, _canvas.transform);
            _reminderRef.transform.name = "SceneInfo";
            _reminderRef.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 0.9f
                , _reminderRef.transform.position.z);
            _reminderRef.GetComponent<ReminerPopUp>().Show();
            _spawnedReminder = true;
        }

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
            Destroy(_reminderRef);
            Destroy(this.gameObject);
        });
        
    }
}
