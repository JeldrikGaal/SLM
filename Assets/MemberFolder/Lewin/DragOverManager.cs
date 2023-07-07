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
            if (EventSystem.current.IsPointerOverGameObject() || (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                // Find what object the pointer is currently over
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    // check if there is a touch happening
                    position = Input.touchCount > 0 ? (Vector2)Input.GetTouch(0).position : Input.mousePosition
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
                            //Debug.Log("Pointer is over a UI element with the tag: " + result.gameObject.tag);
                            if (result.gameObject.tag == "TP_Collider")
                            {
                                ShowOutline(true, result.gameObject);
                                madeitshown = true;
                                //Debug.Log(result.gameObject.transform.parent.gameObject.GetComponent<TPC>() != null);
                            }
                        
                        }
                    
                    }
                }
            
            }
        
        }
        if (!madeitshown) ShowOutline(false, null);
    }


    void ShowOutline(bool show, GameObject pOtherHitBox)
    {
        if (show != _dropInfoShow)
        {
            _dropInfoShow = show;
            _activeDraggingCard.ShowDropInfo(_dropInfoShow, pOtherHitBox);
        }
    }
    
    
}
