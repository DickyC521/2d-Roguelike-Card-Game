using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FYP_project;

public class BTree : MonoBehaviour
{
    private Image BTreeBg;
    private TMP_Text BTreeText;
    private GameObject BTreeButton,BTreeBackground;
    public float blockSpacing;
    public float verticalSpacing;
    public List<int> blockNo_List = new List<int>(); 
    //public List<GameObject> BTreeBlock = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        BTreeButton = GameObject.Find("B+Tree");
        BTreeText =  GameObject.Find("B+TreeItem").GetComponent<TMP_Text>();
        BTreeBackground = GameObject.Find("B+TreeBackground");
        BTreeBg = BTreeBackground.GetComponent<Image>();
        BTreeBg.enabled = false;
        //BTreeBlock = GetComponent<CardMovement>().BTreelist;
    }

    // Update is called once per frame
    void Update()
    {
        //int.TryParse(BTreeText.text,out int testNo);
        //Debug.Log(testNo);
        //BTreeText.text = BTreeBackground.transform.childCount.ToString();
        //UpdateHandVisuals();
    }
    public void DisplayBackground()
    {
        //UpdateHandVisuals();
        BTreeBg.enabled = !BTreeBg.enabled;
        for(int x = 0; x < BTreeBackground.transform.childCount; x++)
        {
            GameObject block = BTreeBackground.transform.GetChild(x).gameObject;
            int.TryParse(block.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>().text,out int containNo_);
            if(containNo_ < 2)
            {
                Image image = block.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                image.enabled = !image.enabled;
                TMP_Text blockNoText = block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();     //Get number of the block
                blockNoText.enabled = !blockNoText.enabled;     //Enable to display number contained in blocks
                
                //block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().enabled = !block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().enabled;
                //block.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>().enabled = !block.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>().enabled;
            }   
            else
            {
                for(int i = 0; i < block.transform.GetChild(0).childCount; i++)
                {
                    Image image = block.transform.GetChild(0).GetChild(i).GetComponent<Image>();
                    image.enabled = !image.enabled;
                    if(i < 3)
                    {
                        for(int y = 0; y < 2; y++)
                        {
                            TMP_Text blockNoText = block.transform.GetChild(0).GetChild(i).GetChild(y).GetComponent<TMP_Text>();     //Get number of the block
                            blockNoText.enabled = !blockNoText.enabled;
                        }
                    }
                }
            }
            
            
            //Image image = BTreeBackground.transform.GetChild(x).GetChild(0).GetChild(0).GetComponent<Image>();          //Get Image of block
            //TMP_Text blockNoText = block.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();     //Get number of the block
            //TMP_Text blockContainNoText = block.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
            //image.enabled = !image.enabled;             //Enable to display block
             //Enable to display number contained in blocks
            //blockContainNoText.enabled = !blockContainNoText.enabled;
            
        }
    }
    public void UpdateHandVisuals()
    {
        int blockCount = BTreeBackground.transform.childCount;
        verticalSpacing = 0f;
        int rowBlock = 0, rowCount = 0;
        if(blockCount == 1)
        {
            BTreeBackground.transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
            for(int i=0; i < blockCount;i++)
            {
                
                float horizonalOffset = (blockSpacing * (i - (blockCount - 1)/ 2f));  // = i * cardSpacing          // move cards left or right
                float normalizedPosition = (2f * i/ (blockCount - 1) -1f);       //Normalize card position between -1,1
                float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);   // = i * verticalSpacing     //move cards up and down
                
                //Set card position
                BTreeBackground.transform.GetChild(i).transform.localPosition = new Vector3(horizonalOffset, verticalOffset, 0f);
            }
        
        
    }
}
