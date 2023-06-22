using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScribbleEffect : MonoBehaviour
{

    [SerializeField] Color _blinkColor;
    [SerializeField] float _blinkTime;
    Color _originalColor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            MakeBlink(GetComponent<Image>());
        }
    }

    public void MakeBlink(Image _image)
    {
        _originalColor = _image.color;
        DOTween.To(() => _image.color, x => _image.color = x, _blinkColor, _blinkTime * 0.5f).SetEase(Ease.InSine).OnComplete(() => {
            DOTween.To(() => _image.color, x => _image.color = x, _originalColor, _blinkTime * 0.5f).SetEase(Ease.InSine);
        });
        
    }
}
