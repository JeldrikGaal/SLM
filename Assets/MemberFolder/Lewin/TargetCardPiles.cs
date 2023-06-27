using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetCardPiles : MonoBehaviour
{
    public GameObject TargetPileCollider;  // The card prefab, set in the inspector
    public RectTransform canvas;  // The canvas where cards will be instantiated, set in the inspector
    private List<GameObject> cards = new List<GameObject>(); // List to hold the card instances
    public Canvas cardCanvas;
    public float yOffset = 10f; // The offset from the left border of the screen

    void Start()
    {
        //SpawnPileColliders();
    }


    void SpawnPileColliders()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject PileCollider = Instantiate(TargetPileCollider, cardCanvas.transform); // Instantiate a new collider
            RectTransform cardRectTransform = PileCollider.GetComponent<RectTransform>();

            // Calculate the anchors for the card
            float anchorX = 1f - (i + 0.5f) / 4f;
            cardRectTransform.anchorMin = new Vector2(anchorX,1);
            cardRectTransform.anchorMax = new Vector2(anchorX,1);
            
            // Set the pivot to be at the top of the card
            cardRectTransform.pivot = new Vector2(0.5f, 1);
            
            // Set the position of the card to be offset from the left border of the Canvas
            cardRectTransform.anchoredPosition = new Vector2(0f, -yOffset);
        }
    }

}