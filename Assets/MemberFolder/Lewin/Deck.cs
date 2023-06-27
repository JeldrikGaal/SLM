using System.Collections;
using System.Collections.Generic;
using CW.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Deck : MonoBehaviour
{
    public GameObject cardPrefab; // Assign your card prefab in the inspector
    public Transform canvas; // Assign your Canvas in the inspector
    public Canvas cardCanvas;
    public GameObject startObject; // Assign your starting GameObject in the inspector
    public Vector3 offset; // The offset between each card
    public GameObject TargetPileCollider;
    private Card DeckTopCard;
    private CardDatabase CDB;
    private List<TPC> PileColliders = new List<TPC>();
    public float TargetPiles_yOffset;
    public int[] Deck1_Initial;
    public int[] Deck2;
    public int[] Deck3;
    public int[] Deck4;
    public int[] Deck5;
    public int[] Deck6;
    
    void Start()
    {
        CDB = GetComponent<CardDatabase>();
        
        
        for (int i = 0; i < Deck2.Length; i++)
        {
            Card CardGO = CDB.SpawnCard(Deck2[i], startObject.transform.position + i * offset, false, false);
            var CardRef = CardGO.GetComponent<Card>();
            DeckTopCard = CardRef;
        }

        SpawnIntitalTargetCards();
    }

    public void MoveTopCardToCenter()
    {
        DeckTopCard.MoveToCenter();
        DeckTopCard.MakeInteractableAfterTime();
    }
    
    void SpawnIntitalTargetCards()
    {
        for (int i = 0; i < Deck1_Initial.Length; i++)
        {
            Card CardGO = CDB.SpawnCard(Deck1_Initial[i], startObject.transform.position + i * offset, false, true);
            var CardRef = CardGO.GetComponent<Card>();
            RectTransform cardRectTransform = CardGO.GetComponent<RectTransform>();

            // Calculate the anchors for the card
            float anchorX = 1f - (i + 0.5f) / 4f;
            cardRectTransform.anchorMin = new Vector2(anchorX,1);
            cardRectTransform.anchorMax = new Vector2(anchorX,1);
            
            // Set the pivot to be at the top of the card
            cardRectTransform.pivot = new Vector2(0.5f, 1);
            
            // Set the position of the card to be offset from the left border of the Canvas
            cardRectTransform.anchoredPosition = new Vector2(0f, -TargetPiles_yOffset);
            
            
            
            //Spawn PileCollider
            GameObject PileColliderGO = Instantiate(TargetPileCollider, startObject.transform.position + i * offset, Quaternion.identity, canvas);
            PileColliders.Add(PileColliderGO.GetComponent<TPC>());
            RectTransform tpTransform = PileColliderGO.GetComponent<RectTransform>();
            
            tpTransform.anchorMin = new Vector2(anchorX,1);
            tpTransform.anchorMax = new Vector2(anchorX,1);
            
            //Set the pivot to be at the top of the card
            tpTransform.pivot = new Vector2(0.5f, 1);
            
            //Set the position of the card to be offset from the left border of the Canvas
            tpTransform.anchoredPosition = new Vector2(0f, -TargetPiles_yOffset);
        }
    }

    public void SetPileCollidersTraceable(bool pTraceable)
    {
        foreach (TPC tpc in PileColliders)
        {
            tpc.SetTraceable(pTraceable);
        }
    }
}
