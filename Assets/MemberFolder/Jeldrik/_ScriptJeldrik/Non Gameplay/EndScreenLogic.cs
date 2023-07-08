using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenLogic : MonoBehaviour
{
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private ClickableManager _cM;
    [SerializeField] private QuestionManager _qM;
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private List<ClickableHolder> _holder;
    [SerializeField] private Color _greyedOutColor;
    [SerializeField] private Image _buttonImage;
    private Color _buttonImageColorSave;

    private List<Image> _slotImages = new List<Image>();
    private List<TMP_Text> _slotText = new List<TMP_Text>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _slots.Count; i++)
        {
            _slotImages.Add(_slots[i].GetComponentInChildren<Image>(true));
            _slotText.Add(_slots[i].GetComponentInChildren<TMP_Text>(true));
        }
        _buttonImageColorSave = _buttonImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadEndScreen();
        }
    }

    void LoadEndScreen()
    {
        _buttonImage.color =  _buttonImageColorSave;
        _endScreen.SetActive(true);
        _endScreen.transform.localScale = Vector3.zero;
        for(int i = 0; i < _slots.Count; i++)
        {
            _slotImages[i].sprite = _holder[i].ProgressionImage;
            _slotText[i].text = _holder[i].Title;
            if (!ContainsHolder(_holder[i]))
            {
                _slotImages[i].color = _greyedOutColor;
            }
            else 
            {
                _slotImages[i].color = new Color(1,1,1,1);
            }
        }
        _endScreen.transform.DOScale(Vector3.one, 0.75f).OnComplete(() => {
        for(int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetActive(true);
        }
        });
       
        

    }

    public void CloseEndScreen()
    {
        Sequence seq = DOTween.Sequence();

        SlideColorStripe.DOAlpha(_buttonImage, 1, 0.75f, seq, 0);
        seq.Insert(1.25f, _endScreen.transform.DOScale(Vector3.zero, 0.75f));
        seq.OnComplete(()=>{
            _endScreen.SetActive(false);
        });
    }

    private bool ContainsHolder(ClickableHolder cH)
    {
        foreach(List<ClickableHolder> l in _qM._alreadyClickedComb)
        {
            if (l.Contains(cH)) return true;
        }
        return false;
    }
}
