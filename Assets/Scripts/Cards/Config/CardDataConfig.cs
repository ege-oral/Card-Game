using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Cards.Config
{
    [CreateAssetMenu(fileName = "CardDataConfig", menuName = "Card/CardDataConfig")]
    public class CardDataConfig : SerializedScriptableObject
    {
        [Header("Card Assets")]
        public Sprite backSprite;
        
        [Header("Card Data")]
        [OdinSerialize]
        public Dictionary<CardSuit, Dictionary<int, Sprite>> CardSpritesBySuit = new();
    }
}