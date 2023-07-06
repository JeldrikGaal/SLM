using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardData
{
    public int ID;
    public string LocalizationKey;
    public Sprite BackSprite;
    public Sprite FrontSprite;
    public int MatchingID;
}

public class CardDatabase : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform canvas;
    public Canvas cardCanvas;
    public CardData[] cards;
    private Deck DeckRef;
    private DragOverManager _dragOverManager;
    public Sprite DefaultCardSprite;

    private void Awake()
    { 
        DeckRef = GetComponent<Deck>();
        _dragOverManager = GetComponent<DragOverManager>();
    }

    public Card SpawnCard(int ID, Vector3 parentTransform, bool pDraggable, bool pFlippable, float angle = 0)
    {
        // Instantiate a new card as a child of the provided transform
        GameObject cardGO = Instantiate(cardPrefab, parentTransform, Quaternion.Euler(0, 0, angle), canvas);

        // Get the Card component
        Card card = cardGO.GetComponent<Card>();

        // Initialize the card with the ID
        card.Init(ID, this, pDraggable, pFlippable, cardCanvas, DeckRef, _dragOverManager);
        card.SetCardSide(true);
        // Return the Card component
        return card;
    }
    
}
