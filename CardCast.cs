using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FYP_project;

public class CardCast : MonoBehaviour
{
    private string castCardName,cardType;
    private HandManager handManager;
    private DeckManager deckManager;
    public Transform pointeringCard;
    public GameObject gameButton;
    public int core,damage;
    private int cost,defenseValue;
    private TMP_Text systemText;
    // Start is called before the first frame update
    void Start()
    {
        handManager = FindObjectOfType<HandManager>();
        deckManager = FindObjectOfType<DeckManager>();
        gameButton = GameObject.Find("EndButton");
        systemText = GameObject.Find("SystemMessage").GetComponent<TMP_Text>();
    }

    public void GetCastCard()
    {
        castCardName = GetComponent<CardMovement>().gameObjectName;
        cardType = GetComponent<CardMovement>().cardType;
    }
    public IEnumerator RemoveCastedCard()
    {
        
        yield return 0;
        handManager.cardInHand.Remove(GameObject.Find(castCardName));       //Remove casted card in hand
        Destroy(GameObject.Find(castCardName));         //Remove gameObject in screen
    }
    public IEnumerator DisplayMessage(string cardName,bool successfulCast)
    {   
        systemText.enabled = true;              //enable to show message of casting card on screen
        if(successfulCast == true)
        {
            systemText.text = cardName+" Card is casted.";           //Set message with "Card is casted" to display cast card successfully
            if(cardType == "delete")
            {
                systemText.text = systemText.text + " Please delete a block in the tree.";
            }
        }
        else
        {
            systemText.text = "Not enough core to cast "+cardName+" Card";      //Set error message about not enough core to cast card
        }
        
        yield return new WaitForSeconds(1);
        systemText.text = "";               //Clear the system message and disable the system message on screen
        systemText.enabled = false;
    }
    public int EffectOnCard()
    {
        switch(castCardName)                    //check the core cost by identifying the name of hovering card
        {
            case "Claw":
            {   
                cost = 1;
                damage = 5;
                if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)     //If enemy has defense action in last turn, it has defense value on reducing damage from player
                {
                    int.TryParse(GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text,out int enemyCurrentDefenseValue);
                    if(damage > enemyCurrentDefenseValue)
                    {
                        GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= (damage - enemyCurrentDefenseValue);    //Get the real damage value by deducting current defense value of enemy
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = null;
                        GameObject.Find("EnemyDefense").GetComponent<Image>().enabled = false;
                    }
                    else
                    {
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = (enemyCurrentDefenseValue - damage).ToString();
                    }  
                }
                else
                {
                    GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= damage;
                }
                
                break;
            }
            case "Double Claw":
            {
                cost = 1;
                damage = 6;
                if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)
                {
                    int.TryParse(GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text,out int enemyCurrentDefenseValue);
                    if(damage > enemyCurrentDefenseValue)
                    {
                        GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= (damage - enemyCurrentDefenseValue);
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = null;
                        GameObject.Find("EnemyDefense").GetComponent<Image>().enabled = false;
                    }
                    else
                    {
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = (enemyCurrentDefenseValue - damage).ToString();
                    }   
                }
                else
                {
                    GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= damage;
                }
                //GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= 8;
                //Debug.Log(GetComponent<BattleSystem>().num_core);

                break;
            }
            case "Duplication":         //increase 2 cores in current player turn
            {
                cost = 1;
                GameObject.Find("EndButton").GetComponent<BattleSystem>().num_core += 2;

                break;
            }
            case "Exhaust":
            {
                cost = 1;
                
                if(GameObject.Find("EndButton").GetComponent<BattleSystem>().action > 5)        //If enemy is ready to attack player in enemy turn, 
                                                                                                 //reduce the damage of enemy by half
                {
                    GameObject.Find("EndButton").GetComponent<BattleSystem>().value /= 2;
                    GameObject.Find("EndButton").GetComponent<BattleSystem>().valueText.text = GameObject.Find("EndButton").GetComponent<BattleSystem>().value.ToString();
                }
                break;
            }
            case "Find":
            {
                cost = 1;
                int randomNum1 = Random.Range(0, 100);      //generate 2 numbers randomly for Finding blocks with range of numbers
                int randomNum2 = Random.Range(0, 100);
                if(randomNum1 > randomNum2)             //exchange the position if 2nd number > 1st number
                {                           
                    int temp  = randomNum1;
                    randomNum1 = randomNum2;
                    randomNum2 = temp;
                }
                systemText.text = "The random block is "+randomNum1 + " to "+randomNum2;
                for(int i = 0; i < GameObject.Find("B+TreeBackground").transform.childCount; i++)       //Find all blocks in B+ tree for fulfilling the condition
                {
                    string blockNo_Text = GameObject.Find("B+TreeBackground").transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text;
                    int.TryParse(blockNo_Text,out int blockNo_);        //Change the block number from text to integer
                    if(blockNo_ >= randomNum1 && blockNo_ <= randomNum2)        //if the condition is satisfied, deal 8 damage to enemy and receive 8 shield value to player
                    {
                        damage = 8;
                        if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)
                        {
                            int.TryParse(GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text,out int enemyCurrentDefenseValue);
                            if(damage > enemyCurrentDefenseValue)
                            {
                                GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= (damage - enemyCurrentDefenseValue);
                                GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = null;
                                GameObject.Find("EnemyDefense").GetComponent<Image>().enabled = false;
                            }
                            else
                            {
                                GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = (enemyCurrentDefenseValue - damage).ToString();
                            }   
                        }
                        else
                        {
                            GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= damage;
                        }
                        int.TryParse(GameObject.Find("PlayerDefenseValue").GetComponent<TMP_Text>().text,out int currentDefenseValue);
                        defenseValue = currentDefenseValue;
                        defenseValue +=8;
                        systemText.text = systemText.text + ". You have the block exists in B+ Tree!";
                        GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseValue.text = defenseValue.ToString();
                        GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseImage.enabled = true;
                    }
                }
                //if contains, receive shield and do damage to enemy, both 8
                //battleSystem.num_core -= 1;
                break;
            }
            case "Golden Shield":           
            {
                int.TryParse(GameObject.Find("PlayerDefenseValue").GetComponent<TMP_Text>().text,out int currentDefenseValue);
                defenseValue = currentDefenseValue;
                cost = 2;
                defenseValue +=12;
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseValue.text = defenseValue.ToString();
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseImage.enabled = true;
                //battleSystem.num_core -= 2;
                break;
            }
            case "Infinite Shield":
            {
                int.TryParse(GameObject.Find("PlayerDefenseValue").GetComponent<TMP_Text>().text,out int currentDefenseValue);
                defenseValue = currentDefenseValue;
                cost = 2;
                int.TryParse(GameObject.Find("B+TreeItem").GetComponent<TMP_Text>().text,out int BTreeContainNo_);
                int blockWithShieldValue = BTreeContainNo_ * 4;
                defenseValue += blockWithShieldValue; 
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseValue.text = defenseValue.ToString();
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseImage.enabled = true;
                //battleSystem.num_core -= 2;
                break;
            }
            case "Power Strike":
            {
                cost = 3;
                damage = 20;
                if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)
                {
                    int.TryParse(GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text,out int enemyCurrentDefenseValue);
                    if(damage > enemyCurrentDefenseValue)
                    {
                        GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= (damage - enemyCurrentDefenseValue);
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = null;
                        GameObject.Find("EnemyDefense").GetComponent<Image>().enabled = false;
                    }  
                    else
                    {
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = (enemyCurrentDefenseValue - damage).ToString();
                    } 
                }
                else
                {
                    GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= damage;
                }
                //GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= 20;
                //battleSystem.num_core -= 3;
                break;
            }
            
            case "Rock Attack":
            {
                cost = 2;
                damage = 10;
                if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)
                {
                    int.TryParse(GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text,out int enemyCurrentDefenseValue);
                    if(damage > enemyCurrentDefenseValue)
                    {
                        GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= (damage - enemyCurrentDefenseValue);
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = null;
                        GameObject.Find("EnemyDefense").GetComponent<Image>().enabled = false;
                    }  
                    else
                    {
                        GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>().text = (enemyCurrentDefenseValue - damage).ToString();
                    } 
                }
                else
                {
                    GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health -= damage;
                }

                break;
            }
            case "Shield":
            {
                int.TryParse(GameObject.Find("PlayerDefenseValue").GetComponent<TMP_Text>().text,out int currentDefenseValue);
                defenseValue = currentDefenseValue;
                cost = 1;
                defenseValue += 5;
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseValue.text = defenseValue.ToString();;
                GameObject.Find("EndButton").GetComponent<BattleSystem>().playerDefenseImage.enabled = true;
                //battleSystem.num_core -= 1;
                break;
            }
        }
        return cost;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<CardMovement>().castCard == 1)
        {
            GetCastCard();
            //core = EffectOnCard();
            if(gameButton.GetComponent<BattleSystem>().num_core >= core)          // if enoguh core to cast card, start to cast and destroy the gameobejct of corresponding card
            {   
                 StartCoroutine(DisplayMessage(castCardName,true));          //Display system message on casting card successfully
                core = EffectOnCard();
                StartCoroutine(RemoveCastedCard());
                gameButton.GetComponent<BattleSystem>().num_core -= core;       //Spend the core to cast the card

                GetComponent<CardMovement>().castCard = 0;                  
            }
            else
            {
                StartCoroutine(DisplayMessage(castCardName,false));
                GetComponent<CardMovement>().castCard = 0;
                //Debug.Log("Not enough core");
            }
        }
        
    }
}
