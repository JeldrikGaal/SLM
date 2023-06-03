using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "ReminderPopUp_Name_ID", menuName = "ScriptableObjects/ReminderPopUpHolder")]
public class ReminderPopUpHolder : ScriptableObject
{
    public string Title;
    public string Content;
    public float LifeTime;
}
