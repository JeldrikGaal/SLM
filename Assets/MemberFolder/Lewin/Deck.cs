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
    public int[] WholeDrawPile;
    private int CurrentDeckIndex = 7;
    private int maxDecks = 6;
    private int counterDrawDeckCards = 0;

    public GameObject EmptyGameObject; // The GameObject you select in the Inspector
    public float FinalEndMoveTargetY; // The target y position you want to animate to
    public float FinalEndMoveTargetX; // The target x position you want to animate to
    public float FinalCamMoveDuration; // Duration of the animation

    public GameObject FinalParentGO;

    public Image _vignette;
    private float _originalAlpha;
    private bool _drawnFistCard;

    
    void Awake()
    {
        _originalAlpha = _vignette.color.a;
    }
    void Start()
    {
        Deck2 = ShuffleArray(Deck2);
        Deck3 = ShuffleArray(Deck3);
        Deck4 = ShuffleArray(Deck4);
        Deck5= ShuffleArray(Deck5);
        Deck6 = ShuffleArray(Deck6);
        
        CDB = GetComponent<CardDatabase>();

        
        SpawnIntitalTargetCards();
        SpawnWholeDrawPile();

        //StartCoroutine(Final_1(5.5f));
        ReparentChildObjects(cardCanvas.transform, EmptyGameObject.transform);

    }

    IEnumerator Final_1(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ReparentChildObjects(cardCanvas.transform, EmptyGameObject.transform);
        StartCoroutine(AnimatePosition(EmptyGameObject, FinalEndMoveTargetY, FinalEndMoveTargetX, FinalCamMoveDuration));
    }

    public void NextDeck()
{
    DeckTopCard = null;
    CurrentDeckIndex--;
    if (CurrentDeckIndex > 1)
    {
        switch (CurrentDeckIndex)
        {
            case 2:
                for (int i = 0; i < Deck2.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck2[i], startObject.transform.position + counterDrawDeckCards * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 3:
                for (int i = 0; i < Deck3.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck3[i], startObject.transform.position + counterDrawDeckCards * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 4:
                for (int i = 0; i < Deck4.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck4[i], startObject.transform.position + counterDrawDeckCards * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 5:
                for (int i = 0; i < Deck5.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck5[i], startObject.transform.position + counterDrawDeckCards * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            case 6:
                for (int i = 0; i < Deck6.Length; i++)
                {
                    Card CardGO = CDB.SpawnCard(Deck6[i], startObject.transform.position + counterDrawDeckCards * offset, false, false);
                    var CardRef = CardGO.GetComponent<Card>();
                    DrawPileCards.Add(CardRef);
                    DeckTopCard = CardRef;
                }
                break;

            default:
                break;
        }
            counterDrawDeckCards++;
    }
}

    public int[] AppendAllDecks()
    {
        return Deck6.Concat(Deck5).Concat(Deck4).Concat(Deck3).Concat(Deck2).ToArray();
    }
    public void MoveTopCardToCenter()
    {
        if (DeckTopCard)
        {
            if (!_drawnFistCard)
            {
                _drawnFistCard = true;
                foreach (var _tpc in PileColliders)
                {
                    _tpc.TopCardGO.GetComponent<Card>().FlipAnimated();
                }
            }
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
        else StartCoroutine(Final_1(0.5f));
    }
    
    void SpawnIntitalTargetCards()
    {
        for (int i = 0; i < Deck1_Initial.Length; i++)
        {
            Card CardReference = CDB.SpawnCard(Deck1_Initial[i], startObject.transform.position + i * offset, false, true);
            RectTransform cardRectTransform = CardReference.GetComponent<RectTransform>();

            CardReference._placedOnTargetPile = true;
            //CardReference.FlipAnimated();

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

    void SpawnWholeDrawPile()
    {
        WholeDrawPile = AppendAllDecks();
        for (int i = 0; i < WholeDrawPile.Length; i++)
        {
            float angle = 0;
            if (i == 0) angle = -14;
            if (i == 1) angle = 7;
            if (i == 2) angle = -4;
            Card CardGO = CDB.SpawnCard(WholeDrawPile[i], startObject.transform.position, false, false, angle);
            var CardRef = CardGO.GetComponent<Card>();
            DrawPileCards.Add(CardRef);
            DeckTopCard = CardRef;
        }
    }


    void ReparentChildObjects(Transform parent, Transform newParent)
    {
        // Temporarily store all first-level children
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }

        // Iterate over the stored list and reparent
        foreach (Transform child in children)
        {
            if (child.gameObject == EmptyGameObject) continue; // Ignore the EmptyGameObject itself
            child.SetParent(newParent, true);
        }
    }
    

    IEnumerator AnimatePosition(GameObject obj, float offsetY, float offsetX, float duration)
    {
        DisableCardInteractions();
        Vector3 startPos = obj.transform.position;

        // Convert offset to relative offset using Screen height
        float relativeOffsetY = (Screen.height * offsetY) / 100.0f;
        float relativeOffsetX = (Screen.width * offsetX) / 100.0f;

        Vector3 endPos = new Vector3(startPos.x + relativeOffsetX, startPos.y + relativeOffsetY, startPos.z);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = SmootherStep(0.0f, 1.0f, t); // Use SmootherStep instead of Mathf.SmoothStep
            obj.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        obj.transform.position = endPos; // Ensure GameObject is at target position at the end
    }




    float SmootherStep(float edge0, float edge1, float x)
    {
        x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
        return x * x * x * (x * (x * 6 - 15) + 10);
    }

    void DisableCardInteractions()
    {
        foreach (var collider in PileColliders)
        {
            collider.GetComponent<TPC>().TopCardGO.GetComponent<Card>().FlipIfPictureSideIsUp();
            collider.GetComponent<TPC>().TopCardGO.GetComponent<Card>().EnableFlippable(false);
        }
    }
    
    public void AnimateVignetteAlpha(float targetAlpha, float duration)
    {
        StartCoroutine(AnimateAlpha(targetAlpha, duration));
    }

    IEnumerator AnimateAlpha(float targetAlpha, float duration)
    {
        // Store the original color
        Color originalColor = _vignette.color;
        
        // Half duration for fade in and fade out separately
        float halfDuration = duration / 2;

        // Animate from original alpha to target alpha
        for (float t = 0; t < 1; t += Time.deltaTime / halfDuration)
        {
            float newAlpha = Mathf.Lerp(_originalAlpha, targetAlpha, t);
            _vignette.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        // Animate from target alpha back to original alpha
        for (float t = 0; t < 1; t += Time.deltaTime / halfDuration)
        {
            float newAlpha = Mathf.Lerp(targetAlpha, _originalAlpha, t);
            _vignette.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        // Ensure it finishes exactly at the original alpha
        _vignette.color = originalColor;
    }

    
}

