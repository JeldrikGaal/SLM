using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VALUECONTROLER : MonoBehaviour
{
    [Header("Outline")]
    [Tooltip("The size from the outliner outline in percent betwenn 0-100%"), Range(0,100)] public int Outline_Size;
    [Tooltip("The color of the outline")] public Color Outline_Color;

    [Header("SceneInfo")]
    [Tooltip("Speed of spawning info")] public float SceneInfo_AnimSpeed;
    [Tooltip("Time the scene input is blocked")] public float SceneInfo_BlockTime;
    [Tooltip("Time after the scene reminder is spawned")] public float SceneInfo_ReminderTime;

    [Header("PopUp")]
    [Tooltip("Time the scene input is blocked")] public float PopUp_BlockTime;
    [Tooltip("Time after the scene reminder is spawned")] public float PopUp_ReminderTime;
    [Tooltip("How long the animation when the PopUp spawns take")] public float PopUp_AnimSpeed;
    [Tooltip("0-100% of the upper half of the screen to position the PopUp"), Range(0, 100)] public float PopUp_Reminder_Pos;

    [Header("Questions")]
    [Tooltip("List of the Question objects in order of needed completion")] public List<Question> Questions;

    [Header("Clickables")]
    [Tooltip("Highlighted object position ( 0-100% of half the screen starting from the camera)"), Range(0, 100)] public float Clickable_Pos;

    [Header("Camera")]
    [Tooltip("Border around the image that the camera can move in"), Range(0, 1)] public float Camera_Border;
    [Tooltip("How long the Camera takes to move to the clicked options")] public float Camera_ClickMoveTime;
    [Tooltip("Factor with how fast the camera moves when being dragged around"), Range(0,1)] public float Camera_MoveSpeed;

    [Header("Misc")]
    [Tooltip("How long to wait for next input to deploy a reminder")] public float Misc_WaitTime_InputReminder;
    [Tooltip("How long to wait after first reminder to deploy a second one")] public float Misc_SecondWaitTime_InputReminder;
    [Tooltip("The prefab for the reminder that gets deployed after having no input for Misc_WaitTime_InputReminder seconds")] public GameObject Misc_Reminder_Object;

    [Header("Book transition")]
    [Tooltip("Time for the Book transition at the end of the game")] public float BookTransition_Time;
    
    [Header("Swirl")]
    [Tooltip("Time the swirl takes to swirl :)")] public float Swirl_Time;

    [Header("Confetti")]
    [Tooltip("The size of the confetti")] public float Confetti_Size;

    [Header("Glow Effect")]
    [Tooltip("The size of the confetti")] public Color Glow_Color;
    [Tooltip("The size of the confetti")] public float RepetetionTime;
    [Tooltip("The size of the confetti")] public float GlowTime;
    public bool Do_Glow_Effect;

    /*[InspectorButton("SetValues")]
    [Tooltip("Updates the values from the Valuecontroller during runtime")] public bool SETVALUES;*/
    /* 
    [Header("")]
    [Tooltip("")]
    */

    void Start()
    {
         //VALUECONTROLER _VC;
        //_VC = GameObject.FindGameObjectWithTag("VC").GetComponent<VALUECONTROLER>();
    }

    private void SetValues()
    {
       
    }
    


}
