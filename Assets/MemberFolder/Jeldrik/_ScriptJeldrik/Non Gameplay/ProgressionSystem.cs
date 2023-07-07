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
    [SerializeField] private GameObject _foundParent;
    [SerializeField] private GameObject _toBeFoundParent;
    [SerializeField] private TouchHandler _tH;
    private Vector3 _foundParentPos;
    private Vector3 _toBeFoundParentPos;
    private List<Vector3> _foundSlotsPos = new List<Vector3>();
    [SerializeField] private List<GameObject> _toBeFoundSlots = new List<GameObject>();
    private List<Vector3> _toBeFoundSlotsPos = new List<Vector3>();
    private List<Image> _foundSlotsImages = new List<Image>();
    private List<Image> _toBeFoundSlotsImages = new List<Image>();
    [SerializeField] private List<ClickableHolder> _found;
    private bool _foundIsOpen;
    [SerializeField] private List<ClickableHolder> _toBeFound;
    private bool _toBeFoundIsOpen;
    [SerializeField] private ClickableHolder _emptyHolder;
    private List<ClickableHolder> _q1Order = new List<ClickableHolder>();
    private List<ClickableHolder> _q2Order = new List<ClickableHolder>();
    private List<ClickableHolder> _q3Order = new List<ClickableHolder>();
    public List<List<ClickableHolder>> _orders = new List<List<ClickableHolder>>();
    public List<List<ClickableHolder>> _ordersSaved = new List<List<ClickableHolder>>();
    private List<ClickableHolder> _orderSaver = new List<ClickableHolder>();


    // Start is called before the first frame update
    void Awake()
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

        foreach(GameObject g in _foundSlots)
        {
            _foundSlotsPos.Add(g.transform.position);
        }
        foreach(GameObject g in _toBeFoundSlots)
        {
            _toBeFoundSlotsPos.Add(g.transform.position);
        }
        _foundParentPos = _foundParent.transform.localPosition;
        _toBeFoundParentPos = _toBeFoundParent.transform.localPosition;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleBar(true, true);
            ToggleBar(false, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToggleBar(true, false);
            ToggleBar(false, false);
        }

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (_toBeFoundSlots[1].activeInHierarchy || _foundSlots[1].activeInHierarchy)
            {
                HideBothBars();
            }
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

        _orders.Add(_q1Order);
        _orders.Add(_q2Order);
        _orders.Add(_q3Order);
    }

    public void CallFindObjectProgression(int questionNumber, ClickableHolder cH)
    {
        int index = _toBeFound.IndexOf(cH);
        _foundSlots[0].transform.DOScale(Vector3.zero, 0.75f);
        _toBeFoundSlots[index].transform.DOScale(Vector3.zero, 0.75f).OnComplete(() => 
        {
            _toBeFound.Remove(cH);
            _toBeFound.Add(_emptyHolder);
            _found.Insert(0, cH);
            _found.RemoveAt(_found.Count - 1);
            _foundSlots[0].transform.localScale = Vector3.zero;
            UpdateDisplay();
            _foundSlots[0].transform.DOScale(Vector3.one, 0.75f);
            _toBeFoundSlots[index].transform.DOScale(Vector3.one, 0.75f);
        });
    }

    // Returns if one of the first 2 spots is free otherwise returns -1
    private int GetFreeSpot(List<ClickableHolder> order)
    {
        for (int i = 0; i < order.Count -1; i++)
        {
            if (order[i] == _emptyHolder)
            {
                return i;
            }
        }
        return -1;
    }

    public void SaveOrders()
    {

    }
    public void LoadOrders()
    {
        
    }

    public void ShowOrder(List<ClickableHolder> order, bool anim = false) 
    {
        _toBeFoundSlots[0].transform.DOScale(Vector3.zero, 0.75f);
        _foundSlots[0].transform.DOScale(Vector3.zero, 0.75f).OnComplete(() =>
        {
            _found = new List<ClickableHolder>();
            _toBeFound = order;

            // Set all the images for the to be found list 
            for (int i = 0; i < _toBeFound.Count - 1; i++)
            {
                if (_toBeFound[i] != _emptyHolder)
                {
                    _toBeFoundSlotsImages[i].enabled = true;
                    _toBeFoundSlotsImages[i].sprite = _toBeFound[i].ProgressionImage;
                }
                _found.Add(_emptyHolder);
            }

            // Fill the found list with empty holders
            if (_found.Count < 9)
            {
                int count =  9 - _found.Count;
                for (int i = 0; i < count; i++)
                {
                    _found.Add(_emptyHolder);
                }
            }
            // Fill the toBeFound list with empty holders
            if (_toBeFound.Count < 9)
            {
                int count =  9 - _toBeFound.Count;
                for (int i = 0; i < count; i++)
                {
                    _toBeFound.Add(_emptyHolder);
                }
            }

            UpdateDisplay();

            if (anim)
            {
                float newX = _foundParent.transform.localPosition.x + 200;
                _foundParent.transform.DOLocalMoveX(newX, 0.75f).OnComplete(() =>
                {
                     _foundParent.transform.DOLocalMoveX(_foundParentPos.x, 0.75f);
                });

                float newX2 = _toBeFoundParent.transform.localPosition.x + 200;
                _toBeFoundParent.transform.DOLocalMoveX(newX, 0.75f).OnComplete(() =>
                {
                     _toBeFoundParent.transform.DOLocalMoveX(_toBeFoundParentPos.x, 0.75f);
                });
            }
            
            //HideBothBars();
            

            _toBeFoundSlots[0].transform.DOScale(Vector3.one, 0.75f);
            _foundSlots[0].transform.DOScale(Vector3.one, 0.75f);
        });
        
    }


    private void UpdateRow(List<ClickableHolder> clickableList, List<GameObject> slotList, List<Image> imagesList)
    {
        for (int i = 0; i < clickableList.Count; i++)
        {
            if (clickableList[i] != _emptyHolder)
            {
                imagesList[i].enabled = true;
                imagesList[i].sprite = clickableList[i].ProgressionImage;
            }
            else 
            {
                imagesList[i].enabled = false;
                imagesList[i].sprite = null;
            }
        }
    }

    public void UpdateDisplay()
    {
        UpdateRow(_toBeFound, _toBeFoundSlots, _toBeFoundSlotsImages);
        UpdateRow(_found, _foundSlots, _foundSlotsImages);
    }


    public void ToggleFound()
    {
        ToggleBar(true, !_foundIsOpen);
    }
    public void ToggleToBeFound()
    {
        ToggleBar(false, !_toBeFoundIsOpen);
    }

    private void HideBothBars()
    {
        ToggleBar(true, false);
        ToggleBar(false, false);
    }

    private void ToggleBar(bool found, bool toggle)
    {
        if (found)
        {
            _foundIsOpen = toggle;
        }
        else
        {
            _toBeFoundIsOpen = toggle;
        }
        UpdateDisplay();
        List<GameObject> slots = new List<GameObject>();
        List<ClickableHolder> content = new List<ClickableHolder>();
        GameObject parent;
        Vector3 parentPos;
        if (found)
        {
            slots = _foundSlots;
            content = _found;
            parent = _foundParent;
            parentPos = _foundParentPos;
        }
        else
        {
            slots = _toBeFoundSlots;
            content = _toBeFound;
            parent = _toBeFoundParent;
            parentPos = _toBeFoundParentPos;
        }
        if (toggle)
        {
            int i = 0;
            Sequence seq = DOTween.Sequence();
            // Moving Parent
            float spacing = 100 * parent.transform.localScale.x;
            float distance;
            Debug.Log(GetFreeSpot(content));
            if (GetFreeSpot(content) != -1)
            {
                distance = (spacing * ( GetFreeSpot(content) - 1 ));
            }
            else 
            {
                distance = spacing * ( content.Count - 1 );
            }
            float newX = parent.transform.localPosition.x - distance;
            seq.Insert(0, parent.transform.DOLocalMoveX(newX, 0.75f));

            // Scaling slots
            foreach (GameObject g in slots)
            {
                if (i > 0 && content[i] != _emptyHolder)
                {
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    g.SetActive(true);
                    seq.Insert(0.25f, g.transform.DOScale(Vector3.one, 0.75f));
                } 
                i++;
            }
            
        }
        else 
        {
            int i = 0;
            bool once = false;
            Sequence seq = DOTween.Sequence();
            float newX = parentPos.x;
            seq.Insert(0, parent.transform.DOLocalMoveX(newX, 0.75f));
            foreach (GameObject g in slots)
            {
                if (i > 0 && content[i] != _emptyHolder)
                {
                    seq.Insert(0, g.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.75f));
                    seq.OnComplete(() =>
                    {
                        //g.SetActive(false);
                        if (!once)
                        {
                            once = true;
                            ResetAfterToggle();
                        }
                    });
                } 
                i++;
            }
        }
    }

    private void ResetAfterToggle()
    {
        _toBeFoundSlots[0].transform.localScale = Vector3.one;
        _foundSlots[0].transform.localScale = Vector3.one;
        /*for (int i = 0; i < _toBeFoundSlots.Count; i++)
        {
            _toBeFoundSlots[i].transform.position = _toBeFoundSlotsPos[i];
        }
        for (int i = 0; i < _foundSlots.Count; i++)
        {
            _foundSlots[i].transform.position = _foundSlotsPos[i];
        }*/
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
