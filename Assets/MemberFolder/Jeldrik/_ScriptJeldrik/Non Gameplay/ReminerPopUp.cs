using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Assets.SimpleLocalization;

public class ReminerPopUp : MonoBehaviour
{
    #region References
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _content;
    [SerializeField] ReminderPopUpHolder _holder;
    [SerializeField] GameObject _container;

    FontManager _fontManager;
    #endregion

    // Lifetime related variables
    private float _startLifeTime;
    private bool _started;

    // Start is called before the first frame update
    void Start()
    {
        _title.text = _holder.Title;
        _content.text = LocalizationManager.Localize(_holder.LocalizationKey);
        
        // Set correct font 
        _fontManager = GameObject.FindWithTag("FontManager").GetComponent<FontManager>();
        _title.font = _fontManager.GetFont();
        _content.font = _fontManager.GetFont();
       
    }

    void Update()
    {
        // Calling end function once the lifetime runs out
        if (_started && Time.time - _startLifeTime > _holder.LifeTime)
        {
            End();
        }
    }

    // Show the reminder pop up with some small animations
    public void Show()
    {
        // Initializing 
        _container.SetActive(true);
        _startLifeTime = Time.time;
        _started = true;

        // Animation
        transform.DOShakeRotation(0.5f, new Vector3(0, 0, 5));
        transform.DOShakePosition(0.5f, new Vector3(10, 10, 0));
        transform.DOShakeScale(0.5f, new Vector3(0.01f, 0.01f, 0));
    }

    // Disables container and destroys the reminder
    public void End()
    {
        _container.SetActive(false);
        Destroy(gameObject);
    }
}
