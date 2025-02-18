using Cards;
using Cards.Factory;
using Cards.Services;
using Deck;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject cardPrefab;
        public override void InstallBindings()
        {
            Container.Bind<Card>().AsTransient();
            Container.BindInterfacesTo<CardSortOrderService>().AsSingle();
            Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<CardController, CardControllerFactory>().FromComponentInNewPrefab(cardPrefab);
        }
    }
}