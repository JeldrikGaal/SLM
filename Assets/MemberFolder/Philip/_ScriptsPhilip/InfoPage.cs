using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPage : MonoBehaviour
{
    private ClickableStorage _storage;

    public GameObject infoCard;
    public Transform infoHolder;

    private void Awake()
    {
        _storage = ClickableStorage.Instance;

        if (ClickableStorage.Instance == null) return;
        
        for (int i = 0; i < _storage._clickedQuestion.Count; i++)
        {
            var newInfoCard = (GameObject)Instantiate(infoCard, infoHolder);

            newInfoCard.GetComponent<InfoCard>().image.sprite = _storage._clickedQuestion[i].Image;
            newInfoCard.GetComponent<InfoCard>().description.text = _storage._clickedQuestion[i].Description;
        }
    }

    
}
