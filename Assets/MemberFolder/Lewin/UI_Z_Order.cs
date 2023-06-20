using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Z_Order : MonoBehaviour
{
    public bool BringToFront;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetUpZOrder", 0.01f);
        Invoke("SetUpZOrder", 0.2f);
    }

    private void SetUpZOrder()
    {
        if (BringToFront)
        {
            transform.SetAsLastSibling();
        }
        else
        {
            transform.SetAsFirstSibling();
        }
    }
}
