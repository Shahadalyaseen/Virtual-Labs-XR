using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image fadeImage;

    [Tooltip("مدة الانتقال")]
    public float fadeDuration = 0.6f;

    [Header("Main Menu")]
    public string mainMenuSceneName = "startUI"; // تم تصحيح الاسم هنا تلقائياً

    private bool isTransitioning = false;

    private void Start()
    {
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (isTransitioning)
            return;

        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    public void ExitToMainMenu()
    {
        if (isTransitioning)
            return;

        StartCoroutine(FadeOutAndLoad(mainMenuSceneName));
    }

    private IEnumerator FadeIn()
    {
        isTransitioning = true;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;

            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;

                color.a = Mathf.Lerp(
                    1f,
                    0f,
                    timer / fadeDuration
                );

                fadeImage.color = color;

                yield return null;
            }

            color.a = 0f;
            fadeImage.color = color;

            fadeImage.gameObject.SetActive(false);
        }

        isTransitioning = false;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isTransitioning = true;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;

            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;

                color.a = Mathf.Lerp(
                    0f,
                    1f,
                    timer / fadeDuration
                );

                fadeImage.color = color;

                yield return null;
            }

            color.a = 1f;
            fadeImage.color = color;
        }

        SceneManager.LoadScene(sceneName);
    }
}