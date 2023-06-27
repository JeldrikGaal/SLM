using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Card DeckTopCard;
    private CardDatabase CDB;
    private List<TPC> PileColliders = new List<TPC>();
    private List<Card> DrawPileCards = new List<Card>();
    public float TargetPiles_yOffset;
    public int[] Deck1_Initial;
    public int[] Deck2;
    public int[] Deck3;
    public int[] Deck4;
    public int[] Deck5;
    public int[] Deck6;
    private int CurrentDeckIndex = 1;
    private int maxDecks = 6;
    
    void Start()
    {
        Deck1_Initial = ShuffleArray(Deck1_Initial);
        Deck2 = ShuffleArray(Deck2);
        Deck3 = ShuffleArray(Deck3);
        Deck4 = ShuffleArray(Deck4);
        Deck5= ShuffleArray(Deck5);
        Deck6 = ShuffleArray(Deck6);
        
        CDB = GetComponent<CardDatabase>();

        NextDeck();
        
        SpawnIntitalTargetCards();
    }

public void NextDeck()
{
    DeckTopCard = null;
    CurrentDeckIndex++;
    if (CurrentDeckIndex <= maxDecks)
    {
        switch (CurrentDeckIndex)
        {
            case 2:
                for (int i = 0; i < Deck2.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck2[i], startObject.transform.position + i * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 3:
                for (int i = 0; i < Deck3.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck3[i], startObject.transform.position + i * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 4:
                for (int i = 0; i < Deck4.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck4[i], startObject.transform.position + i * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 5:
                for (int i = 0; i < Deck5.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck5[i], startObject.transform.position + i * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 6:
                for (int i = 0; i < Deck6.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck6[i], startObject.transform.position + i * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            default:
                // Do something for all other cases
                break;
        }
    }
}


    public void MoveTopCardToCenter()
    {
        if (DeckTopCard)
        {
            DeckTopCard.MoveToCenter();
            DeckTopCard.MakeInteractableAfterTime();
            int _count = DrawPileCards.Count;
            if (_count > 0) DrawPileCards.RemoveAt(_count-1);
            if (_count - 1 > 0)
            {
                DeckTopCard = DrawPileCards[DrawPileCards.Count-1];
            }
            else
            {
                DeckTopCard = null;
            }
        }
    }
    
    void SpawnIntitalTargetCards()
    {
        for (int i = 0; i < Deck1_Initial.Length; i++)
        {
            Card CardReference = CDB.SpawnCard(Deck1_Initial[i], startObject.transform.position + i * offset, false, true);
            RectTransform cardRectTransform = CardReference.GetComponent<RectTransform>();

            CardReference._placedOnTargetPile = true;
            
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
            
            
            PileColliderGO.GetComponent<TPC>().TopCardGO = CardReference.gameObject;
            PileColliderGO.GetComponent<TPC>().BottomCardGO = CardReference.gameObject;
        }
    }

    public void SetPileCollidersTraceable(bool pTraceable)
    {
        foreach (TPC tpc in PileColliders)
        {
            tpc.SetTraceable(pTraceable);
        }
    }
    
    
    public T[] ShuffleArray<T>(T[] array)
    {
        System.Random rand = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        return array;
    }

    
    
}
