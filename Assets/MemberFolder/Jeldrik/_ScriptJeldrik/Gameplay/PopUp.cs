using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    #region References
    [SerializeField] private GameObject _reminderPrefab;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;

    public bool MainPopUp;

    private Canvas _canvas;
    private TouchHandler _tH;
    private VALUECONTROLER _VC;
    private ClickableManager cM;
    private GameObject _reminderRef;
    #endregion
    private float _spawnTime;
    private bool _spawnedReminder;
    



    void Start()
    {
        // Fetching reference to all needed manager
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        _tH = Camera.main.GetComponent<TouchHandler>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();

    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
        _spawnedReminder = false;
    }

    private void Update()
    {
        // Waiting until more than _VC.PopUp_BlockTime seconds have passed to allow closing the popup
        if (Time.time - _spawnTime > _VC.PopUp_BlockTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Close();
            }
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Close();
                }
            }
        }

        // Ensuring the object is in the middle of the screen
        if (MainPopUp) transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

        // Spawning a reminder prefab after the waiting time set in VC
        if (Time.time - _spawnTime > _VC.PopUp_ReminderTime && !_spawnedReminder)
        {
            _reminderRef = Instantiate(_reminderPrefab, _canvas.transform);
            _reminderRef.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * (_VC.PopUp_Reminder_Pos * 0.01f), _reminderRef.transform.position.z);
            _reminderRef.GetComponent<ReminerPopUp>().Show();
            _spawnedReminder = true;
        }
    }

    public void UpdateText(ClickableHolder cH)
    {
        if (!cM) cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        if (!_VC) _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        // Setting all the info in the popup
        _titleText.text = cH.Title;
        _descriptionText.text = cH.Description;
       
        if (cH.Question)
        {
            _image.sprite = cH.Image;
            _image.preserveAspect = true;
        }
       
    }

    // Closes the popup - gets called by UI button
    public void Close()
    {
        if (_reminderRef != null)
        {
            Destroy(_reminderRef);
        }
        // Preventing popup to spawn while the animation is running
        _spawnTime = Time.time;
        
        Debug.Log("CBT");
        cM.HidePopUp(this);
    }
}
        