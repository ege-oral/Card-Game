using UnityEngine;

namespace Theme.Data
{
    [CreateAssetMenu(fileName = "CardThemeSo", menuName = "Cards/CardThemeSo")]
    public class CardThemeSo : ScriptableObject
    {
        public string themeName;
        public Sprite background;
    }
}