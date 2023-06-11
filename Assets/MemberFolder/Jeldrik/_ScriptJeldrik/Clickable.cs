using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Clickable : MonoBehaviour
{
    #region References
    [Tooltip("Assign Object that holds all clickables")]
    [SerializeField] ClickableHolder cH;
    private ClickableManager cM;
    private Image _image;
    private TouchHandler _tH;
    #endregion

    void Start()
    {
        // Assigning references
        cM = GameObject.FindGameObjectWithTag("ClickableManager").GetComponent<ClickableManager>();
        _image = GetComponent<Image>();
        _tH = Camera.main.GetComponent<TouchHandler>();

        // Making the editor display invisible on scene start
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }

    // Function used by the UI button if this object is clicked. Tries to display the information in an popup by calling the function on the Clickable Manager and returns an warning if the holder is missing
    public void Clicked()
    {
        if (cH != null)
        {
            cM.TryDisplayPopUp(cH, this);
            _tH.BlockWrongInputPart();
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

    void Update()
    {
        
    }
}
