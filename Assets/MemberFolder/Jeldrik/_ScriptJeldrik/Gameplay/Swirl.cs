using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swirl : MonoBehaviour
{
    [Tooltip("Time it takes the swirl to fully form")]
     private float _swirlTime;
    private Image _image;
    private VALUECONTROLER _VC;

    void Start()
    {
        _image = GetComponent<Image>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowSwirl();
        }
    }

    public void ShowSwirl()
    {
        _image.fillAmount = 0;
        DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, 1f, _VC.Swirl_Time).SetEase(Ease.InSine);
    }
}
