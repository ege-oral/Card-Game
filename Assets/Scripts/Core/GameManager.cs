using Buttons.Signals;
using Deck;
using Player;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private DeckManager _deckManager;
        private PlayerController _playerController;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(DeckManager deckManager, PlayerController playerController, SignalBus signalBus)
        {
            _deckManager = deckManager;
            _playerController = playerController;
            _signalBus = signalBus;
            _signalBus.Subscribe<RestartGameSignal>(RestartGame);
        }

        private void Start()
        {
            _deckManager.CreateDeck();
        }
        
        private void RestartGame()
        {
            _playerController.ResetPlayerHand();
            _deckManager.ResetDeckAndCreateDeck();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<RestartGameSignal>(RestartGame);
        }
    }
}