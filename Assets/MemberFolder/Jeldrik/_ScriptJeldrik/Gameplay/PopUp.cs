using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject _reminderPrefab;

    private ClickableManager cM;
    private float _spawnTime;
    private Canvas _canvas;
    private TouchHandler _tH;
    private VALUECONTROLER _VC;
    private bool _spawnedReminder;
    private GameObject _reminderRef;



    void Start()
    {
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
        if (Time.time - _spawnTime > _VC.PopUp_BlockTime)
        {
            // TODO: Include proper touch controls
            if (Input.GetMouseButtonDown(0))
            {
                Close();
            }
        }

        if (Time.time - _spawnTime > _VC.PopUp_ReminderTime && !_spawnedReminder)
        {
            _reminderRef = Instantiate(_reminderPrefab, _canvas.transform);
            _reminderRef.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * (_VC.PopUp_Reminder_Pos * 0.01f), _reminderRef.transform.position.z);
            _reminderRef.GetComponent<ReminerPopUp>().Show();
            _spawnedReminder = true;
        }
    }

    // Closes the popup - gets called by UI button
    public void Close()
    {
        Debug.Log(_reminderRef);
        if (_reminderRef != null)
        {
            Destroy(_reminderRef);
        }
        // Preventing popup to spawn while the animation is running
        _spawnTime = Time.time;
        cM.HidePopUp();
    }
}
