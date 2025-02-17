using System.Collections;
using SceneLoader.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SceneLoader
{
    public class SceneLoaderController : MonoBehaviour
    {
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private bool playTransitionOnStart;
        [SerializeField] private float transitionDuration;
        
        private SignalBus _signalBus;
        private ZenjectSceneLoader _zenjectSceneLoader;

        [Inject]
        public void Construct(SignalBus signalBus, ZenjectSceneLoader zenjectSceneLoader)
        {
            _signalBus = signalBus;
            _zenjectSceneLoader = zenjectSceneLoader;
            _signalBus.Subscribe<LoadNextLevelSignal>(OnLoadNextLevelSignal);
        }
        
        private void OnLoadNextLevelSignal()
        {
            StartCoroutine(LoadNextSceneRoutine());
        }

        private IEnumerator LoadNextSceneRoutine()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = currentSceneIndex + 1;
            
            yield return StartCoroutine(PlayAnimation("FadeOut"));

            yield return _zenjectSceneLoader.LoadSceneAsync(nextSceneIndex);

            yield return StartCoroutine(PlayAnimation("FadeIn"));
        }

        private IEnumerator PlayAnimation(string animationTrigger)
        {
            if (transitionAnimator == null) yield break;
            
            transitionAnimator.SetTrigger(animationTrigger);
            yield return new WaitForSeconds(transitionDuration);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<LoadNextLevelSignal>(OnLoadNextLevelSignal);
        }
    }
}