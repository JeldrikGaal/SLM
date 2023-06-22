using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class SlideColorStripe : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private float _slideTime;
    [SerializeField] private Transform _text1;
    [SerializeField] private Transform _text2;
    [SerializeField] private Transform _questionImage;
    [SerializeField] private Transform _twig;
    [SerializeField] private Transform _child;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SlideIn2();  
        }
    }

    public void SlideIn2()
    {
        transform.localPosition = new Vector3 (transform.localPosition.x + 241, transform.localPosition.y, transform.localPosition.z);
        _child.localPosition = new Vector3 (_child.localPosition.x - 241, _child.localPosition.y, _child.localPosition.z);
        float goal = transform.localPosition.x - 241;
        float goal2 = transform.localPosition.x + 241;
        transform.DOLocalMoveX(goal, _slideTime).SetEase(Ease.InSine);
        _child.DOLocalMoveX(goal2, _slideTime).SetEase(Ease.InSine);
    }

    public void SlideIn()
    {
        transform.localPosition = new Vector3 (transform.localPosition.x + 241, transform.localPosition.y, transform.localPosition.z);
        _text1.localPosition = new Vector3 (_text1.localPosition.x + 241, _text1.localPosition.y, _text1.localPosition.z);
        _text2.localPosition = new Vector3 (_text2.localPosition.x + 241, _text2.localPosition.y, _text2.localPosition.z);
        _questionImage.localPosition = new Vector3 (_questionImage.localPosition.x + 241, _questionImage.localPosition.y, _questionImage.localPosition.z);
        //_twig.localPosition = new Vector3 (_twig.localPosition.x + 241, _twig.localPosition.y, _twig.localPosition.z);
        float goal = transform.localPosition.x - 241;
        float goal2 = _text1.localPosition.x - 241;
        float goal3 = _text2.localPosition.x - 241;
        float goal4 = _questionImage.localPosition.x - 241;
        float goal5 = _twig.localPosition.x - 241;
        transform.DOLocalMoveX(goal, _slideTime);
        _text1.DOLocalMoveX(goal2, _slideTime);
        _text2.DOLocalMoveX(goal3, _slideTime);
        _questionImage.DOLocalMoveX(goal4, _slideTime);
        //_twig.DOLocalMoveX(goal5, _slideTime);
    }
}
