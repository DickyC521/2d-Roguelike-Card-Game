using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP_project
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string CardName;
        public CardType cardType;
        public Rarity rarity;
        public int dealValue;
        public int ManaCost;
        public string description;
        public enum CardType
        {
            Insert,
            Delete,
            Magic,
            Find,
            Store
        }
        public enum Rarity
        {
            Common,
            Rare,
            Legendary
        }
    }
}
/*public class CardType : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
