using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenLogic : MonoBehaviour
{
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private ClickableManager _cM;
    [SerializeField] private QuestionManager _qM;
    [SerializeField] private TouchHandler _tH;
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private List<ClickableHolder> _holder;
    [SerializeField] private Color _greyedOutColor;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _fakeSignatureImage;
    [SerializeField] private Image _blockEverythingElse;
    [SerializeField] private GameObject _currentQuestion;
    [SerializeField] private GameObject _progressionSystem;
    private Color _buttonImageColorSave;

    private List<Image> _slotImages = new List<Image>();
    private List<TMP_Text> _slotText = new List<TMP_Text>();

    [SerializeField] private Sprite _signature;
    [SerializeField] private Sprite _unterschrift;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _slots.Count; i++)
        {
            _slotImages.Add(_slots[i].GetComponentInChildren<Image>(true));
            _slotText.Add(_slots[i].GetComponentInChildren<TMP_Text>(true));
        }
        _buttonImageColorSave = _buttonImage.color;

        if (LocalizationManager.Language.Contains("German"))
        {
            _fakeSignatureImage.sprite = _unterschrift;
        }
        else
        {
            _fakeSignatureImage.sprite = _signature;
        }
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadEndScreen();
        }
    }

    public void LoadEndScreen()
    {
        _tH.UnlockInput(); 

        _currentQuestion.SetActive(false);
        _progressionSystem.SetActive(false);

        _qM.HideEndQuestion();

        _buttonImage.color = _buttonImageColorSave;
        _endScreen.SetActive(true);
        _endScreen.transform.localScale = Vector3.zero;
        for(int i = 0; i < _slots.Count; i++)
        {
            _slotImages[i].sprite = _holder[i].ProgressionImage;
            
            if (!ContainsHolder(_holder[i]))
            {
                _slotImages[i].color = _greyedOutColor;
                _slotText[i].text = "???";
            }
            else 
            {
                _slotImages[i].color = new Color(1,1,1,1);
                _slotText[i].text = _holder[i].Title;
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
        _tH.LockInput();

        Sequence seq = DOTween.Sequence();

        SlideColorStripe.DOAlpha(_fakeSignatureImage, 0, 0.75f, seq, 0);
        SlideColorStripe.DOAlpha(_buttonImage, 1, 1.25f, seq, 0.75f);
        seq.Insert(4f, _endScreen.transform.DOScale(Vector3.zero, 1.5f));
        Invoke("InvokeFirework", 2.25f);
        Invoke("InvokeFirework", 2.75f);
        Invoke("InvokeFirework", 3.25f);
        Invoke("InvokeFirework", 3.75f);
        seq.OnComplete(()=>{
            _endScreen.SetActive(false);
            InvokeFirework();
            Invoke("FadeOut", 0.5f);
        });
    }

    private void InvokeFirework()
    {
        GameObject temp = Instantiate(_qM._confetti, Camera.main.transform);
        temp.transform.localScale *= _qM._VC.Confetti_Size * 2;
        Destroy(temp, 4);

    }

    private void FadeOut()
    {
        _blockEverythingElse.gameObject.SetActive(true);
        SlideColorStripe.DOAlpha(_blockEverythingElse, 1, 0.75f);
        Invoke("InvokeLeaveScene", 0.75f);
    }

    private void InvokeLeaveScene()
    {
        SceneManager.LoadScene("Book_Main");
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
