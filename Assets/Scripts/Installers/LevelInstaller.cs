using Cards.View;
using Cards.Factory;
using Cards.Services;
using Cards.Utils;
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
            Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<CardController, CardControllerFactory>().FromComponentInNewPrefab(cardPrefab);

            Container.Bind<ISorting>().WithId("OneTwoThreeSorting").To<OneTwoThreeSorting>().AsSingle();
            Container.Bind<ISorting>().WithId("SevenSevenSevenSorting").To<SevenSevenSevenSorting>().AsSingle();
            Container.BindInterfacesTo<CardSortingService>().AsSingle();
            Container.BindInterfacesTo<CardSortOrderService>().AsSingle();
            Container.Bind<CardRankComparer>().AsSingle();
        }
    }
}