using System.Collections;
using UnityEngine;

public class AcousticLevitationLab : MonoBehaviour
{
    [Header("Audio & 3D Wave Visuals")]
    public AudioSource soundSource;
    public GameObject soundRingPrefab;  // اسحبي SoundWaveRing Prefab هنا
    public Transform waveSpawnPoint;    // اسحبي كائن WaveSpawnPoint هنا

    // تُستدعى من مطرقة الجزء الأول (MalletStrike)
    public void TriggerSoundWave()
    {
        // 1. تشغيل الصوت
        if (soundSource != null)
        {
            soundSource.Play();
        }

        // 2. إطلاق حلقات الموجات ثلاثية الأبعاد
        StartCoroutine(SpawnWaveRingsRoutine());
    }

    private IEnumerator SpawnWaveRingsRoutine()
    {
        if (soundRingPrefab == null) yield break;

        Transform spawn = waveSpawnPoint != null ? waveSpawnPoint : transform;

        // إطلاق 3 حلقات 3D متتالية تتوسع في الهواء
        for (int i = 0; i < 3; i++)
        {
            Instantiate(soundRingPrefab, spawn.position, spawn.rotation);
            yield return new WaitForSeconds(0.15f);
        }
    }
}