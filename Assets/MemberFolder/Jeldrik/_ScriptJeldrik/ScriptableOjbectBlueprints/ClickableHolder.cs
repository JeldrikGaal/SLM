using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to hold information about any clickable objects in the scene that is used to display the information in a popup 
/// </summary>
[CreateAssetMenu(fileName = "Object_Name_ID", menuName = "ScriptableObjects/ClickableHolder", order = 1)]
public class ClickableHolder : ScriptableObject
{
    [Header("Fill this stuff with all needed info")]
    [Tooltip("Main image thats gonna be displayed")] public Sprite Image;
    [Tooltip("Title of the image / popup")] public string Title;
    [Tooltip("Additional second information")] public string Description;
    [Tooltip("Information that only needs to be displayed in the book")] public string BookInfo;
    [Tooltip("True if this object is needed to proceed in a question")] public bool Question;
    [Tooltip("Language ID ( 0 = German, 1 = English ")] public int language;

    [Header("Info only for runtime do not touch bls")]
    public float FirstClickTime;
    public int Num;
}
