using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SlideColorStripe : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private Image _button1;
    [SerializeField] private Image _button2;
    [SerializeField] private float _slideTime;
    [SerializeField] private Transform _text1;
    [SerializeField] private Transform _text2;
    [SerializeField] private Transform _questionImage;
    [SerializeField] private Transform _twig;
    [SerializeField] private Transform _child;

    [SerializeField] ProgressionSystem _progression;
    [SerializeField] QuestionManager _qM;

    [SerializeField] TMP_Text _text1Text; 
    [SerializeField] TMP_Text _text2Text;

    [SerializeField] CurrentQuestion _currentQuestion; 

    [SerializeField] public Question _q;

    private string _newText;
   

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

    public void CompleteSlide(Question q, float fast = 1f)
    {
        float tf = FadeAll();
        _q = q;
        Invoke("SlideIn", (_slideTime * tf + 0.2f) * fast);
    }

    public void Appear(bool cQToggle = true, float time = 0.5f)
    {
       ChangeAlpha(_text1Text, 0);
       ChangeAlpha(_text2Text, 0);
       ChangeAlpha(_image, 0);
       ChangeAlpha(_twig.GetComponent<Image>(), 0);
       _currentQuestion.gameObject.SetActive(cQToggle);
       DOAlpha(_text1Text, 1, time);
       DOAlpha(_text2Text, 1, time);
       DOAlpha(_image, 1, time);
       DOAlpha(_twig.GetComponent<Image>(), 1, time);
    }

    public void OrangeAppear(bool cQToggle = true)
    {
       ChangeAlpha(_text1Text, 0);
       ChangeAlpha(_text2Text, 0);
       ChangeAlpha(_image, 0);
       ChangeAlpha(_twig.GetComponent<Image>(), 0);
       _currentQuestion.gameObject.SetActive(cQToggle);
       DOAlpha(_twig.GetComponent<Image>(), 1, 0.75f);
    }

    public void Disappear(bool cQToggle = true)
    {
       ChangeAlpha(_text1Text, 1);
       ChangeAlpha(_text2Text, 1);
       ChangeAlpha(_image,     1);
       ChangeAlpha(_twig.GetComponent<Image>(), 1);
       DOAlpha(_text1Text, 0, 0.75f);
       DOAlpha(_text2Text, 0, 0.75f);
       DOAlpha(_image,     0, 0.75f);
       DOAlpha(_twig.GetComponent<Image>(), 0, 0.75f);
       _currentQuestion.gameObject.SetActive(cQToggle);
    }

    static public void ChangeAlpha(Image img, float a)
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, a);
        img.color = c;
    }
    static public void ChangeAlpha(TMP_Text tex, float a)
    {
        Color c = new Color(tex.color.r, tex.color.g, tex.color.b, a);
        tex.color = c;
    }

    static public void DOAlpha(TMP_Text tex, float a, float time)
    {
        Color c = new Color(tex.color.r, tex.color.g, tex.color.b, a);
        tex.DOColor(c, time);
    }
    static public void DOAlpha(Image img, float a, float time)
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, a);
        img.DOColor(c, time);
    }
    static public void DOAlpha(Image img, float a, float time, Sequence seq, float insertTime)
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, a);
        seq.Insert(insertTime,img.DOColor(c, time));
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
        FadeElement(_button1  , a, 0, seq, timefactor);
        FadeElement(_button2  , a, 0, seq, timefactor);
        seq.OnComplete(()=>{
            PositionAllObjects(-241);
            _currentQuestion._sliding = false;
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
        // Also slide progression Menu
        //_progression.ShowOrder(_progression._orders[_qM.GetCurrentQuestionId()], true);

        Sequence seq = DOTween.Sequence();

        //_moving = true;

        // Move Elements
        SlideElement(transform,distance, seq);
        SlideElement(_text1,distance, seq);
        SlideElement(_text2,distance, seq);
        SlideElement(_questionImage,distance, seq);
        SlideElement(_button1.transform,distance, seq);
        SlideElement(_button2.transform,distance, seq);

        // Fade Text
        float a = distance < 0 ? 0f : 1f;
        if (distance > 0)
        {
            
            FadeElement(_text1Text, a, _slideTime * 0.65f, seq);
            FadeElement(_text2Text, a, _slideTime * 0.65f, seq);
            FadeElement(_button1, a, _slideTime * 0.65f, seq);
            FadeElement(_button2, a, _slideTime * 0.65f, seq);

            Invoke("UpdateInfo", _slideTime * 0.2f);
            /*seq.OnComplete(() => {
                _currentQuestion.ChangeCurrentQuestionInternal(_q);
            });*/
        }
        else 
        {
            FadeElement(_text1Text, a, 0, seq);
            FadeElement(_text2Text, a, 0, seq);
            FadeElement(_button1, a, 0, seq);
            FadeElement(_button2, a, 0, seq);
        }    

        seq.OnComplete(() => {
            //_currentQuestion._sliding = false;
        });
        
    }

    private void PositionAllObjects(float distance)
    {
        PositionObject(_text1, distance);
        PositionObject(_text2, distance);
        PositionObject(transform, distance);
        PositionObject(_button1.transform, distance);
        PositionObject(_button2.transform, distance);
        PositionObject(_questionImage.transform, distance);

    }

    private void PositionObject(Transform element, float distance)
    {
        float goal = element.localPosition.x - distance;
        element.localPosition = new Vector3(goal, element.localPosition.y, element.localPosition.z);   
    }

    private void UpdateInfo()
    {
        _currentQuestion.ChangeCurrentQuestionInternal(_q, false);
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
