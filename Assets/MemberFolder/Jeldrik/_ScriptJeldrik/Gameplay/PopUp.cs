using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private float _blockTime;
    [SerializeField] private float _reminderTime;
    [SerializeField] private GameObject _reminderPrefab;

    private ClickableManager cM;
    private float _spawnTime;
    private Canvas _canvas;
    private TouchHandler _tH;
    private bool _spawnedReminder;
    private GameObject _reminderRef;



    void Start()
    {
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        _tH = Camera.main.GetComponent<TouchHandler>();
        
    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
        _spawnedReminder = false;
    }

    private void Update()
    {
        if (Time.time - _spawnTime > _blockTime)
        {
            // TODO: Include proper touch controls
            if (Input.GetMouseButtonDown(0))
            {
                Close();
            }
        }

        if (Time.time - _spawnTime > _reminderTime && !_spawnedReminder)
        {
            _reminderRef = Instantiate(_reminderPrefab, _canvas.transform);
            _reminderRef.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 0.85f, _reminderRef.transform.position.z);
            _reminderRef.GetComponent<ReminerPopUp>().Show();
            _spawnedReminder = true;
        }
    }

    // Closes the popup - gets called by UI button
    public void Close()
    {
        if (_reminderRef != null)
        {
            Destroy(_reminderRef);
        }
        cM.HidePopUp();
    }
}
