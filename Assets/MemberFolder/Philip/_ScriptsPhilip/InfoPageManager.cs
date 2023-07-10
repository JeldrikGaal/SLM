using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfoPageManager : MonoBehaviour
{
    public List<GameObject> infoPagePrefabs;
    public GameObject infoPagePrefab;

    private bool toggled = false;

    public GameObject johannPage;

    public Transform book;
    public GameObject infoCard1;
    public GameObject infoCard2;
    public GameObject infoCard3;
    public GameObject infoCard4;

    public GameObject cardPage1;
    public GameObject cardPage2;
    public GameObject cardPage3;

    public Sprite placeHolderImage;

    public MenuManager menuManager;

    private void Start()
    {
        StartCoroutine(StartDelay());
        
        if (ClickableStorage.Instance == null) {return;}
        StartCoroutine(AddInfoPagesCoroutine());
    }

    private IEnumerator AddInfoPagesCoroutine()
    {
        // Wait for a short delay to ensure the storage is properly initialized
        yield return new WaitForSeconds(0.1f);

        CreateInfoPages();

        if (ClickableStorage.Instance._clickedQuestion.Count > 0)
        {
            if (GameManager.Instance.minigame1Complete && !toggled)
            {
                //menuManager.infoPopup.SetActive(true);
                toggled = true;
            }
        }
        
        
        
    }
    
    private IEnumerator StartDelay()
    {
        // Wait for a short delay to ensure the storage is properly initialized
        yield return new WaitForSeconds(0.2f);

        AddMG2Pages();
    }

    private void AddMG2Pages()
    {
        if (GameManager.Instance.minigame2Complete)
        {
            var newPage1 = Instantiate(cardPage1, book);
            menuManager.pages.Add(newPage1);
            
            var newPage2 = Instantiate(cardPage2, book);
            menuManager.pages.Add(newPage2);
            
            var newPage3 = Instantiate(cardPage3, book);
            menuManager.pages.Add(newPage3);
        }

        menuManager.ShowCurrentPage();
    }

    private void CreateInfoPages()
    {
        ClearInfoPages();
        
        //combine ClickableInfo and ClickableQuestion into one list
        List<ClickableHolder> combinedList = ClickableStorage.Instance._clickedQuestion.Concat(ClickableStorage.Instance._clickedInfo).ToList();
        
        List<ClickableHolder> slaveCards = new List<ClickableHolder>();
        List<ClickableHolder> guestCards = new List<ClickableHolder>();
        List<ClickableHolder> infoCards = new List<ClickableHolder>();
        //List<ClickableHolder> johannCards = new List<ClickableHolder>();
        
        foreach (var card in combinedList)
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
        }
        
        //check cards if johann has been found
        bool hasJohannTaggedCards = ClickableStorage.Instance._clickedQuestion.Any(card => card.johann);

        if (hasJohannTaggedCards)
        {
            var newPage = Instantiate(johannPage, book);
            newPage.SetActive(false);
            menuManager.pages.Add(newPage);
            menuManager.ShowCurrentPage();
        }
        
        //CreateInfoPagesOfType(johannCards, "Johann Mauritz");
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



