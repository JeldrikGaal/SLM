using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetCardPiles : MonoBehaviour
{
    public GameObject cardPrefab;  // The card prefab, set in the inspector
    public RectTransform canvas;  // The canvas where cards will be instantiated, set in the inspector
    private List<GameObject> cards = new List<GameObject>(); // List to hold the card instances
    public Canvas cardCanvas;
    public float xOffset = 10f; // The offset from the left border of the screen

    void Start()
    {
        SpawnCards();
    }


    void SpawnCards()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(cardPrefab, cardCanvas.transform); // Instantiate a new card
            Card CardRef = card.GetComponent<Card>();
            RectTransform cardRectTransform = card.GetComponent<RectTransform>();

            // Calculate the anchors for the card
            float anchorY = 1f - (i + 0.5f) / 4f;
            cardRectTransform.anchorMin = new Vector2(0, anchorY);
            cardRectTransform.anchorMax = new Vector2(0, anchorY);

            // Set the position of the card to be offset from the left border of the Canvas
            cardRectTransform.anchoredPosition = new Vector2(xOffset, 0);
            
            // Set up card
            CardRef.EnableDragging(false);
            CardRef.EnableFlippable(true);
            

            // Add the new card to our list
            cards.Add(card);
        }
    }

}