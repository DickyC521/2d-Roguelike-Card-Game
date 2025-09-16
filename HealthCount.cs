using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthCount : MonoBehaviour
{
    private TMP_Text healthText;
    public int currentHealth,maxHealth;
    // Start is called before the first frame update
    void Start()
    {

        healthText = this.GetComponent<TMP_Text>();
        healthText.color=new Color32(213,40,40,255);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = PlayerPrefs.GetInt("currentHealth");
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        healthText.text = currentHealth.ToString()+"/"+maxHealth.ToString();
        healthText.color=new Color32(213,40,40,255);
    }
}
