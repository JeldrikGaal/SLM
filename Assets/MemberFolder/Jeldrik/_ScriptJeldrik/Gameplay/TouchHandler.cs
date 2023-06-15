using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private VALUECONTROLER _VC;
    [HideInInspector]
    public float _width, _height, _aspect;

    #region Serialized Fields
    [Tooltip("Factor with how fast the camera moves when being dragged around")]
    private float _dragFactor = 0.1f;

    [Tooltip("How long to wait for next input to deploy a reminder")]
     private float _waitTimeReminder;

    [Tooltip("How long to after Spawning a reminder for the next one")]
    private float _waitTimeReminderWaitTime;

    [Tooltip("Prefab for the input reminder Object")]
    [SerializeField] private GameObject _inputReminderObject;

    [Tooltip("Prefab for the wrong input particle")]
    [SerializeField] private GameObject _wrongInputParticle;
    #endregion

    private float _lastInputTime;
    private float _lastBlockPartTime;

    // Reference to the button toggling the question menu
    private QuestionMenuButton _qMB;
    private CurrentQuestion _cQ;

    private bool locked;
    

    // Set true by buttons if in this frame a button was clicked to prevent spawning of wrong input particle
    [HideInInspector] public bool ValidInput;
    private bool _lastFrameClicked;
    private float _lastFrameClickedStartTime;
    private float _lastFrameClickedCountTime = 0.02f;

    // Camera stuff
    [HideInInspector] public Vector2 _camLimits;
    private Vector3 mpS;
    private Vector3 camPS;
    private GameObject _canvas;
    private Transform _camTransform;

    // Start is called before the first frame update
    void Awake()
    {
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _dragFactor = _VC.Camera_MoveSpeed;
        _waitTimeReminder = _VC.Misc_WaitTime_InputReminder;
        _waitTimeReminderWaitTime = _VC.Misc_SecondWaitTime_InputReminder;
        _inputReminderObject = _VC.Misc_Reminder_Object;

        _width = (float)Screen.width / 2.0f;
        _height = (float)Screen.height / 2.0f;

        // Used for timing the spawning of 'Wrong Input' particle
        _lastFrameClickedCountTime = 0.3f;

        // Limits where the camera can be moved to by dragging
        _aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        _camLimits = new Vector2(_width - Camera.main.orthographicSize  * _aspect, _height - Camera.main.orthographicSize);
        _camLimits *= 1 + _VC.Camera_Border;

        // Locks all input until its unlocked by UnlockInput()
        locked = true;

        // Used to time the spawning of reminder notifications
        _lastInputTime = Time.time;

        // Fetching needed references 
        _camTransform = Camera.main.transform;
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        //_qMB = GameObject.FindGameObjectWithTag("QuestionMenuButton").GetComponent<QuestionMenuButton>();
        //_qMB.ToggleSelf(false);
        _cQ = GameObject.FindGameObjectWithTag("CurrentQuestion").GetComponent<CurrentQuestion>();
        _cQ.ToggleSelf(false);
    }

    void Update()
    {
        // Prevents any input when the handler is locked
        if (locked) return;

        #region Commented Touch controls
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
        #endregion

        #region Wrong Input particle Logic
        // Checking if last frame the user started a click and if they are now dragging or not
        if (_lastFrameClicked && !Input.GetMouseButton(0))
        {
            
            if (!ValidInput)
            {
                SpawnWrongInputPart(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            
        }

        // Resetting variable that checks if a button has been clicked after x sec
        if (Time.time - _lastBlockPartTime > 0.1f && ValidInput)
        {
            ValidInput = false;
        }
        #endregion

        #region Handling Input
        // Starting to drag the camera around ( with mouse controls for debugging purposes )
        if (Input.GetMouseButtonDown(0))
        {
            mpS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camPS = Camera.main.transform.position;

            LogInputTime();
            _lastFrameClicked = true;
            _lastFrameClickedStartTime = Time.time;


        }
        else
        {
            if (Time.time - _lastFrameClickedStartTime > _lastFrameClickedCountTime && _lastFrameClicked)
            {
                _lastFrameClicked = false;
            }
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
        #endregion

        ReminderLogic();
    }

    #region Reminder Notification Functions
    // Logic for spawning reminder notifcations when user has not pressed anything for _waitTimeReminder seconds 
    private void ReminderLogic()
    {
        if (Time.time - _lastInputTime > _waitTimeReminder)
        {
            SpawnInputReminder();
            _lastInputTime = Time.time + _waitTimeReminderWaitTime;
        }
    }

    // Spawn reminder notifcation Objects
    private void SpawnInputReminder()
    {
        GameObject temp = Instantiate(_inputReminderObject, _canvas.transform);
        temp.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * _VC.PopUp_Reminder_Pos, temp.transform.position.z);
        temp.GetComponent<ReminerPopUp>().Show();
    }
    #endregion

    #region wrong input functions
    // Spawning the 'red ripple' particle at pos
    public void SpawnWrongInputPart(Vector2 pos)
    {
        GameObject temp = Instantiate(_wrongInputParticle, Camera.main.transform);
        temp.transform.position = new Vector3(pos.x, pos.y, temp.transform.position.z);
        Destroy(temp, 2f);
        _lastFrameClicked = false;
    }

    // Called by buttons to block the spawning of wrong input particles 
    public void BlockWrongInputPart()
    {
        ValidInput = true;
        _lastBlockPartTime = Time.time;
    }
    #endregion

    #region Input helper
    private void LogInputTime()
    {
        _lastInputTime = Time.time;
    }

    public void LockInput(bool qmb = false)
    {
        // Only toggle question manager button if needed
        if (qmb )
        {
            //_qMB.ToggleSelf(false);
            _cQ.ToggleSelf(false);
        }
        locked = true;
    }

    public void UnlockInput()
    {
        //_qMB.ToggleSelf(true);
        _cQ.ToggleSelf(true);
        locked = false;
        _lastInputTime = Time.time;
    }
    #endregion
}
