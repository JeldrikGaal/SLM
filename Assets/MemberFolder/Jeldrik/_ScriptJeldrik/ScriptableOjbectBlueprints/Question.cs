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
    public ClickableHolder ObjectToFind;
    public bool End = false;
}
