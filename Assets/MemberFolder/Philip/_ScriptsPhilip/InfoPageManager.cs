using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPageManager : MonoBehaviour
{
    public List<GameObject> infoPagePrefabs;
    public GameObject infoPagePrefab;

    public Transform book;
    public GameObject infoCard1;
    public GameObject infoCard2;
    public GameObject infoCard3;
    public GameObject infoCard4;

    public Sprite placeHolderImage;

    public MenuManager menuManager;

    private void Start()
    {
        if (ClickableStorage.Instance == null) {return;}
        StartCoroutine(AddInfoPagesCoroutine());
    }

    private IEnumerator AddInfoPagesCoroutine()
    {
        // Wait for a short delay to ensure the storage is properly initialized
        yield return new WaitForSeconds(0.1f);
        
        CreateInfoPages();
    }

    private void CreateInfoPages()
    {
        ClearInfoPages();
        
        List<ClickableHolder> slaveCards = new List<ClickableHolder>();
        List<ClickableHolder> guestCards = new List<ClickableHolder>();
        List<ClickableHolder> infoCards = new List<ClickableHolder>();
        List<ClickableHolder> johannCards = new List<ClickableHolder>();
        
        foreach (var card in ClickableStorage.Instance._clickedQuestion)
        {
            if (card.slave)
            {
                slaveCards.Add(card);
            }
            else if (card.guest)
            {
                guestCards.Add(card);
            }
            else if (card.info)
            {
                infoCards.Add(card);
            }
            else if (card.johann)
            {
                johannCards.Add(card);
            }
        }
        
        CreateInfoPagesOfType(johannCards, "Johann Mauritz");
        CreateInfoPagesOfType(guestCards, "Guest cards");
        CreateInfoPagesOfType(slaveCards, "Slave cards");
        CreateInfoPagesOfType(infoCards, "Extra Info cards");
        
    }
    
    private void CreateInfoPagesOfType(List<ClickableHolder> cards, string pageTitle)
    {
        if (cards.Count == 0)
        {
            return;
        }

        int maxCardsPerPage = 4;
        int pageCount = Mathf.CeilToInt((float)cards.Count / maxCardsPerPage);

        for (int i = 0; i < pageCount; i++)
        {
            var newPage = Instantiate(infoPagePrefab, book);
            menuManager.pages.Add(newPage);

            var infoPage = newPage.GetComponent<InfoPage>();
            infoPage.title.text = pageTitle;
            
            int startIndex = i * maxCardsPerPage;
            int endIndex = Mathf.Min(startIndex + maxCardsPerPage, cards.Count);

            for (int j = startIndex; j < endIndex; j++)
            {
                //use different card prefab for each card
                int prefabIndex = j % 4 + 1; // Calculate the prefab index (1 to 4)

                GameObject prefabToInstantiate;

                switch(prefabIndex){
                    case 1:
                        prefabToInstantiate = infoCard1;
                        break;
                    case 2:
                        prefabToInstantiate = infoCard2;
                        break;
                    case 3:
                        prefabToInstantiate = infoCard3;
                        break;
                    case 4:
                        prefabToInstantiate = infoCard4;
                        break;
                    default:
                        prefabToInstantiate = infoCard1; 
                        break;
                }
                
                //var cardObject = Instantiate(j < startIndex + 2 ? infoCardPrefab : infoCardPrefabFlipped, infoPage.infoHolder);
                var cardObject = Instantiate(prefabToInstantiate, infoPage.infoHolder);
                var card = cardObject.GetComponent<InfoCard>();

                string cardDescription = cards[j].Description;
                string cardTitle = cards[j].Title;
                //set placholder image first
                card.SetContent(placeHolderImage, cardDescription, cardTitle);

                //if card has image then set that image
                if(cards[j].Image != null){
                    Sprite cardImage = cards[j].Image;
                    card.SetContent(cardImage, cardDescription, cardTitle);
                }
                
                cardObject.SetActive(true);
            }

            newPage.SetActive(false);
            infoPage.gameObject.SetActive(false);
            menuManager.ShowCurrentPage();

        }
    }

    private void ClearInfoPages()
    {
        // Start the loop from index 3 to exclude the first three pages
        for (int i = 3; i < menuManager.pages.Count; i++)
        {
            Destroy(menuManager.pages[i]);
        }

        // Remove the dynamically created pages, starting from index 3
        menuManager.pages.RemoveRange(3, menuManager.pages.Count - 3);
    }
    
}



