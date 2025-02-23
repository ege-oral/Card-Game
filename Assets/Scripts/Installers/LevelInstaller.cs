using Board.Services;
using Cards.View;
using Cards.Factory;
using Cards.Pool;
using Cards.Services.Combination;
using Cards.Services.Optimization;
using Cards.Services.Sorting.Base;
using Cards.Services.Sorting.Strategies;
using Cards.Services.Validation;
using Cards.Utils;
using Cards.View.Services;
using Core;
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
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DeckManager>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<CardController, CardControllerFactory>().FromComponentInNewPrefab(cardPrefab);
            Container.Bind<CardPool>().FromComponentInHierarchy().AsSingle();
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
            Container.BindInterfacesTo<BoardAnimationService>().AsSingle();
        }
    }
}