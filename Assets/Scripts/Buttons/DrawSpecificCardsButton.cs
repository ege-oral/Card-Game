using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class DrawSpecificCardsButton : MonoBehaviour
    {
        [SerializeField] private Button drawSpecificCardsButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            drawSpecificCardsButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<DrawSpecificCardsSignal>();
        }
    }
}