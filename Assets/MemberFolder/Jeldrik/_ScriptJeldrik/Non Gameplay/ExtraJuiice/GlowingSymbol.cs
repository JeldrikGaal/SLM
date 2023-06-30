using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlowingSymbol : MonoBehaviour
{
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GlowEffect(0.4f);
        }
        
    }

    public void GlowEffect(float time)
    {
        Color c = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
        Color c2 = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        Vector3 s = transform.localScale;
        Vector3 s2 = transform.localScale * 2.5f;
        _image.color = c;
        _image.DOColor(c2, time).OnComplete(() => {
             _image.DOColor(c, time);
        });
        _image.transform.DOScale(s2, time).OnComplete(() => {
             _image.transform.DOScale(s, time);
        });
    }
}
