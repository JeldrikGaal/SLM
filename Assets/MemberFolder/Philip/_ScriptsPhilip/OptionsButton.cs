using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class OptionsButton : MonoBehaviour
{
   private TextMeshProUGUI text;
   public float fontSizeActive = 40;
   public float fontSizeInactive = 35;
   private Image image;
   private RectTransform rect;

   public bool active;

   private void Start()
   {
      text = GetComponentInChildren<TextMeshProUGUI>();
      image = GetComponent<Image>();
      rect = GetComponent<RectTransform>();
   }

   public void SetActive()
   {
      text.fontSize = fontSizeActive;
      var shadow = GetComponent<Shadow>().effectDistance = new Vector2(5, -5);
      rect.sizeDelta = new Vector2(250f, 130f);
      image.color = new Color(250,250,250);

      active = true;
   }

   public void SetInactive()
   {
      text.fontSize = fontSizeInactive;
      var shadow = GetComponent<Shadow>().effectDistance = new Vector2(1, -2);
      rect.sizeDelta = new Vector2(200f, 90f);
      image.color = new Color(230,230,230);

      active = false;
   }
}
