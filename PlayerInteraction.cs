using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    int health,maxHealth,healAmount;
    private Image image,nextButton;
    private TMP_Text tMP_Text,buttonText;
    public void Heal()
    {
        health=PlayerPrefs.GetInt("currentHealth");
         maxHealth=PlayerPrefs.GetInt("maxHealth");
         healAmount=GetComponent<PlayerStat>().healAmount;
        //GameObject.Find("HealButton").SetActive(true);
        Debug.Log(health);
        if((health+healAmount)<maxHealth)
            health+=healAmount;
        else
            health=maxHealth;
        PlayerPrefs.SetInt("currentHealth",health);
        Debug.Log(health);
        image.enabled = false;
        tMP_Text.enabled = false;
        nextButton.enabled = true;
        buttonText.enabled = true;
        //EventSystem.current.currentSelectedGameObject.SetActive(false);
    }
    void Start()
    {
        image = GameObject.Find("HealButton").GetComponent<Image>();
        tMP_Text = GameObject.Find("HealText").GetComponent<TMP_Text>();
        nextButton = GameObject.Find("NextButton").GetComponent<Image>();
        buttonText = GameObject.Find("ButtonText").GetComponent<TMP_Text>();
        nextButton.enabled = false;
        buttonText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
