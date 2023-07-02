using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class InfoCard : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI description;

    public void SetContent(Sprite sprite, string desc)
    {
        image.sprite = sprite;
        description.text = desc;
    }

}
