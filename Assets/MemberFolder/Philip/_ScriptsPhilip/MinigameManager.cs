using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private GameManager gm;

    public GameObject scribble1, scribble2;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void CheckCompletion()
    {
        if (gm.toggleMG1)
        {
            scribble1.SetActive(false);
        }

        if (gm.toggleMG2)
        {
            scribble2.SetActive(false);
        }

        if (gm.minigame1Complete)
        {
            StartCoroutine(FadeOut(scribble1, 1));
        }

        if (gm.minigame2Complete)
        {
            StartCoroutine(FadeOut(scribble2, 2));
        }
    }

    private void OnEnable()
    {
        gm = GameManager.Instance;
        
        CheckCompletion();
    }

    private IEnumerator FadeOut(GameObject minigame, int mg)
    {
        minigame.GetComponent<Animator>().SetTrigger("winGame");
        
        yield return new WaitForSeconds(2);

        if (mg == 1)
        {
            gm.toggleMG1 = true;
        }
        else if (mg == 2)
        {
            gm.toggleMG2 = true;
        }
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
