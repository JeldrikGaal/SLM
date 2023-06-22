using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShakeOnClick : MonoBehaviour
{
    private Image _image;
    [SerializeField] private float _shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        _image.transform.DORotate(new Vector3(0, 0, 5), _shakeTime).SetEase(Ease.InSine).OnComplete(() =>
        {
            _image.transform.DORotate(new Vector3(0, 0, -5), _shakeTime).SetEase(Ease.InSine).OnComplete(() => {
                _image.transform.DORotate(new Vector3(0, 0, 0), _shakeTime * 0.5f).SetEase(Ease.InSine);
            });
        });
    }
}
