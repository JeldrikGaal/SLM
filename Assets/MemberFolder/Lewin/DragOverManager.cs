using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOverManager : MonoBehaviour
{
    private Card _activeDraggingCard;
    private bool _shouldRaycast;
    private bool _dropInfoShow;

    public void SetActiveDraggingCard(Card pCard)
    {
        _shouldRaycast = pCard != null;
        _activeDraggingCard = pCard;
        if (pCard == null) _dropInfoShow = false;
    }

    void Update()
    {
        bool madeitshown = false;
        if (_shouldRaycast)
        {
            // Check if the mouse pointer is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Find what object the pointer is currently over
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if(results.Count > 0)
                {
                    //For every result returned, output its tag if it's not a "Card"
                    foreach (RaycastResult result in results)
                    {
                        if(result.gameObject.tag != "" && result.gameObject.tag != "Card")
                        {
                            Debug.Log("Pointer is over a UI element with the tag: " + result.gameObject.tag);
                            if (result.gameObject.tag == "TP_Collider")
                            {
                                ShowOutline(true, gameObject);
                                madeitshown = true;
                            }
                            
                        }
                        
                    }
                }
                
            }
            
        }
        if (!madeitshown) ShowOutline(false, null);
    }

    void ShowOutline(bool show, GameObject pOtherCard)
    {
        if (show != _dropInfoShow)
        {
            _dropInfoShow = show;
            _activeDraggingCard.ShowDropInfo(_dropInfoShow, pOtherCard);
        }
    }
    
    
}
