using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FYP_project;
public class HandManager : MonoBehaviour
{
    //public Card cardData;
    public DeckManager deckManager;
    public GameObject cardPrefab; //Assign card prefab in inspector
    public Transform handTransform; //Root of the hand position
    public float fanSpread = 0f;
    public float cardSpacing = 157f;

    public float verticalSpacing = 0f;
    public List<GameObject>cardInHand = new List<GameObject>(); //Hold a list of card objects in hand
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddCardToHand(Card cardData)
    {
        bool playerTurn = GameObject.Find("EndButton").GetComponent<BattleSystem>().playerTurn;
        if(playerTurn)
        {
        //Instaniate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        //Set the CardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.name = cardData.name;
        cardInHand.Add(newCard);

        UpdateHandVisuals();
        }
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardInHand.Count;

        if(cardCount == 1)
        {
            cardInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for(int i=0; i < cardCount;i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1)/ 2f));
            cardInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);     //rotate cards

            float horizonalOffset = (cardSpacing * (i - (cardCount - 1)/ 2f));  // = i * cardSpacing          // move cards left or right
            float normalizedPosition = (2f * i/ (cardCount - 1) -1f);       //Normalize card position between -1,1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);   // = i * verticalSpacing     //move cards up and down
            
            //Set card position
            cardInHand[i].transform.localPosition = new Vector3(horizonalOffset, verticalOffset, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHandVisuals();
    }
    public void DrawCard()
    {
        //AddCardToHand();
    }
}
