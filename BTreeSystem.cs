using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using FYP_project;
public class BTreeSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    private Canvas canvas;
    private Vector2 startPointerPos;
    private RectTransform rectTransform;
    private GameObject BTreeBackground;
    private Vector3 startPanelPos;
    string blockName,blockStoreNo_;
    //GameObject gameObject;
    GameObject block;
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        BTreeBackground = GameObject.Find("B+TreeBackground");
        //gameObject = this.GetComponent<BTree>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //blockName = name;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //blockName = null;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out startPointerPos);
        startPanelPos = rectTransform.localPosition;            //Get position of selected block
        blockName = name;
        block = GameObject.Find(blockName);

        if(GameObject.Find("EndButton").GetComponent<BattleSystem>().deleteBool)      //Allowing for doing effect of cards if any blocks contained in B+ tree
        {
            int damage;
            int.TryParse(block.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text,out int containNo_);
            if(containNo_ == 1)
            {
                Destroy(block);
                GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Clear();
            }
            else
            {
            for(int i = 1; i <= 6; i++)
            {
                GameObject targetBlock = block.transform.GetChild(i).gameObject;
                if(block.transform.position.x > targetBlock.transform.position.x - 10 && block.transform.position.x < targetBlock.transform.position.x+10)          //check dragging block x coordinate is near to target block x coordinate
                    {
                        if(block.transform.position.y > targetBlock.transform.position.y -10 && block.transform.position.y < targetBlock.transform.position.y+10)        //check dragging block y coordinate is near to target block y coordinate
                        {
                            Debug.Log(targetBlock.transform.position);
                            Debug.Log("Click "+i);
                            if(i>4)
                            {
                                blockStoreNo_ = block.transform.GetChild(0).GetChild(2).GetChild(i%2).GetComponent<TMP_Text>().text;
                                
                                block.transform.GetChild(0).GetChild(2).GetChild(i%2).GetComponent<TMP_Text>().text = "";
                            }
                            else if(i > 2)
                            {
                                blockStoreNo_ = block.transform.GetChild(0).GetChild(1).GetChild(i%2).GetComponent<TMP_Text>().text;
                                
                                block.transform.GetChild(0).GetChild(1).GetChild(i%2).GetComponent<TMP_Text>().text = "";
                            }
                            else
                            {
                                blockStoreNo_ = block.transform.GetChild(0).GetChild(0).GetChild(i%2).GetComponent<TMP_Text>().text;
                                
                                block.transform.GetChild(0).GetChild(0).GetChild(i%2).GetComponent<TMP_Text>().text = "";

                            }
                                //BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(x/2-1).GetChild(x%2).GetComponent<TMP_Text>().text = insertNo_;
                                containNo_--;
                                block.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = containNo_.ToString();
                                
                            }
                        }
            }
            
            int.TryParse(blockStoreNo_,out int blockNo);
            GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Remove(blockNo);
            }
            GameObject.Find("B+TreeItem").GetComponent<TMP_Text>().text = GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Count.ToString();//Destroy the block after cast Delete card
            for(int x = 0; x< block.transform.GetChild(0).childCount; x++)
                    {
                        Image image = block.transform.GetChild(0).GetChild(x).GetComponent<Image>();
                        image.enabled = false;
                        if(x < 3)
                        {
                            for(int y = 0; y < 3; y++)
                        {
                            block.transform.GetChild(0).GetChild(y).GetChild(0).GetComponent<TMP_Text>().enabled = false;
                            block.transform.GetChild(0).GetChild(y).GetChild(1).GetComponent<TMP_Text>().enabled = false;
                        }
                        }
                    
                    }
            BTreeBackground.GetComponent<Image>().enabled = false;


            switch(GameObject.Find("EndButton").GetComponent<BattleSystem>().lastCardName)      //check the card name for different damage value to enemy
            {
                case "Claw":
                {
                    damage = 3;                    
                    //Delete the numbers stored in the main block
                    
                    //Update the block list message 
                    GameObject.Find("BlockList").GetComponent<TMP_Text>().text = "Block Numbers:";    
                    for(int x = 0; x < GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List.Count; x++)
                    {
                        GameObject.Find("BlockList").GetComponent<TMP_Text>().text = GameObject.Find("BlockList").GetComponent<TMP_Text>().text +GameObject.Find("B+Tree").GetComponent<BTree>().blockNo_List[x]+", ";
                    }
                    
                    if(GameObject.Find("EndButton").GetComponent<BattleSystem>().defenseValue.text != null)     //check if enemy has defense value
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
                    GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You deal 5 +"+damage+" damage to enemy!";
                    break;
                }

                case "Double Claw":
                {
                    damage = 4;                

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
                    GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You deal 6 +"+damage+" damage to enemy!";
                    break;
                }

                case "Rock Attack":
                {
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
                    GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You deal 10 +"+damage+" damage to enemy!";
                    break;
                }

                case "Power Strike":
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
                    GameObject.Find("SystemMessage").GetComponent<TMP_Text>().text = "You deal 20 +"+damage+" damage to enemy!";
                    break;
                }
            }
            
            GameObject.Find("EndButton").GetComponent<BattleSystem>().deleteBool = false;       //Set Boolean of delete for avoiding error on next action
            if(GameObject.Find("EnemyHealth").GetComponent<EnemyHealth>().health <= 0)
            {
                GameObject.Find("RewardBackground").GetComponent<RewardSystem>().GetReward();
            }
        }
        //Debug.Log(blockName);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                //Debug.Log(name);
                rectTransform.position = Input.mousePosition;       //move blocks in B+ tree by dragging
            }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 selectedBlockPosition, localBlockPosition;
        selectedBlockPosition = block.GetComponent<RectTransform>().anchoredPosition;
        block.transform.position = Input.mousePosition;
        //Debug.Log(localPointerPosition);
        for(int i = 0; i < BTreeBackground.transform.childCount; i++)
        {
            if(name != BTreeBackground.transform.GetChild(i).name)
            {
                localBlockPosition = BTreeBackground.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;     //Get position of blocks in screen
                int.TryParse(BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text,out int containNo_);
                string insertNo_ = block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text;
                if(containNo_ < 2)
                {
                    if(selectedBlockPosition.x > localBlockPosition.x - 100 && selectedBlockPosition.x < localBlockPosition.x+100)          //check dragging block x coordinate is near to target block x coordinate
                    {
                        if(selectedBlockPosition.y > localBlockPosition.y - 50 && selectedBlockPosition.y < localBlockPosition.y+50)        //check dragging block y coordinate is near to target block y coordinate
                        {
                            BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = insertNo_;
                            containNo_++;
                            BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = containNo_.ToString();
                                for(int y = 0; y < 3; y++)
                                {
                                    Image image = BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
                                    image.enabled = false;
                                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(y).GetChild(0).GetComponent<TMP_Text>().enabled = false;
                                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(y).GetChild(1).GetComponent<TMP_Text>().enabled = false;
                                }
                                
                            
                            Destroy(block);         //destroy the original dragging block
                            BTreeBackground.GetComponent<Image>().enabled = false;
                        }
                    }
                    
                }
                else
                {
                    Debug.Log(block.transform.position);
                    for(int x = 1; x <= 6; x++)
                    {
                        GameObject targetBlock = BTreeBackground.transform.GetChild(i).GetChild(x).gameObject;
                        if(block.transform.position.x > targetBlock.transform.position.x - 100 && block.transform.position.x < targetBlock.transform.position.x+100)          //check dragging block x coordinate is near to target block x coordinate
                        {
                            if(block.transform.position.y > targetBlock.transform.position.y - 50 && block.transform.position.y < targetBlock.transform.position.y+50)        //check dragging block y coordinate is near to target block y coordinate
                            {
                                Debug.Log(targetBlock.transform.position);
                                Debug.Log("Click "+x);
                                if(x>4)
                                {
                                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(x%2).GetComponent<TMP_Text>().text = insertNo_;
                                }
                                else if(x > 2)
                                {
                                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(x%2).GetComponent<TMP_Text>().text = insertNo_;

                                }
                                else
                                {
                                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(x%2).GetComponent<TMP_Text>().text = insertNo_;

                                }
                                //BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(x/2-1).GetChild(x%2).GetComponent<TMP_Text>().text = insertNo_;
                                containNo_++;
                                BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = containNo_.ToString();
                                
                            }
                        }
                    }
                    containNo_++;
                    BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = containNo_.ToString();
                    for(int x = 0; x< BTreeBackground.transform.GetChild(i).GetChild(0).childCount; x++)
                    {
                        Image image = BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(x).GetComponent<Image>();
                        image.enabled = false;
                        if(x < 3)
                        {
                            for(int y = 0; y < 3; y++)
                        {
                            BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(y).GetChild(0).GetComponent<TMP_Text>().enabled = false;
                            BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(y).GetChild(1).GetComponent<TMP_Text>().enabled = false;
                        }
                        }
                    
                    }
                    Destroy(block);         //destroy the original dragging block
                    BTreeBackground.GetComponent<Image>().enabled = false;
                }
                        //if(BTreeBackground.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text == null)       //No other blocks stored in the target block
                        
                        
                        //Destroy(block);         //destroy the original dragging block
                        
                    
                }
                
                }
        blockName = null;
    }
    
}
