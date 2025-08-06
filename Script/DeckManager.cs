using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FYP_project;
public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public List<Card> startDeck = new List<Card>();
    public List<Card> rewardDeck = new List<Card>();
    public List<Card> rewardList = new List<Card>();
    public List<Card> availableDeck = new List<Card>();
    public List<Card> cardInHand = new List<Card>();
    public List<Card> disposalDeck = new List<Card>();
    private int currentIndex = 0;
    public int currentNoCard;
    private bool drawCardBool = true;
    public HandManager handManager;

    public void DrawCard(HandManager handManager)
    {
        if(allCards.Count == 0)
            return;
        currentIndex = Random.Range(0, availableDeck.Count);      //Random draw a card and add to hand in deck ||| availableDeck to startDeck
        Card nextCard = availableDeck[currentIndex];            //Card nextCard = availableDeck[currentIndex];   ||| startDeck[currentIndex]
        handManager.AddCardToHand(nextCard);
        cardInHand.Add(nextCard);
        availableDeck.Remove(nextCard);         //Remove card just drawn from the card deck
    }
    // Start is called before the first frame update
    void Awake()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");        //load all card assets from Card folder 
        allCards.AddRange(cards);
        rewardDeck.AddRange(cards);
        rewardDeck.RemoveAt(rewardDeck.Count-1);
        rewardDeck.RemoveAt(0);
    }
    void Start()
    {
        
        //Add to the loaded assets to the allCards list

        for(int i=0; i< 3; i++)
        {
            int index = Random.Range(0,rewardDeck.Count);
            rewardList.Add(rewardDeck[index]);
        }
        availableDeck.AddRange(allCards);
        availableDeck.Add(allCards[allCards.Count-1]);
        availableDeck.Add(allCards[5]);
        availableDeck.Add(allCards[6]);    
        startDeck.AddRange(availableDeck);  
        //availableDeck.AddRange(cards);
        for(int i = 0 ; i < 4; i++)
            DrawCard(handManager);
        currentNoCard = cardInHand.Count;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(GameObject.Find("EndButton").GetComponent<BattleSystem>().playerTurn && drawCardBool)   //Draw card if player turn
        {
            if(availableDeck.Count == 0)
            {
                availableDeck.AddRange(disposalDeck);
                disposalDeck.Clear();
            }
            DrawCard(handManager);
            drawCardBool = !drawCardBool;
        }
        if(GameObject.Find("EndButton").GetComponent<BattleSystem>().playerTurn == false && !drawCardBool)     //Reset drawCardBool to false
        {
            drawCardBool = !drawCardBool;
        }
        
    }
}
