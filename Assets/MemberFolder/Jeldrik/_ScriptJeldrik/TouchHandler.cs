using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [HideInInspector]
    public float _width, _height;
    private Vector3 _position;

    [Tooltip("Factor with how fast the camera moves when being dragged around")]
    [SerializeField] private float _dragFactor = 0.1f;

    [Tooltip("How long to wait for next input to deploy a reminder")]
    [SerializeField] private float _waitTimeReminder;

    [Tooltip("How long to after Spawning a reminder for the next one")]
    [SerializeField] private float _waitTimeReminderWaitTime;


    private float _lastInputTime;

    [SerializeField] private GameObject _inputReminderObject;

    private QuestionMenuButton _qMB;

    private bool locked;

    private Vector2 _camLimits;
    private Transform _camTransform;

    private Vector3 mpS;
    private Vector3 mp;
    private Vector3 camPS;
    private GameObject _canvas;

   

    // Start is called before the first frame update
    void Awake()
    {
        _width = (float)Screen.width / 2.0f;
        _height = (float)Screen.height / 2.0f;
        _camLimits = new Vector2(65,39);
        _camTransform = Camera.main.transform;
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        locked = true;
        _lastInputTime = Time.time;
        _qMB = GameObject.FindGameObjectWithTag("QuestionMenuButton").GetComponent<QuestionMenuButton>();
        _qMB.ToggleSelf(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Prevents any input when the handler is locked
        if (locked) return;

        /*if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 pos = touch.position;
                pos.x = (pos.x - _width) / _width;
                pos.y = (pos.y - _height) / _height;
                _position = new Vector3(-pos.x, pos.y, 0.0f);
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 pos = touch.position;
                pos.x = (pos.x - _width) / _width;
                pos.y = (pos.y - _height) / _height;
            }
        }*/
    
        // Starting to drag the camera around ( with mouse controls for debugging purposes )
        if (Input.GetMouseButtonDown(0))
        {
            mpS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camPS = Camera.main.transform.position;

            LogInputTime();



        }
        // Actually dragging the camera around ( with mouse controls for debugging purposes )
        if (Input.GetMouseButton(0))
        {
            Vector3 dif = mpS - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = camPS + ( dif  * _dragFactor) ;
            _camTransform.position = new Vector3(
                Mathf.Max(-_camLimits.x, Mathf.Min(_camLimits.x, newPos.x)),
                Mathf.Max(-_camLimits.y, Mathf.Min(_camLimits.y, newPos.y)), _camTransform.position.z);

            LogInputTime();
        }

        ReminderLogic();
    }

    private void ReminderLogic()
    {
        if (Time.time - _lastInputTime > _waitTimeReminder)
        {
            SpawnInputReminder();
            _lastInputTime = Time.time + _waitTimeReminderWaitTime;
        }
    }

    private void SpawnInputReminder()
    {
        Debug.Log("SPAWN!");
        GameObject temp = Instantiate(_inputReminderObject, _canvas.transform);
        temp.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + _height * 0.05f, temp.transform.position.z);
        temp.GetComponent<ReminerPopUp>().Show();
    }

    private void LogInputTime()
    {
        _lastInputTime = Time.time;
    }

    public void LockInput()
    {
        _qMB.ToggleSelf(false);
        locked = true;
    }

    public void UnlockInput()
    {
        _qMB.ToggleSelf(true);
        locked = false;
        _lastInputTime = Time.time;
    }
}
