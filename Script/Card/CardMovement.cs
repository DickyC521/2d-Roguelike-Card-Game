using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using FYP_project;
public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler
{

    [SerializeField] private float _verticalMoveAmount = 50f;
    //[SerializeField] private float _moveTime = 0.1f;
    [SerializeField] private float scaleAmount = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPos;
    private RectTransform rectTransform;
    public Transform hoveringTransform,BTreeTransform;
    private Canvas canvas;
    private Vector2 startPointerPos;
    private Vector3 startPanelPos;
    private Vector3 startScale;
    private int currentStates;
    public int castCard,cardIndex,manaCost,number_core;
    private Vector3 startPos;   //originalPosition
    private GameObject hoveringCard;
    public GameObject endButton,BTreeBlock;
    public List<GameObject> BTreelist = new List<GameObject>();
    public HandManager handManager;
    public DeckManager deckManager;
    public string gameObjectName,cardType;
    //private Vector3 _startPos;
    //private Vector3 _startScale;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        startScale = rectTransform.localScale;
        startPos = rectTransform.localPosition;
        handManager = GameObject.Find("HandManager").GetComponent<HandManager>();
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        hoveringTransform = GameObject.Find("PointeringCardPosition").GetComponent<RectTransform>();
        endButton = GameObject.Find("EndButton");
        BTreeTransform = GameObject.Find("B+TreeBackground").GetComponent<RectTransform>();
    }
    void Start()
    {
        //_startPos = transform.position;
        //_startScale = transform.localScale;
        //BTreelist = GetComponent<BTree>().BTreeBlock;
    }  

    void Update()
    {
        switch(currentStates)
        {
            case 1:
                HandleHoverState();
                break;
            
            case 2:
                if(!Input.GetMouseButton(0))
                    TransitionToState0();
                break;
            case 3:
                HandlePlayState();
                if(!Input.GetMouseButton(0))
                    TransitionToState0();
                break;
        }
    }

    private void TransitionToState0()
    {
        currentStates = 0;
        rectTransform.localScale = startScale;  //Reset Scale
        rectTransform.localPosition = startPos;
    }

    private void HandleHoverState()
    {
        rectTransform.localPosition = startPos + new Vector3(0f, _verticalMoveAmount, 0f);
        rectTransform.localScale = startScale * scaleAmount;

    }
    
    private void HandlePlayState()
    {
        rectTransform.localPosition = playPos;

        if(Input.mousePosition.y < cardPlay.y)
        {
            currentStates = 2;
        }
    }
    /*private IEnumerator MoveCard(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime = Time.deltaTime;

            if(startingAnimation)
            {
                endPosition = _startPos + new Vector3(0f, _verticalMoveAmount, 0f);
                endScale = _startScale * _scaleAmount;
            }

            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            // Calculate the larged amounts
            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));

            // Actually apply the changes to the position and scale
            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Select the card
        //number_core = endButton.GetComponent<BattleSystem>().num_core;
        if(currentStates == 0)
        {
            startPos = rectTransform.localPosition;
            startScale = rectTransform.localScale;
            gameObjectName = name;
            for(int x = 0; x < handManager.cardInHand.Count; x++)
            {
                if(handManager.cardInHand[x].name == gameObjectName)
                {
                    hoveringCard = Instantiate(handManager.cardInHand[x], hoveringTransform.position, Quaternion.identity, hoveringTransform);      //Display the holding card on screen by creating new gameobject
                    hoveringCard.name = "HoveringCard";
                    break;
                }
            }
            currentStates = 1;
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        //eventData.selectedObject = null;
        //gameObject = null;
        if(currentStates == 1)
        {
            castCard = 0;
            Destroy(GameObject.Find("HoveringCard"));          //Destroy clone of holding card if no card is holding 
            gameObjectName = null;

            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //gameObjectName = name;
        
        Destroy(hoveringCard);  //Remove the holding card when no cards are holding on the mouse
                
        number_core = endButton.GetComponent<BattleSystem>().num_core;      //Get the current core number if a card is attempting to be casted
        for(int x = 0; x < handManager.cardInHand.Count; x++)
        {
            if(handManager.cardInHand[x].name == gameObjectName)        //search the card with same name of holding card
            {
                cardIndex = x;
                cardType = handManager.cardInHand[x].GetComponent<CardDisplay>().typeText.text;
                string manaCostText = handManager.cardInHand[x].GetComponent<CardDisplay>().ManaCostText.text;      //Get string of manacost of hovering card
                manaCost = int.Parse(manaCostText);     //change type of manacost from string to integer
            }
        }
        castCard = 1;
        if(number_core > manaCost)
        {
            endButton.GetComponent<BattleSystem>().lastCardType = cardType;         //save card type and name for delete and insert type of card
            endButton.GetComponent<BattleSystem>().lastCardName = gameObjectName;
            for(int x = 0; x < handManager.cardInHand.Count; x++)
            {
                if(handManager.cardInHand[x].name == gameObjectName)
                {
                    if(cardType == "Insert")
                    {
                        GameObject block = Instantiate(BTreeBlock, GameObject.Find("BlockSpawnPoint").GetComponent<RectTransform>().position, Quaternion.identity, BTreeTransform);         //create new block in B+ tree
                        int randomNum = Random.Range(0, 100);           //Randomly assign number to the block contained
                        GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Add(randomNum);
                        GameObject.Find("BlockList").GetComponent<TMP_Text>().text = "Block Numbers:";
                        for(int i = 0; i < GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Count; i++)     //Update message of displaying block numbers in B+ tree
                        {
                            GameObject.Find("BlockList").GetComponent<TMP_Text>().text = GameObject.Find("BlockList").GetComponent<TMP_Text>().text +GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List[i]+", ";
                        }
                        block.name = "Block"+randomNum.ToString();          //Rename the block
                        for(int i = 0; i<block.transform.GetChild(0).childCount; i++)
                        {
                            block.transform.GetChild(0).GetChild(i).GetComponent<Image>().enabled = false;
                        }
                        //for(int i = 6; i<block.transform.GetChild(0).childCount; i++)
                        //{
                        //    block.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<TMP_Text>().enabled = false;
                        //}
                        //block.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;        //set the block is disabled to be displayed
                        block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = randomNum.ToString();       //Assign the number as TMP_text in the block
                        block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().enabled = false;
                        block.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text="1";
                        
                        GameObject.Find("B+TreeItem").GetComponent<TMP_Text>().text = GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Count.ToString();
                        //GetComponent<BTree>().BTreeBlock.Add(block);
                        //block.enabled = false;
                    }
                    else if(cardType == "Delete")       //Open B+ tree and choose 1 block to delete
                    {
                        if(GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health <= 0)
                        {
                            GameObject.Find("RewardBackground").GetComponent<RewardSystem>().GetReward();
                            break;          //stop when enemy health is <= 0 
                        }
                        else
                        {
                        int.TryParse(GameObject.Find("B+TreeItem").GetComponent<TMP_Text>().text, out int BTreeContainNo_);
                        if(BTreeContainNo_ > 0)     //if there is > 1 block contained in B+ tree, start the deletion of block
                        {
                            endButton.GetComponent<BattleSystem>().deleteBool = true;       //Boolean for allowing funtion of Delete card in BTreeSystem Script
                            GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "Choose a block to delete";
                            GameObject.Find("B+TreeBackground").GetComponent<Image>().enabled = true;
                            GameObject BTreeBg = GameObject.Find("B+TreeBackground");
                            for(int i = 0; i < BTreeBg.transform.childCount; i++)           //Display blocks and nuumbers contained in the block
                            {
                                for(int z = 0; z < 6;z++)
                                {
                                    BTreeBg.transform.GetChild(i).GetChild(0).GetChild(z).GetComponent<Image>().enabled = true;
                                    if(z < 3)
                                    {
                                        BTreeBg.transform.GetChild(i).GetChild(0).GetChild(z).GetChild(0).GetComponent<TMP_Text>().enabled = true;
                                        BTreeBg.transform.GetChild(i).GetChild(0).GetChild(z).GetChild(1).GetComponent<TMP_Text>().enabled = true;
                                    }
                                }    
                            }
                        }
                        }   
                    }   
                    deckManager.disposalDeck.Add(deckManager.cardInHand[x]);         //Add casted card to disposal deck
                    deckManager.cardInHand.RemoveAt(x);         //Remove casted card in cardInHand deck
                    break;      //stop if another card with same name is found
                }       
            }
        }
        if(currentStates == 1)
        {
            currentStates = 2;
            //gameObjectName = name;
            

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out startPointerPos);
            startPanelPos = rectTransform.localPosition;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        /*if (currentStates == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Input.mousePosition;

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentStates = 3;
                    rectTransform.localPosition = playPos;
                }
            }
        }*/
    }
    
}
