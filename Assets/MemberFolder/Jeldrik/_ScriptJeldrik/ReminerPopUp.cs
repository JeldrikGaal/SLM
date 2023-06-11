using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class ReminerPopUp : MonoBehaviour
{
    #region References
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _content;
    [SerializeField] ReminderPopUpHolder _holder;
    [SerializeField] GameObject _container;
    #endregion

    // Lifetime related variables
    private float _startLifeTime;
    private bool _started;

    // Start is called before the first frame update
    void Start()
    {
        _title.text = _holder.Title;
        _content.text = _holder.Content;
       
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.DOShakeRotation(0.5f, 5);
        transform.DOShakePosition(0.5f, 10);
        transform.DOShakeScale(0.5f, 0.05f);
    }

    // Disables container and destroys the reminder
    public void End()
    {
        _container.SetActive(false);
        Destroy(gameObject);
    }
}
