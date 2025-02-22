using System;
using Cards.Signals;
using UnityEngine;
using Zenject;

namespace Board.Services
{
    public class BoardAnimationService : IBoardAnimationService, IDisposable
    {
        private readonly SignalBus _signalBus;
        private int _animationCounter;
        
        public BoardAnimationService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            SubscribeToSignals();
        }
        
        public bool IsAnyAnimationPlaying()
        {
            return _animationCounter != 0;
        }
        
        private void IncreaseAnimationCounter()
        {
            _animationCounter += 1;
            Debug.Log($"Animation Counter Increased, new value is {_animationCounter}");
        }

        private void DecreaseAnimationCounter()
        {
            _animationCounter -= 1;
            Debug.Log($"Animation Counter Decreased, new value is {_animationCounter}");
        }
        
        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<CardDrawAnimationStartedSignal>(IncreaseAnimationCounter);
            _signalBus.Subscribe<HandReArrangeAnimationStartedSignal>(IncreaseAnimationCounter);
         
            _signalBus.Subscribe<CardDrawAnimationFinishedSignal>(DecreaseAnimationCounter);
            _signalBus.Subscribe<HandReArrangeAnimationFinishedSignal>(DecreaseAnimationCounter);
        }
        
        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<CardDrawAnimationStartedSignal>(IncreaseAnimationCounter);
            _signalBus.Unsubscribe<HandReArrangeAnimationStartedSignal>(IncreaseAnimationCounter);
            
            _signalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(DecreaseAnimationCounter);
            _signalBus.Unsubscribe<HandReArrangeAnimationFinishedSignal>(DecreaseAnimationCounter);
        }
        
        public void Dispose()
        {
            UnsubscribeFromSignals();
        }
    }
}