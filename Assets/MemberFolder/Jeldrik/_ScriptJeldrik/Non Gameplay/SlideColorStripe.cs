using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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

    [SerializeField] TMP_Text _text1Text; 
    [SerializeField] TMP_Text _text2Text;

    [SerializeField] CurrentQuestion _currentQuestion; 

    private string _newText;
    private Question _q;

    public int index;

    List<Ease> l = new List<Ease>();

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
        _text1Text = _text1.GetComponent<TMP_Text>();
        _text2Text = _text2.GetComponent<TMP_Text>();
        _currentQuestion = GameObject.FindGameObjectWithTag("CurrentQuestion").GetComponent<CurrentQuestion>();

        l.Add(Ease.Linear);
        l.Add(Ease.InBack);
        l.Add(Ease.InCirc);
        l.Add(Ease.InCubic);
        l.Add(Ease.InExpo);
        l.Add(Ease.InFlash);
        l.Add(Ease.OutBack);
        l.Add(Ease.OutCirc);
        l.Add(Ease.OutCubic);
        l.Add(Ease.OutExpo);
        l.Add(Ease.OutFlash);
        l.Add(Ease.InOutBack);
        l.Add(Ease.InOutCirc);
        l.Add(Ease.InOutCubic);
        l.Add(Ease.InOutExpo);
        l.Add(Ease.InOutFlash);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SlideIn();  
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SlideOut();  
        }
    }

    public void CompleteSlide(Question q)
    {
        float tf = FadeAll();
        _q = q;
        Invoke("SlideIn", _slideTime * tf + 0.2f);
    }

    public void SlideIn()
    {
        Slide(241);
    }

    public void SlideOut()
    {
        Slide(-241);
    }

    public float FadeAll()
    {
        float a = 0;
        float timefactor = 1f;
        float waitTime = 0.2f;
        Sequence seq = DOTween.Sequence();
        FadeElement(_text1Text, a, 0, seq, timefactor);
        FadeElement(_text2Text, a, 0, seq, timefactor);
        FadeElement(_image    , a, 0, seq, timefactor);
        seq.OnComplete(()=>{
            PositionAllObjects(-241);
            Invoke("ResetStrokeAlpha", waitTime);
        });
        return timefactor;
    }

    private void ResetStrokeAlpha()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }

    public void Slide(float distance)
    {
        
        Sequence seq = DOTween.Sequence();

        // Move Elements
        SlideElement(transform,distance, seq);
        SlideElement(_text1,distance, seq);
        SlideElement(_text2,distance, seq);
        SlideElement(_questionImage,distance, seq);

        // Fade Text
        float a = distance < 0 ? 0f : 1f;
        if (distance > 0)
        {
            
            FadeElement(_text1Text, a, _slideTime * 0.65f, seq);
            FadeElement(_text2Text, a, _slideTime * 0.65f, seq);

            Invoke("UpdateInfo", _slideTime * 0.2f);
            /*seq.OnComplete(() => {
                _currentQuestion.ChangeCurrentQuestionInternal(_q);
            });*/
        }
        else 
        {
            FadeElement(_text1Text, a, 0, seq);
            FadeElement(_text2Text, a, 0, seq);
        }    

        
    }

    private void PositionAllObjects(float distance)
    {
        PositionObject(_text1, distance);
        PositionObject(_text2, distance);
        PositionObject(transform, distance);
    }

    private void PositionObject(Transform element, float distance)
    {
        float goal = element.localPosition.x - distance;
        element.localPosition = new Vector3(goal, element.localPosition.y, element.localPosition.z);   
    }

    private void UpdateInfo()
    {
        _currentQuestion.ChangeCurrentQuestionInternal(_q);
    }

    
    
    

    public void FadeElement(TMP_Text element, float a, float delay, Sequence s, float time = 0.35f)
    {
        Color c = new Color(element.color.r, element.color.g, element.color.b, a);
        s.Insert(delay, DOTween.To(() => element.color, x => element.color = x, c, _slideTime * time)).SetEase(Ease.OutCubic);
    }
    public void FadeElement(Image element, float a, float delay, Sequence s, float time = 0.35f)
    {
        Color c = new Color(element.color.r, element.color.g, element.color.b, a);
        s.Insert(delay, DOTween.To(() => element.color, x => element.color = x, c, _slideTime * time)).SetEase(Ease.OutCubic);
    }

    public void SlideElement(Transform element, float distance, Sequence s)
    {
        float goal = element.localPosition.x - distance;
        s.Insert(0, element.DOLocalMoveX(goal, _slideTime).SetEase(Ease.InOutSine));
    }

    
}
