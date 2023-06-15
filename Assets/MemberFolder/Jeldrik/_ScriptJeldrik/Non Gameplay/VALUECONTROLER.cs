using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VALUECONTROLER : MonoBehaviour
{
    [Header("Outline")]
    [Tooltip("The size from the outliner outline in percent betwenn 0-100%"), Range(0,100)] public int Outline_Size;
    [Tooltip("The color of the outline")] public Color Outline_Color;

    [Header("Book transition")]
    [Tooltip("Time for the Book transition at the end of the game")] public float BookTransition_Time;
    
    [Header("Swirl")]
    [Tooltip("Time the swirl takes to swirl :)")] public float Swirl_Time;

    [Header("Confetti")]
    [Tooltip("The size of the confetti")]
    public float Confetti_Size;

    [Header("SceneInfo")]
    [Tooltip("Speed of spawning info")] public float SceneInfo_AnimSpeed;
    [Tooltip("Time the scene input is blocked")] public float SceneInfo_BlockTime;
    [Tooltip("Time after the scene reminder is spawned")] public float SceneInfo_ReminderTime;

    [Header("PopUp")]
    [Tooltip("Time the scene input is blocked")] public float PopUp_BlockTime;
    [Tooltip("Time after the scene reminder is spawned")] public float PopUp_ReminderTime;
    [Tooltip("0-100% of the upper half of the screen to position the PopUp"), Range(0, 100)] public float PopUp_Pos;
    [Tooltip("How long the animation when the PopUp spawns take")] public float PopUp_AnimSpeed;

    [Header("Reminder PopUp")]
    [Tooltip("Time the scene input is blocked")] public float Reminder_BlockTime;
    [Tooltip("Time after the scene reminder is spawned")] public float Reminder_ReminderTime;
    [Tooltip("0-100% of the upper half of the screen to position the PopUp"), Range(0, 100)] public float Reminder_Pos;

    

    [Header("Questions")]
    [Tooltip("List of the Question objects in order of needed completion")] public List<Question> Questions;

    [Header("Clickables")]
    [Tooltip("Highlighted object position"), Range(0, 100)] public float Clickable_Pos;

    [Header("Camera")]
    [Tooltip("Border around the image that the camera can move in"), Range(0, 1)] public float Camera_Border;
    [Tooltip("How long the Camera takes to move to the clicked options")] public float Camera_ClickMoveTime;
    



    /* 
    [Header("")]
    [Tooltip("")]
    */

    void Start()
    {
         VALUECONTROLER _VC;
        _VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
