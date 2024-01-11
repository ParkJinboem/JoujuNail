using System.Collections;
using OnDot.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OnDot.System
{
    public class ScreenFaderManager : PersistentSingleton<ScreenFaderManager>
    {
        public enum FadeType
        {
            Black,
            Loading
        }

        [SerializeField] private float fadeDuration = 1f;

        [Header("Black")]
        [SerializeField] private CanvasGroup blackCanvasGroup;

        [Header("Loading")]
        public CanvasGroup loadingCanvasGroup;
        [SerializeField] private TextMeshProUGUI loadingProgressText;
        [SerializeField] private Slider loadingProgressSlider;
        [SerializeField] private Animator loadingProgressAnim;

        public static void DirectFadeOut(FadeType fadeType = FadeType.Black)
        {
            Progress("0%");
            Progressbar(0f);

            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.blackCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    Instance.AnimAction(true);
                    break;
            }
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = false;
        }

        public static void FadeOut(FadeType fadeType = FadeType.Black)
        {
            Progress("0%");
            Progressbar(0f);

            Instance.StartCoroutine(Instance.IFadeOut(fadeType));
        }

        public static void Progress(string progress)
        {
            Instance.loadingProgressText.text = progress;
        }

        public static void Progressbar(float progress)
        {
            Instance.loadingProgressSlider.value = progress;
        }

        private IEnumerator IFadeOut(FadeType fadeType)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.blackCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }
            canvasGroup.gameObject.SetActive(true);
            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }

        public static void FadeIn()
        {
            Instance.StartCoroutine(Instance.IFadeIn());
        }

        private IEnumerator IFadeIn()
        {
            CanvasGroup canvasGroup;
            if (Instance.blackCanvasGroup.alpha > 0.1f)
            {
                canvasGroup = Instance.blackCanvasGroup;
            }
            else
            {
                canvasGroup = Instance.loadingCanvasGroup;
            }

            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));
            canvasGroup.gameObject.SetActive(false);
        }

        private IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            AnimAction(false);
            canvasGroup.alpha = finalAlpha;
            canvasGroup.blocksRaycasts = false;
        }

        public float canvasAlphaNumber()
        {
            float alphaNumber = loadingCanvasGroup.alpha;
            return alphaNumber;
        }

        public void AnimAction(bool action)
        {
            loadingProgressAnim.SetBool("Action", action);
        }

    }
}