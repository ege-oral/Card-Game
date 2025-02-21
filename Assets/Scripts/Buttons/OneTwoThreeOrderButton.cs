using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class OneTwoThreeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button oneTwoThreeOrderButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            oneTwoThreeOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<OneTwoThreeOrderSignal>();
        }
    }
}