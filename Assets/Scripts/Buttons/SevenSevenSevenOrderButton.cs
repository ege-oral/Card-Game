using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class SevenSevenSevenOrderButton : MonoBehaviour
    {
        [SerializeField] private Button sevenSevenSevenOrderButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            sevenSevenSevenOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<SevenSevenSevenOrderSignal>();
        }
    }
}