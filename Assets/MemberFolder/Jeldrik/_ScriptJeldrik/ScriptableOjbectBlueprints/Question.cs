using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds all information needed to construct and fill a question field in the question manager
/// </summary>
[CreateAssetMenu(fileName = "Question_Number", menuName = "ScriptableObjects/Question")]
public class Question : ScriptableObject
{
    public string Text;
    public string Hint;
    public int Number;
    public Sprite Image;
    public List<ClickableHolder> ObjectsToFind1 = new List<ClickableHolder>();
    public List<ClickableHolder> ObjectsToFind2 = new List<ClickableHolder>();
    public List<ClickableHolder> ObjectsToFind3 = new List<ClickableHolder>();
    public List<int> AmountNeeded = new List<int>(); 
    public bool End = false;

    public string LocalizationKey;
}
