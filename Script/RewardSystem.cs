using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using FYP_project;
public class RewardSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private float scaleAmount = 1.2f;
    public List<GameObject> reward = new List<GameObject>();
    public List<Card> rewardList = new List<Card>();
    public List<int> rewardIndex = new List<int>();
    private RectTransform rectTransform;
    private int currentStates;
    private Vector3 startScale;
    public string gameObjectName;
    public float cardSpacing = 300f;
    public GameObject cardPrefab;
    public Transform handTransform; 
    public DeckManager deckManager;
    // Start is called before the first frame update
    /*
    after defeat enemy, 3 cards can be chosen and coins as reward
    3 cards: random chosen in the reward deck, after chosen, add to startDeck, upper middle
    coin: pick up for buying cards in shop, show how many coins are rewarded
    coins shows up next to health 
    */
    private void TransitionToState0()
    {
        currentStates = 0;
        rectTransform.localScale = startScale;  //Reset Scale
    }
    private void HandleHoverState()
    {
        //rectTransform.localPosition = startPos + new Vector3(0f, _verticalMoveAmount, 0f);
        rectTransform.localScale = startScale * scaleAmount;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentStates == 0)
        {
            startScale = rectTransform.localScale;
            gameObjectName = name;
            
            currentStates = 1;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(currentStates == 1)
        {
            gameObjectName = null;

            TransitionToState0();
    }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Card card = GameObject.Find(gameObjectName).GetComponent<CardDisplay>().cardData;
        GameObject.Find("DeckManager").GetComponent<DeckManager>().startDeck.Add(card);
        Destroy(this.gameObject);
    }
    public void UpdateHandVisuals()
    {
        int count = reward.Count;
        for(int i=0; i < count;i++)
        {
            float horizonalOffset = (cardSpacing * (i - (count - 1)/ 2f));  // = i * cardSpacing          // move cards left or right 
            
            //Set card position
            reward[i].transform.localPosition = new Vector3(horizonalOffset, 0f, 0f);
        }
    }
    public void AddCardToReward(Card cardData)
    {
            //Instaniate card
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            //Set the CardData of the instantiated card
            newCard.GetComponent<CardCast>().enabled = false;
            newCard.GetComponent<CardDisplay>().cardData = cardData;
            newCard.name = cardData.name;
            reward.Add(newCard);
            UpdateHandVisuals();
            
        }
    public void GetReward()
    {
        rewardList = deckManager.GetComponent<DeckManager>().rewardList;
    }
    void Start()
    {
        handTransform = GameObject.Find("RewardPosition").transform;
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        GetReward();
        for(int x = 0; x<3; x++)
        {
            AddCardToReward(rewardList[x]);
        }
                //Card card = GameObject.Find("DeckManager").GetComponent<DeckManager>().rewardDeck[index];
                //rewardList.Add(card);
                //AddCardToReward(card);
                //newCard.GetComponent<CardDisplay>().cardData = cardData;
                //newCard.name = cardData.name;
                //reward.Add(newCard);
                
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health <= 0)
        {
            //GameObject.Find("RewardBackground").GetComponent<Image>().enabled = true;
            
        }
        switch(currentStates)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                if(!Input.GetMouseButton(0))
                    TransitionToState0();
                break;
        }
    }
}
