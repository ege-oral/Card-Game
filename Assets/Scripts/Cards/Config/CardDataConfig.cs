using System.Collections.Generic;
using Cards.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Cards.Config
{
    [CreateAssetMenu(fileName = "CardDataConfig", menuName = "Cards/CardDataConfig")]
    public class CardDataConfig : SerializedScriptableObject
    {
        [Header("Card Data")]
        [OdinSerialize]
        public Dictionary<CardSuit, Dictionary<int, Sprite>> CardSpritesBySuit = new();
    }
}