using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public int num_core,max_core,action,value;
    private int castCard;
    public bool playerTurn=true,deleteBool,BTreeCorrect;
    private TMP_Text numCoreText,turnText,winText,cardLeftText;
    public TMP_Text playerDefenseValue,valueText,defenseValue;
    public Image playerDefenseImage;
    public string lastCardType,lastCardName;
    private Image defenseImage,turn,winImage,cardLeftImage;
    Image attackIcon,defenseIcon;
 
    void DecideAction()
    {
        action=Random.Range(1,10); //action for enemy: 1 is attack, action 2 is defense
        if(action>5)
        {
            attackIcon.enabled = true;
            defenseIcon.enabled = false;
            if(SceneManager.GetActiveScene().name=="Combat")
            value = 6;              //Attack value
            else if(SceneManager.GetActiveScene().name=="MiniBoss")
            value = 11;
            else if(SceneManager.GetActiveScene().name=="Boss")
            value = 17;
        }
        else
        {
            attackIcon.enabled = false;
            defenseIcon.enabled = true;
            if(SceneManager.GetActiveScene().name=="Combat")
            value = 4;          //Defense value
            else if(SceneManager.GetActiveScene().name=="MiniBoss")
            value = 11;
            else if(SceneManager.GetActiveScene().name=="Boss")
            value = 15;
        }
        valueText.text = value.ToString();          //copy value to valueText
    }
    public void EndTurn()
    {
        playerTurn=!playerTurn;
        ShowTurn();
        if(playerTurn)
        {
            DecideAction();     //enemy choose action in next turn
            
            num_core = max_core; //Restore core to maximum core
            playerDefenseImage.enabled = false;
            playerDefenseValue.text = null;
        }
        else
        {       //Check whether the blocks are placed correctly in B+ tree, if correct, give 8 extra shield to player
            int.TryParse(GameObject.Find("B+TreeItem").GetComponent<TMP_Text>().text, out int containNo_);
            if(containNo_ > 1)      //If > 1 block in B+ tree, start checking the order of B+ tree
            {   

                GameObject BTreeBg = GameObject.Find("B+TreeBackground");
                for(int i = 0; i < BTreeBg.transform.childCount; i++)
                {
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text,out int mainBlockLeftNo_);
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text,out int mainBlockRightNo_);
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text,out int rightBlockLeftNo_);
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text,out int rightblockRightNo_);
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text,out int leftBlockLeftNo_);
                    int.TryParse(BTreeBg.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_Text>().text,out int leftblockRightNo_);

                    if(mainBlockLeftNo_ != null && mainBlockRightNo_ != null && mainBlockRightNo_ >= mainBlockLeftNo_)
                    {
                        BTreeCorrect = true;
                        if(rightBlockLeftNo_ != null && rightblockRightNo_ != null && rightblockRightNo_ >= rightBlockLeftNo_)
                        {
                            BTreeCorrect= true;
                        }
                    }
                    if(BTreeCorrect)            //if the order is correct, reward 8 extra shield to player before enemy acts 
                    {
                        int.TryParse(playerDefenseValue.text,out int currentDefenseValue);
                        currentDefenseValue += 8;
                        playerDefenseValue.text = currentDefenseValue.ToString();
                        playerDefenseImage.enabled = true;
                    }
                }
                
            }
            if(action>5)
            {
                defenseImage.enabled = false;
                defenseValue.text = "";
                int.TryParse(playerDefenseValue.text,out int playerDefenseValueNo_);
                if(value > playerDefenseValueNo_)
                {
                    value -= playerDefenseValueNo_;
                    playerDefenseImage.enabled = false;
                    playerDefenseValue.text = null;
                    int health = PlayerPrefs.GetInt("currentHealth");       //Get health of player from database
                    health = health - value;
                    PlayerPrefs.SetInt("currentHealth",health);     //Set the current health of player to database
                    GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You receive "+value+" damage from enemy!";    //Display message of being dealt by enemy
                }
                else
                {
                    playerDefenseValueNo_ -= value;
                    playerDefenseValue.text = playerDefenseValueNo_.ToString();
                }
                new WaitForSeconds(1);
                //playerTurn=!playerTurn;
                //ShowTurn();
                
            }
            else
            {
                defenseImage.enabled = true;
                defenseValue.text = value.ToString();
                //new WaitForSeconds(1);
                //playerTurn=!playerTurn;
                //ShowTurn();
            }
                
        }
    }
    void ShowTurn()
    {
        StartCoroutine(DisplayTurn());
    }
    IEnumerator DisplayTurn()               //Function of Displaying what turn is in current status, player or enemy turn
    {
        turn.enabled = true;
        if(playerTurn)
            turnText.text = "Player Turn";
        else
            turnText.text = "Enemy Turn";
        yield return new WaitForSeconds(1);
        turn.enabled = false;
        turnText.text = "";
    }

    void Awake()
    {
        attackIcon = GameObject.Find("AttackIcon").GetComponent<Image>();
        defenseIcon = GameObject.Find("DefenseIcon").GetComponent<Image>();
        valueText = GameObject.Find("Value").GetComponent<TMP_Text>();
        numCoreText = GameObject.Find("NumCore").GetComponent<TMP_Text>();
        turnText = GameObject.Find("TurnText").GetComponent<TMP_Text>();
        defenseImage = GameObject.Find("EnemyDefense").GetComponent<Image>();
        defenseValue = GameObject.Find("EnemyDefenseValue").GetComponent<TMP_Text>();
        playerDefenseImage = GameObject.Find("PlayerDefense").GetComponent<Image>();
        playerDefenseValue = GameObject.Find("PlayerDefenseValue").GetComponent<TMP_Text>();
        turn =  GameObject.Find("Turn").GetComponent<Image>();
        winImage = GameObject.Find("WinButton").GetComponent<Image>();
        winText = GameObject.Find("WinText").GetComponent<TMP_Text>();
        cardLeftImage = GameObject.Find("CardLeft").GetComponent<Image>();
        cardLeftText = GameObject.Find("CardLeftText").GetComponent<TMP_Text>();
        cardLeftImage.enabled = true;
        cardLeftText.enabled = true;
        attackIcon.enabled = true;
        defenseIcon.enabled = true;
        playerDefenseImage.enabled = false;
        defenseImage.enabled = false;
        turn.enabled = false;
        winImage.enabled = false;
        winText.enabled = false;
        ShowTurn();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DecideAction();
        max_core = 4;
        num_core = max_core;      
    }

    // Update is called once per frame
    void Update()
    {   
        numCoreText.text = num_core.ToString()+"/"+max_core.ToString();
        int availableCardLeft = GameObject.Find("DeckManager").GetComponent<DeckManager>().availableDeck.Count;
        int totalCardCount = GameObject.Find("DeckManager").GetComponent<DeckManager>().startDeck.Count;

        cardLeftText.text =availableCardLeft.ToString()+"/"+totalCardCount.ToString();
        if(GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health <= 0)
        {
            winImage.enabled = true;
            winText.enabled = true;
            
            GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You win!";
            /*if(GameObject.Find("WinButton").GetComponent<Button>().onClick())
            {
                GameObject.Find("BackButton").GetComponent<SceneController>().stage++;
            }*/
        }

        //StartCoroutine(ReduceCore());
    }
}
