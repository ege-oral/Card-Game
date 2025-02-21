using Cards.View;
using Cards.Factory;
using Cards.Services.Combinations.Combination;
using Cards.Services.Combinations.Optimization;
using Cards.Services.Combinations.Validation;
using Cards.Services.Sorting.Base;
using Cards.Services.Sorting.Strategies;
using Cards.Utils;
using Cards.View.Services;
using Deck;
using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private CardAnimationControllerSo cardAnimationControllerSo;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<CardController, CardControllerFactory>().FromComponentInNewPrefab(cardPrefab);
            Container.BindInstance(cardAnimationControllerSo).AsSingle();

            Container.Bind<ISorting>().WithId("OneTwoThreeSorting").To<OneTwoThreeSorting>().AsSingle();
            Container.Bind<ISorting>().WithId("SevenSevenSevenSorting").To<SevenSevenSevenSorting>().AsSingle();
            Container.Bind<ISorting>().WithId("SmartSorting").To<SmartSorting>().AsSingle();
            Container.BindInterfacesTo<CardSortingService>().AsSingle();
            Container.BindInterfacesTo<CardSortOrderService>().AsSingle();
            Container.BindInterfacesTo<CardCombinationsService>().AsSingle();
            Container.BindInterfacesTo<CardCombinationValidatorService>().AsSingle();
            Container.BindInterfacesTo<CardCombinationOptimizerService>().AsSingle();
            Container.Bind<CardRankComparer>().AsSingle();
            Container.Bind<CardDragHandler>().AsSingle();
            Container.Bind<CardHighlighter>().AsSingle();
            Container.Bind<CardNeighborFinder>().AsSingle();
        }
    }
}