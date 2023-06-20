using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab; // Assign your card prefab in the inspector
    public Transform canvas; // Assign your Canvas in the inspector
    public Canvas cardCanvas;
    public GameObject startObject; // Assign your starting GameObject in the inspector
    public Vector3 offset; // The offset between each card
    public int numCards = 3; // Number of cards in the deck
    public Sprite[] CardImages;
    private Card DeckTopCard;

    private List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        // Instantiate the cards
        for (int i = 0; i < numCards; i++)
        {
            var card = Instantiate(cardPrefab, startObject.transform.position + i * offset, Quaternion.identity, canvas);
            var CardRef = card.GetComponent<Card>();
            CardRef.yourCanvas = cardCanvas;
            CardRef.EnableDragging(false);
            CardRef.EnableFlippable(false);
            CardRef.SetCardSide(true);
            CardRef.SetPicture(CardImages[i]);
            cards.Add(card);
            DeckTopCard = CardRef;
        }
    }

    public void NextCard()
    {
        if (cards.Count == 0)
        {
            Debug.Log("No more cards!");
            return;
        }

        // Get the next card and move it to the middle of the screen
        var currentCard = cards[0];
        cards.RemoveAt(0);

        // Add your logic here to move the card to the desired location
    }

    public bool CheckCard(GameObject targetCard)
    {
        // Check if the card can be placed on the target card
        // Add your logic here to check the rules of your game

        return false; // placeholder return statement
    }

    public void MoveTopCardToCenter()
    {
        DeckTopCard.MoveToCenter();
        DeckTopCard.MakeInteractableAfterTime();
    }
}
