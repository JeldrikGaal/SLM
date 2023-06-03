using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to hold information about any clickable objects in the scene that is used to display the information in a popup 
/// </summary>
[CreateAssetMenu(fileName = "Object_Name_ID", menuName = "ScriptableObjects/ClickableHolder", order = 1)]
public class ClickableHolder : ScriptableObject
{
    public Sprite Image;
    public string Title;
    public string Description;
    public Color BackgroundColor;
}
