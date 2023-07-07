using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlowingSymbol : MonoBehaviour
{
    private Image _image;

    private VALUECONTROLER _VC;
    private float _startTime;



    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GlowEffect(0.4f);
        }

        if (_VC.Do_Glow_Effect)
        {
            if (Time.time - _startTime > _VC.RepetetionTime)
            {
                GlowEffect(_VC.GlowTime);
                _startTime = Time.time + Random.Range(0, 0.5f);
            }
        }        
    }

    public void GlowEffect(float time)
    {
        Color c = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
        Color c2 = _VC.Glow_Color;
        Vector3 s = transform.localScale;
        Vector3 s2 = transform.localScale * 1.5f;
        _image.color = c;
        _image.DOColor(c2, time).OnComplete(() => {
             _image.DOColor(c, time);
        });
        /*image.transform.DOScale(s2, time).OnComplete(() => {
            _image.transform.DOScale(s, time);
        });*/
    }
}
