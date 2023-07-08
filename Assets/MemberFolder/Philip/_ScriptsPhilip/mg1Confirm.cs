using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg1Confirm : MonoBehaviour
{
    public MenuManager mg;
    public Animator anim;

    public int minigame = 1;
    
    public void no()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine("Deactivate", 0.7f);
    }

    private IEnumerator Deactivate(float delay)
    {
        anim.SetTrigger("Exit");
        
        yield return new WaitForSeconds(delay);
        
        this.gameObject.SetActive(false);
    }

    public void yes()
    {
        if (minigame == 1)
        {
            mg.LoadMinigame1();
        } else if (minigame == 2)
        {
            mg.LoadMinigame2();
        }
    }
    
    
}
