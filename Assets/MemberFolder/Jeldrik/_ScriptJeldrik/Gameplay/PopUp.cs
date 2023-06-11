using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    private ClickableManager cM;

    void Start()
    {
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
    }

    // Closes the popup - gets called by UI button
    public void Close()
    {
        cM.HidePopUp();
    }
}
