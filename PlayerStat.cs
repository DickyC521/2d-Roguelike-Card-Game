using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class PlayerStat : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth,maxHealth,healAmount,coin;
    public TMP_Text coinText;
    void Start()
    {
        currentHealth=PlayerPrefs.GetInt("currentHealth");
        maxHealth=PlayerPrefs.GetInt("maxHealth");
        healAmount=PlayerPrefs.GetInt("healAmount");
        coin = PlayerPrefs.GetInt("coin");
        coinText = GameObject.Find("CoinText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth=PlayerPrefs.GetInt("currentHealth");
        maxHealth=PlayerPrefs.GetInt("maxHealth");
        healAmount=PlayerPrefs.GetInt("healAmount");
        coin = PlayerPrefs.GetInt("coin");
        coinText.text = coin.ToString();
    }
}
