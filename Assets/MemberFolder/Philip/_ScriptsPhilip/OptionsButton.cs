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
   public float fontSizeInactive = 28;
   public Image image;
   private RectTransform rect;

   public bool active;

   private void Start()
   {
      text = GetComponentInChildren<TextMeshProUGUI>();
      //image = GetComponent<Image>();
      rect = GetComponent<RectTransform>();
   }

   public void SetActive()
   {
      text.fontSize = fontSizeActive;
      text.fontStyle = FontStyles.Bold;
      var shadow = GetComponent<Shadow>().effectDistance = new Vector2(5, -5);
      rect.sizeDelta = new Vector2(250f, 130f);
      image.color = new Color(231f/255f, 221f/255f, 215f/255f);

      active = true;
   }

   public void SetInactive()
   {
      text.fontSize = fontSizeInactive;
      text.fontStyle = FontStyles.Normal;
      var shadow = GetComponent<Shadow>().effectDistance = new Vector2(1, -2);
      rect.sizeDelta = new Vector2(200f, 90f);
      image.color = new Color(250f/255f, 250f/255f, 250f/255f);

      active = false;
   }
}
