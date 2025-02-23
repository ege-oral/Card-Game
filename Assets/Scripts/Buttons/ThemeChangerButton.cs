using Theme.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class ThemeChangerButton : MonoBehaviour
    {
        [SerializeField] private Button themeChangerButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            themeChangerButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<ChangeCardsThemeSignal>();
        }
    }
}