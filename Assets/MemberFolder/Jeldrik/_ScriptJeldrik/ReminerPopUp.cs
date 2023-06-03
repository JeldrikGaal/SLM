using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReminerPopUp : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _content;
    [SerializeField] ReminderPopUpHolder _holder;
    [SerializeField] GameObject _container;

    private float _lifeTime;
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

    public void Show()
    {
        _container.SetActive(true);
        _startLifeTime = Time.time;
        _started = true;
    }

    public void End()
    {
        _container.SetActive(false);
        Destroy(gameObject);
    }
}
