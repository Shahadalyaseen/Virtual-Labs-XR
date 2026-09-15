using UnityEngine;

public class WelcomeAudioController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomeCanvas; // ضفنا هذا المتغير للكانفس

    [Header("Audio Component")]
    public AudioSource welcomeAudioSource;

    // دالة تشغيل الصوت (نفس كودك السابق)
    public void PlayWelcomeAudio()
    {
        if (welcomeAudioSource != null)
        {
            welcomeAudioSource.Play();
            Debug.Log("تم تشغيل صوت الترحيب بنجاح!");
        }
        else
        {
            Debug.LogWarning("لم يتم ربط AudioSource في الـ Inspector!");
        }
    }

    // الدالة الجديدة لزر START
    public void StartLab()
    {
        // 1. إيقاف الصوت الترحيبي فوراً لو كان لسا شغال
        if (welcomeAudioSource != null && welcomeAudioSource.isPlaying)
        {
            welcomeAudioSource.Stop();
        }

        // 2. إخفاء الكانفس الترحيبي
        if (welcomeCanvas != null)
        {
            welcomeCanvas.SetActive(false);
            Debug.Log("تم إخفاء الكانفس وبدء التجربة!");
        }
    }
}