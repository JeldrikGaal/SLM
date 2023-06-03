using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds all information needed to construct and fill a reminder object
/// </summary>
[CreateAssetMenu(fileName = "ReminderPopUp_Name_ID", menuName = "ScriptableObjects/ReminderPopUpHolder")]
public class ReminderPopUpHolder : ScriptableObject
{
    public string Title;
    public string Content;
    public float LifeTime;
}
