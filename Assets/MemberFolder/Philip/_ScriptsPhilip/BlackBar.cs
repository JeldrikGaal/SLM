using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBar : MonoBehaviour
{
    public int minigame;
    private Animator anim;

    private bool triggered;
    
    void Start()
    {
        anim = GetComponent<Animator>();

        if (triggered)
        {
            Destroy(gameObject);
        }

        switch (minigame)
        {
            case 1:
                if (GameManager.Instance.minigame1Complete && !triggered)
                {
                    anim.Play("blackbarfadeout");
                    triggered = true;
                }
                break;
            case 2:
                if (GameManager.Instance.minigame2Complete && !triggered)
                {
                    anim.Play("blackbarfadeout");
                    triggered = true;
                }
                break;
            default:
                break;
        }
    }
}
