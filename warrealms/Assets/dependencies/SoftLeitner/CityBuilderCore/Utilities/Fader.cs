using System.Collections;
using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// simple helper that fades a canvas group in or out over a short period
    /// </summary>
    public class Fader : MonoBehaviour
    {
        [Tooltip("canvas group that is faded in or out(probably a fully black image on top)")]
        public CanvasGroup CanvasGroup;
        [Tooltip("how long the fading takes in seconds")]
        public float Duration;

        public void Load() => LoadNamed(null);
        public void LoadNamed(string name)
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(load(name));
        }
        private IEnumerator load(string name)
        {
            gameObject.SetActive(true);

            yield return fadeOut();
            Dependencies.Get<IGameSaver>().LoadNamed(name);
            yield return delayedFadeIn(5);
        }

        public void DelayedFadeIn(int frames = 5)
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(delayedFadeIn(frames));
        }
        private IEnumerator delayedFadeIn(int frames)
        {
            CanvasGroup.alpha = 1f;

            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }

            yield return fadeIn();
        }

        private IEnumerator fadeIn()
        {
            CanvasGroup.alpha = 1f;

            float _time = 0f;
            while (_time < Duration)
            {
                yield return null;
                _time += Time.unscaledDeltaTime;
                CanvasGroup.alpha = 1f - _time / Duration;
            }

            gameObject.SetActive(false);
            CanvasGroup.alpha = 0f;
        }
        private IEnumerator fadeOut()
        {
            CanvasGroup.alpha = 0f;

            float _time = 0f;
            while (_time < Duration)
            {
                yield return null;
                _time += Time.unscaledDeltaTime;
                CanvasGroup.alpha = _time / Duration;
            }

            CanvasGroup.alpha = 1f;
        }
    }
}
