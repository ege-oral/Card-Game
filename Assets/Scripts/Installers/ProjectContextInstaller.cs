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
            
            Container.Bind<SceneLoaderController>().FromComponentInNewPrefab(sceneLoaderPrefab).AsSingle();
        }
    }
}
