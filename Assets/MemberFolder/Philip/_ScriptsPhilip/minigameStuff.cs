using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigameStuff : MonoBehaviour
{
    public void SetPageIndex(int index)
    {
        GameManager.Instance.pageIndex = index;
        GameManager.Instance.minigame1Complete = true;
    }
}
