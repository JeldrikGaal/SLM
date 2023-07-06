using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionSystem : MonoBehaviour
{

    [SerializeField] private ClickableManager _cM;
    [SerializeField] private QuestionManager _qM;
    [SerializeField] private CurrentQuestion _cQ;
    [SerializeField] private ClickableStorage _storage;
    [SerializeField] private VALUECONTROLER _VC;
    [SerializeField] private List<GameObject> _foundSlots = new List<GameObject>();
    [SerializeField] private List<GameObject> _toBeFoundSlots = new List<GameObject>();
    private List<Image> _foundSlotsImages = new List<Image>();
    private List<Image> _toBeFoundSlotsImages = new List<Image>();
    [SerializeField] private List<ClickableHolder> _found;
    [SerializeField] private List<ClickableHolder> _toBeFound;
    [SerializeField] private ClickableHolder _emptyHolder;
    private List<ClickableHolder> _q1Order = new List<ClickableHolder>();
    private List<ClickableHolder> _q2Order = new List<ClickableHolder>();
    private List<ClickableHolder> _q3Order = new List<ClickableHolder>();
    public List<List<ClickableHolder>> _orders = new List<List<ClickableHolder>>();
    private List<ClickableHolder> _orderSaver = new List<ClickableHolder>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject g in _foundSlots)
        {
            _foundSlotsImages.Add(g.transform.GetChild(0).GetComponent<Image>());
        }

        foreach (GameObject g in _toBeFoundSlots)
        {
            _toBeFoundSlotsImages.Add(g.transform.GetChild(0).GetComponent<Image>());
        }

        // Check if the player has already played the game
        if  (_storage._alreadyClickedSave.Count > 0)
        {
            // TODO: if player is returning display the menu from that state
        }
        else
        {
            // First time, initalize the list from scratch
            InitOrders();
            ShowOrder(_q1Order);
        }
        _orders.Add(_q1Order);
        _orders.Add(_q2Order);
        _orders.Add(_q3Order);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowOrder(_q1Order);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowOrder(_q2Order);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowOrder(_q3Order);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleBar(_foundSlots, _found, true);
            ToggleBar(_toBeFoundSlots, _toBeFound, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToggleBar(_foundSlots, _found, false);
            ToggleBar(_toBeFoundSlots, _toBeFound, false);
        }
    }

    private void InitOrders()
    {


        // Add Johan to the list
        _q1Order.Add(_VC.Questions[0].ObjectsToFind1[0]);
        // Insert empty holders
        for (int i = 0; i < 8; i++)
        {
            _q1Order.Add(_emptyHolder);
        }
        
        // Get the holder objects 2
        _q2Order.AddRange(_VC.Questions[1].ObjectsToFind1);
        // Shuffle the list 2
        _q2Order = _q2Order.OrderBy(i => Guid.NewGuid()).ToList();
        // Insert empty holders
        for (int i = 0; i < 4; i++)
        {
            _q2Order.Add(_emptyHolder);
        }

        // Get the holder objects
        _q3Order.AddRange(_VC.Questions[2].ObjectsToFind1);
        // Shuffle the list
        _q3Order = _q3Order.OrderBy(i => Guid.NewGuid()).ToList();
    }

    public void CallFindObjectProgression(int questionNumber, ClickableHolder cH)
    {
        _toBeFound.Remove(cH);
        _toBeFound.Add(_emptyHolder);
        _found[GetFreeSpot(_found)] = cH;
        UpdateDisplay();
    }


    // Returns if one of the first 2 spots is free otherwise returns -1
    private int GetFreeSpot(List<ClickableHolder> order)
    {
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] == _emptyHolder)
            {
                return i;
            }
        }
        return -1;
    }

    public void ShowOrder(List<ClickableHolder> order)
    {
        _found = new List<ClickableHolder>();
        //EnableUntil(amount);
        _toBeFound = order;
        for (int i = 0; i < _toBeFound.Count; i++)
        {
            if (_toBeFound[i] != _emptyHolder)
            {
                _toBeFoundSlotsImages[i].enabled = true;
                _toBeFoundSlotsImages[i].sprite = _toBeFound[i].ProgressionImage;
            }
            _found.Add(_emptyHolder);
        }
        UpdateDisplay();
    }


    private void UpdateRow(List<ClickableHolder> clickableList, List<GameObject> slotList, List<Image> imagesList)
    {
        for (int i = 0; i < clickableList.Count; i++)
        {
            if (clickableList[i] != _emptyHolder)
            {
                if (!slotList[i].activeInHierarchy)
                {
                    slotList[i].transform.localScale = Vector3.zero;
                    slotList[i].SetActive(true);
                    slotList[i].transform.DOScale(Vector3.one, 0.75f);
                }
                imagesList[i].enabled = true;
                imagesList[i].sprite = clickableList[i].ProgressionImage;
            }
            else 
            {
                if (i > 0)
                {
                    if (slotList[i].activeInHierarchy)
                    {
                        slotList[i].transform.DOScale(Vector3.zero, 0.75f).OnComplete(() => 
                        {
                             slotList[i].SetActive(false);
                        });
                    }
                } 
                imagesList[i].enabled = false;
            }
        }
    }

    public void UpdateDisplay()
    {
        UpdateRow(_toBeFound, _toBeFoundSlots, _toBeFoundSlotsImages);
        UpdateRow(_found, _foundSlots, _foundSlotsImages);
    }



    private void ToggleBar(List<GameObject> slots,List<ClickableHolder> content, bool toggle)
    {
        for (int i = 1; i < slots.Count; i++)
        {
            if (content[i] != _emptyHolder)
            {
                slots[i].SetActive(toggle);
            }
        }
    }

    private void ChangeAlpha(Image img, float a)
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, a);
        img.color = c;
    }

    public void DOAlpha(Image img, float a, float time)
    {
        Color c = new Color(img.color.r, img.color.g, img.color.b, a);
        img.DOColor(c, time);
    }

    private void FadeIn(float time)
    {

    }

    private void FadeOut(float time, bool fadeInAgain = false)
    {

    }

}
