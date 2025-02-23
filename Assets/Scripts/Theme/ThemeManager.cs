using System.Collections.Generic;
using Cards.View;
using Theme.Data;
using Theme.Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Theme
{
    public class ThemeManager : MonoBehaviour
    {
        [SerializeField] private CardThemeSo defaultTheme;
        public CardThemeSo DefaultTheme => defaultTheme;
        
        [SerializeField] private List<CardThemeSo> availableThemes;
        private CardThemeSo _currentTheme;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ChangeCardsThemeSignal>(ChangeCardsThemeToRandomTheme);
        }

        private void ChangeCardsThemeToRandomTheme()
        {
            if (availableThemes == null || availableThemes.Count < 2) return; // Ensure at least 2 themes exist
            
            CardThemeSo newTheme;
            do
            {
                newTheme = availableThemes[Random.Range(0, availableThemes.Count)];
            } 
            while (newTheme == _currentTheme); // Ensure it's different from the current theme

            _currentTheme = newTheme;
            ApplyTheme(_currentTheme);
        }

        private void ApplyTheme(CardThemeSo theme)
        {
            var allCards = FindObjectsOfType<CardController>();
            foreach (var card in allCards)
            {
                card.UpdateTheme(theme.background);
            }
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<ChangeCardsThemeSignal>(ChangeCardsThemeToRandomTheme);
        }
    }
}