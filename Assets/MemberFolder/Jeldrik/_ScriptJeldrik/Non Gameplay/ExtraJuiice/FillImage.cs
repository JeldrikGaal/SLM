using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FillImage : MonoBehaviour
{
    [Tooltip("Time it takes the the image to fill")]
    [SerializeField] private float _fillTime;
    private Image _image;
    private VALUECONTROLER _VC;


    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        //_VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
    }

    // Update is called once per frame
    void Update()
    {
        
      
    }

    public void Fill()
    {
        _image.fillAmount = 0;
        DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, 1f, _fillTime).SetEase(Ease.InSine);
    }
}
