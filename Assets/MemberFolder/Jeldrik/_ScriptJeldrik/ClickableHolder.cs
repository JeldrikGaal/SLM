using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Object_Name_ID", menuName = "ScriptableObjects/ClickableHolder", order = 1)]
public class ClickableHolder : ScriptableObject
{
    public Sprite Image;
    public string Title;
    public string Description;
    public Color BackgroundColor;
}
