using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerStat Player;
    public int stage;
    int currentHealth;
    private TMP_Text winMessageText;
    private Image winMessageImage;
    public void Play()
    {
        SceneManager.LoadScene("Map");
        if(!PlayerPrefs.HasKey("currentHealth") || currentHealth < 0)
        {
            PlayerPrefs.SetInt("currentHealth",200);
            PlayerPrefs.SetInt("maxHealth",200);
            PlayerPrefs.SetInt("healAmount",40);
            PlayerPrefs.SetInt("coin",0);
            PlayerPrefs.SetInt("stage",0);
        }
        
    }
    public void Quit()
    {
        //PlayerPrefs.SetInt("currentHealth",100);
        //PlayerPrefs.SetInt("maxHealth",200);
        //PlayerPrefs.SetInt("healAmount",40);
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void Back()
    {
        
        if(SceneManager.GetActiveScene().name=="Map")
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("Map");
    }
    public void Combat()
    {
        SceneManager.LoadScene("Combat");
    }
    public void EnterCamp()
    {
        SceneManager.LoadScene("Camp");

        Debug.Log(SceneManager.GetActiveScene().name);
    }
    public void EnterShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void EnterMiniBoss()
    {
        SceneManager.LoadScene("MiniBoss");
    }
    public void EnterBoss()
    {
        SceneManager.LoadScene("Boss");
    }
    public void EnterTreasure()
    {
        SceneManager.LoadScene("Treasure");
    }
    public void WinCombat()
    {
        SceneManager.LoadScene("Map");
        stage++;
        PlayerPrefs.SetInt("stage",stage); 
    }
    void Start()
    {
        currentHealth = PlayerPrefs.GetInt("currentHealth");
        stage = PlayerPrefs.GetInt("stage");
        
        if(SceneManager.GetActiveScene().name=="Map")
        {
            winMessageImage = GameObject.Find("WinMessage").GetComponent<Image>();
            winMessageText = GameObject.Find("WinMessageText").GetComponent<TMP_Text>();
            winMessageImage.enabled = false;
            winMessageText.enabled = false;
            UpdateEvent("Boss",false);
            UpdateEvent("Shop",false);
            UpdateEvent("Monster (2)",false);
            UpdateEvent("Treasure",false);
            UpdateEvent("MiniBoss",false);
            UpdateEvent("Shop (1)",false);
            UpdateEvent("Camp",false);
            UpdateEvent("Monster (1)",false);
            UpdateEvent("Monster",false);
            UpdateEvent("Monster (3)",false);
        }
       
    }
    private void UpdateEvent(string sceneName,bool encounter)
    {
        if(!encounter)
        {
            GameObject.Find(sceneName).GetComponent<Button>().enabled = false;
            GameObject.Find(sceneName).GetComponent<Image>().color = new Color32(255,255,255,60);
        }
        else
        {
            GameObject.Find(sceneName).GetComponent<Button>().enabled = true;
            GameObject.Find(sceneName).GetComponent<Image>().color = new Color32(255,255,255,255);
        }
    }
    // Update is called once per frame
    void Update()
    {
        stage = PlayerPrefs.GetInt("stage");
        if(SceneManager.GetActiveScene().name=="Map")
        {
            if(stage < 6)
            {
            if(stage < 5)
            {
                UpdateEvent("Boss",false);
                if(stage < 4)
                {
                    UpdateEvent("Shop",false);
                    if(stage < 3)
                    {
                        UpdateEvent("Monster (2)",false);
                        UpdateEvent("Treasure",false);
                        if(stage < 2)
                        {
                            UpdateEvent("MiniBoss",false);
                            UpdateEvent("Shop (1)",false);
                            UpdateEvent("Camp",false);
                            if(stage < 1)
                            {
                                UpdateEvent("Monster (1)",false);
                                UpdateEvent("Monster",false);
                                if(stage == 0)
                                {
                                    UpdateEvent("Monster (3)",true);
                                }
                            }
                            else
                            {
                                UpdateEvent("Monster (1)",true);
                                UpdateEvent("Monster",true);

                                UpdateEvent("Monster (3)",false);
                            }
                        }
                        else
                        {
                            UpdateEvent("MiniBoss",true);
                            UpdateEvent("Shop (1)",true);
                            UpdateEvent("Camp",true);

                            UpdateEvent("Monster",false);
                            UpdateEvent("Monster (1)",false);
                        }
                    }
                    else
                    {
                        UpdateEvent("Monster (2)",true);
                        UpdateEvent("Treasure",true);

                        UpdateEvent("MiniBoss",false);
                        UpdateEvent("Shop (1)",false);
                        UpdateEvent("Camp",false);
                    }
                }
                else
                {
                    UpdateEvent("Shop",true);

                    UpdateEvent("Monster (2)",false);
                    UpdateEvent("Treasure",false);
                } 
            }
            else
            {
                UpdateEvent("Boss",true);

                UpdateEvent("Shop",false);
            }
            }
            else
            {
                winMessageImage.enabled = true;
                winMessageText.enabled = true;
            }
        }
    }
}
