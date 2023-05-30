using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Clickable : MonoBehaviour
{
    [SerializeField] ClickableHolder cH;
    private ClickableManager cM;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        _image = GetComponent<Image>();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }

    public void Clicked()
    {
        if (cH != null)
        {
            cM.TryDisplayPopUp(cH, this);
        }
        else
        {
            Debug.LogWarning("Clickable" + transform.name + "has been clicked but does not cotain an Holder");
        }
       
    }

    public void SetColor(Color c)
    {
        _image.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
