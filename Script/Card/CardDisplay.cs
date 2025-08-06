using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FYP_project;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text ManaCostText;
    public TMP_Text cardText;

    public TMP_Text typeText;
    public Image bg;
    //public CardType cardType;
    Image crystal;
    // Start is called before the first frame update
    void Start()
    {
        //crystal = GameObject.Find("Crystal").GetComponent<Image>();
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateDisplay()
    {
        nameText.text = cardData.CardName;
        ManaCostText.text = cardData.ManaCost.ToString();
        cardText.text = cardData.description;
        typeText.text = cardData.cardType.ToString();
        if(cardData.rarity.ToString() == "Common")
        {
            bg.color = new Color32(255,255,255,255);
            //crystal.color = new Color(139,139,140,255);
        }
        else if(cardData.rarity.ToString() == "Rare")
        {
            bg.color = new Color32(172,192,255,255);
            //crystal.color = new Color(30,41,202,255);
        }
        else
        {
            bg.color = new Color32(255,203,138,255);
            //crystal.color = new Color(224,111,20,255);
        }
    }
}
