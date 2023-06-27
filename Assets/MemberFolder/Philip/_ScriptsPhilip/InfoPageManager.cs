using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPageManager : MonoBehaviour
{
    public static InfoPageManager Instance;

    public List<GameObject> _infoPages;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
