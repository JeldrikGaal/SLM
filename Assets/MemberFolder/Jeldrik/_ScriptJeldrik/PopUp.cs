using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    private ClickableManager cM;
    // Start is called before the first frame update
    void Start()
    {
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
    }

    public void Close()
    {
        cM.HidePopUp();
    }
}
