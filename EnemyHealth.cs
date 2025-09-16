using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health,maxHealth;
    private TMP_Text healthText;
    void Start()
    {
        healthText = this.GetComponent<TMP_Text>();
        if(this.tag=="enemy1")
        maxHealth=Random.Range(50,60);
        else if(this.tag=="ememy2")
        maxHealth=Random.Range(30,45);
        else if(this.tag=="ememy3")
        maxHealth=Random.Range(20,40);
        else if(this.tag=="miniBoss")
        maxHealth=Random.Range(150,200);
        else
        maxHealth=400;
        health=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString()+"/"+maxHealth.ToString();
        healthText.color=new Color32(255,0,0,255);
    }
}
