using Buttons.Signals;
using Cards.Signals;
using SceneLoader;
using SceneLoader.Signals;
using Theme.Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoaderController sceneLoaderPrefab;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadNextLevelSignal>();
            Container.DeclareSignal<DrawCardsSignal>();
            Container.DeclareSignal<DrawSpecificCardsSignal>();
            Container.DeclareSignal<OneTwoThreeOrderSignal>();
            Container.DeclareSignal<SevenSevenSevenOrderSignal>();
            Container.DeclareSignal<SmartOrderSignal>();
            Container.DeclareSignal<CardDrawAnimationStartedSignal>();
            Container.DeclareSignal<CardDrawAnimationFinishedSignal>();
            Container.DeclareSignal<HandReArrangeAnimationStartedSignal>();
            Container.DeclareSignal<HandReArrangeAnimationFinishedSignal>();
            Container.DeclareSignal<RestartGameSignal>();
            Container.DeclareSignal<ChangeCardsThemeSignal>();
            
            Container.Bind<SceneLoaderController>().FromComponentInNewPrefab(sceneLoaderPrefab).AsSingle();
        }
    }
}
