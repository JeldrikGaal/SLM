using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class InfoCard : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI description;
    public TextMeshProUGUI title;

    public void SetContent(Sprite sprite, string desc, string tit)
    {
        image.sprite = sprite;
        description.text = desc;
        title.text = tit;
    }

}
