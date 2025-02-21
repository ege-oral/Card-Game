using Buttons.Signals;
using Cards.Signals;
using Input.Signals;
using SceneLoader;
using SceneLoader.Signals;
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
            Container.DeclareSignal<EnableInputSignal>();
            Container.DeclareSignal<DisableInputSignal>();
            Container.DeclareSignal<DrawCardsSignal>();
            Container.DeclareSignal<OneTwoThreeOrderSignal>();
            Container.DeclareSignal<SevenSevenSevenOrderSignal>();
            Container.DeclareSignal<SmartOrderSignal>();
            Container.DeclareSignal<CardDrawAnimationFinishedSignal>();
            
            Container.Bind<SceneLoaderController>().FromComponentInNewPrefab(sceneLoaderPrefab).AsSingle();
        }
    }
}
