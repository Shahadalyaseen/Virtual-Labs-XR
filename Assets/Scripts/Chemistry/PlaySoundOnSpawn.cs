using UnityEngine;

/// <summary>
/// تشغيل صوت عند ظهور الأوبجيكت مع دعم التكرار السلس بدون تقطيع.
/// </summary>
public class PlaySoundOnSpawn : MonoBehaviour
{
    [Tooltip("ملف الصوت المراد تشغيله")]
    public AudioClip spawnSound;

    [Tooltip("مستوى الصوت (من 0 إلى 1)")]
    [Range(0f, 1f)]
    public float volume = 0.8f;

    [Tooltip("تكرار الصوت باستمرار")]
    public bool loop = false;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        // إلغاء التنعيم الفضائي لضمان ثبات الصوت وتجنب التقطيع عند الحركة
        audioSource.spatialBlend = 0f; 
    }

    void OnEnable()
    {
        if (spawnSound == null || audioSource == null) return;

        audioSource.clip = spawnSound;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }

    void OnDisable()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}