using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private GameManager gm;

    public List<GameObject> scribbles1 = new List<GameObject>();
    public List<GameObject> scribbles2 = new List<GameObject>();

    public MenuManager menuManager;
    
    private void Start()
    {
        gm = GameManager.Instance;
    }
    
    private void CheckCompletion()
    {
        if (gm.toggleMG1)
        {
            foreach (GameObject scribble in scribbles1)
            {
                scribble.SetActive(false);
            }
        }

        if (gm.toggleMG2)
        {
            foreach (GameObject scribble in scribbles2)
            {
                scribble.SetActive(false);
            }
        }

        
        if (gm.minigame1Complete && !gm.toggleMG1)
        {
            StartCoroutine(FadeOut(scribbles1, 1));
        }

        if (gm.minigame2Complete && !gm.toggleMG2)
        {
            StartCoroutine(FadeOut(scribbles2, 2));
        }
    }

    private void OnEnable()
    {
        gm = GameManager.Instance;
        
        CheckCompletion();
    }

    private IEnumerator FadeOut(List<GameObject> minigame, int mg)
    {
        yield return new WaitForSeconds(2);
        
        foreach (GameObject scribble in minigame)
        {
            scribble.GetComponent<Animator>().SetTrigger("winGame");
        }

        yield return new WaitForSeconds(2);
        
        
        if (mg == 1)
        {
            gm.toggleMG1 = true;
        }
        else if (mg == 2)
        {
            gm.toggleMG2 = true;
        }

        menuManager.infoPopup.SetActive(true);
    }

    public void WinGame(int i)
    {
        gm = GameManager.Instance;

        if (i == 1)
        {
            gm.minigame1Complete = true;
        } else if (i == 2)
        {
            gm.minigame2Complete = true;
        }
        
    }
}
