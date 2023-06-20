using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeOutline : MonoBehaviour
{

    #region References
    [SerializeField] Color _color;
    [SerializeField] private Material _material;
    private Image _image;
    private Image _outline1;
    private Image _outline2;
    private VALUECONTROLER _VC;
    #endregion

    void Awake()
    {
        _image = GetComponent<Image>();
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
        SpawnOutline();
        ToggleOutline(false);
    }

    // Creates 2 objects based on the image the parent object of this script contains
    // One is smaller making an inner outline and one is bigger making an outer outline
    // The original object will be parented to the inner outline to ensure the correct rendering order
    public void SpawnOutline()
    {

        // Calculating scalings based on VC values and relative screen position
        float scaling = transform.localScale.x - ( ( transform.localScale.x * 0.05f) * (_VC.Outline_Size * 0.02f) );
        float scaling2 = 1 + (.08f * (_VC.Outline_Size * 0.02f));

        // Making the objects
        GameObject temp1 = MakeOutlineObject(scaling);
        GameObject temp2 = MakeOutlineObject(scaling2);
        
        // Getting images
        _outline1 = temp1.GetComponent<Image>();
        _outline2 = temp2.GetComponent<Image>();

        // Setting rotation of outline to match object
        //temp1.transform.localRotation = transform.localRotation;
        //temp2.transform.localRotation = transform.localRotation;

        // Switching the render order
        temp2.transform.SetParent(temp1.transform, false);
        temp2.transform.localPosition = Vector3.zero;
        transform.parent = temp1.transform;
        
    }

    // Toggles the outlines on and off
    public void ToggleOutline(bool toggle)
    {
        _outline1.enabled = toggle;
        _outline2.enabled = toggle;
    }

    // Creates an gameobject with an image component that holds the same sprite as the original image
    // but this sprite gets assigned a material making it all white and then getting colored to the outline color
    private GameObject MakeOutlineObject(float scaling)
    {
        

        RectTransform rectT = GetComponent<RectTransform>();
        GameObject temp = new GameObject();
        temp.transform.parent = transform.parent;
        temp.AddComponent<RectTransform>();
        temp.transform.localScale = new Vector3(scaling, scaling, scaling);
        RectTransform rectTI = temp.GetComponent<RectTransform>();
        rectTI.sizeDelta = rectT.sizeDelta;
        temp.transform.position = transform.position;
        Image temp_i = temp.AddComponent<Image>();
        temp_i.sprite = _image.sprite;
        temp_i.material = _material;
        temp_i.color = _VC.Outline_Color;
        temp_i.raycastTarget = false;

        return temp;
    }
}
