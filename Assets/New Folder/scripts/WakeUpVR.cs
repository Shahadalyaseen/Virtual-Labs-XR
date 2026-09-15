using UnityEngine;

public class WelcomeManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomeCanvas;
    public GameObject mainLabUI; // الكانفس الخاص بالتجربة (إذا كان عندك واحد)

    [Header("Audio")]
    public AudioSource welcomeAudioSource;
    public AudioSource buttonAudioSource;
    public AudioClip buttonClickSound;

    private void Start()
    {
        // أول ما يفتح المشهد، نعرض الترحيب ونخفي تجربة المعمل
        if (welcomeCanvas != null) welcomeCanvas.SetActive(true);
        if (mainLabUI != null) mainLabUI.SetActive(false);
    }

    public void StartLabExperience()
    {
        // 1. تشغيل صوت الزر
        if (buttonAudioSource != null && buttonClickSound != null)
        {
            buttonAudioSource.PlayOneShot(buttonClickSound);
        }

        // 2. إيقاف الصوت الترحيبي فوراً إذا كان شغال
        if (welcomeAudioSource != null && welcomeAudioSource.isPlaying)
        {
            welcomeAudioSource.Stop();
        }

        // 3. إخفاء لوحة الترحيب وإظهار واجهة التجربة
        if (welcomeCanvas != null) welcomeCanvas.SetActive(false);
        if (mainLabUI != null) mainLabUI.SetActive(true);
    }
}