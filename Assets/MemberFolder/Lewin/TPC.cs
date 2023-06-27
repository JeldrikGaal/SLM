using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPC : MonoBehaviour
{
    
    public Image HitArea;
    private Graphic gp;
    public GameObject TopCardGO;
    public GameObject BottomCardGO;
    public int cardCount;

    private void Awake()
    {
        gp = HitArea.GetComponent<Graphic>();
        SetTraceable(false);
    }

    public void SetTraceable(bool pTraceable)
    {
        HitArea.raycastTarget = pTraceable;
    }
}
