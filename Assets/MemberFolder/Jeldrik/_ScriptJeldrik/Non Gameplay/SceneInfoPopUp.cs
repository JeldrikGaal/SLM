using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Assets.SimpleLocalization;
using TMPro;
using Lean.Touch;

public class SceneInfoPopUp : MonoBehaviour
{
    #region References
    private VALUECONTROLER _VC;
    private TouchHandler _tH;
    private GameObject _canvas;
    private GameObject _reminderRef;    
    private LeanDragCamera _dragController;
    [SerializeField] private GameObject _currentQuestion;
    [SerializeField] private TMP_Text _text;
    [SerializeField] GameObject _reminderPrefab;

    [SerializeField] private SlideColorStripe _colorStripe;

    [SerializeField] private BookButtonLogic _bookButton;
    #endregion 

    private float _spawnTime;
    private bool _spawnedReminder;

    // Start is called before the first frame update
    void OnEnable()
    {
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();

        // Getting references
        _tH = Camera.main.GetComponent<TouchHandler>();
        _dragController = Camera.main.GetComponent<LeanDragCamera>();
       

        // _colorStripe = GameObject.FindGameObjectWithTag("ColorStripe").GetComponent<SlideColorStripe>();

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
        // once the block time has run out close the popup after any input has been made
        if (Time.time - _spawnTime > _VC.SceneInfo_BlockTime)
        {
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

        // Spawn and properly position reminder popup
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

    // Destroys the popup and unlocks the other input in the touch handler
    public void EndPopUp()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => 
        {
            
            // TODO: Trigger book button spawning in animation
            _bookButton.SpawnAnimation();

            Destroy(_reminderRef);
            Destroy(this.gameObject);            
        });
        
    }
}
