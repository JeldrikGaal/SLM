using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private SlideColorStripe _colorStripe;
    #endregion

    private float _lastInputTime;
    private float _lastBlockPartTime;

    // Reference to the button toggling the question menu
    private QuestionMenuButton _qMB;
    private CurrentQuestion _cQ;

    // If true updates will be suppressed
    private bool locked;
    

    // Set true by buttons if in this frame a button was clicked to prevent spawning of wrong input particle
    [HideInInspector] public bool ValidInput;
    private bool _lastFrameClicked;
    private float _lastFrameClickedStartTime;
    private float _lastFrameClickedCountTime = 0.02f;

    // Camera stuff
    public Vector4 _camLimits;
    private Vector3 mpS;
    private Vector3 camPS;
    private GameObject _canvas;
    private Transform _camTransform;

    // Needed to send raycast that also hit ui elements to spawn wrong input particles
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // used to toggle on and off functions that should only be called in editor
    private bool EDITOR;

    // Start is called before the first frame update
    void Awake()
    {
        

        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _dragFactor = _VC.Camera_MoveSpeed;
        _waitTimeReminder = _VC.Misc_WaitTime_InputReminder;
        _waitTimeReminderWaitTime = _VC.Misc_SecondWaitTime_InputReminder;
        _inputReminderObject = _VC.Misc_Reminder_Object;

         // Fetching needed references 
        _camTransform = Camera.main.transform;
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        m_Raycaster = _canvas.GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();

        _width = (float)Screen.width / 2.0f;
        _width = 1920 / 2f;
        _height = (float)Screen.height / 2.0f;
        _height = 1080 / 2f;

        // Used for timing the spawning of 'Wrong Input' particle
        _lastFrameClickedCountTime = 0.3f;

        // Limits where the camera can be moved to by dragging
        _aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        
        _camLimits = new Vector2(_width - Camera.main.orthographicSize  * _aspect, _height - Camera.main.orthographicSize);
        _camLimits = new Vector4(- _camLimits.x + _canvas.transform.position.x, _camLimits.x + _canvas.transform.position.x,
                                - _camLimits.y + _canvas.transform.position.y, _camLimits.y + _canvas.transform.position.y);
        //_camLimits = new Vector2();
        _camLimits *= 1 + _VC.Camera_Border;

        // Locks all input until its unlocked by UnlockInput()
        locked = true;

        // Used to time the spawning of reminder notifications
        _lastInputTime = Time.time;

       
        //_qMB = GameObject.FindGameObjectWithTag("QuestionMenuButton").GetComponent<QuestionMenuButton>();
        //_qMB.ToggleSelf(false);
        _cQ = GameObject.FindGameObjectWithTag("CurrentQuestion").GetComponent<CurrentQuestion>();

        EDITOR = false;

        _cQ.ToggleSelf(false);
    }

    void Update()
    {
        // Prevents any input when the handler is locked
        if (locked) return;

        #region Wrong Input particle Logic

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
                //Set up the new Pointer Event
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the game object
                m_PointerEventData.position = Input.mousePosition;
                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();
                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);
                if (results[0].gameObject.CompareTag("BackGround")){
                    Vector3 partPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.5f);
                    SpawnWrongInputPart(partPos);
                }
        }
        #endregion

        #region Handling Input
        if (EDITOR)
        {
            // Starting to drag the camera around ( with mouse controls for debugging purposes )
            if (Input.GetMouseButtonDown(0))
            {
                StartScrolling();
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
                Scrolling();
            } 
        }
        #endregion

        ReminderLogic();
    }

    // Legacy functions -> replaced by leantouch package
    private void StartScrolling()
    {
            mpS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camPS = Camera.main.transform.position;

            LogInputTime();
            _lastFrameClicked = true;
            _lastFrameClickedStartTime = Time.time;
    }

    // Legacy functions -> replaced by leantouch package
    private void Scrolling()
    {
        Vector3 dif = mpS - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = camPS + ( dif  * _dragFactor);
            Camera.main.transform.position = new Vector3(
                Mathf.Max(-_camLimits.x, Mathf.Min(_camLimits.x, newPos.x)),
                Mathf.Max(-_camLimits.y, Mathf.Min(_camLimits.y, newPos.y)),
                 Camera.main.transform.position.z);

            LogInputTime();
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
    public void SpawnWrongInputPart(Vector3 pos)
    {
        return;
        /*GameObject temp = Instantiate(_wrongInputParticle, Camera.main.transform);
        temp.transform.position = new Vector3(pos.x, pos.y, pos.z);
        temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, temp.transform.localPosition.y, 0.5f);
        Destroy(temp, 2f);
        _lastFrameClicked = false;*/
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
        _colorStripe.Appear();
        locked = false;
        _lastInputTime = Time.time;
    }
    #endregion
}
