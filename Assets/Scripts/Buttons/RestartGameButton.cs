using System;
using Buttons.Signals;
using Cards.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class RestartGameButton : MonoBehaviour
    {
        [SerializeField] private Button restartGameButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CardDrawAnimationStartedSignal>(DisableButton);
            _signalBus.Subscribe<HandReArrangeAnimationStartedSignal>(DisableButton);
            
            _signalBus.Subscribe<CardDrawAnimationFinishedSignal>(EnableButton);
            _signalBus.Subscribe<HandReArrangeAnimationFinishedSignal>(EnableButton);
        }
        
        private void OnButtonClicked()
        {
            _signalBus.Fire<RestartGameSignal>();
        }
        
        private void DisableButton()
        {
            restartGameButton.onClick.RemoveListener(OnButtonClicked);
        }
        
        private void EnableButton()
        {
            restartGameButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CardDrawAnimationStartedSignal>(DisableButton);
            _signalBus.Unsubscribe<HandReArrangeAnimationStartedSignal>(DisableButton);
            
            _signalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(EnableButton);
            _signalBus.Unsubscribe<HandReArrangeAnimationFinishedSignal>(EnableButton);
        }
    }
}