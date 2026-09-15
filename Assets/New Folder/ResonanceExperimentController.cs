using UnityEngine;
using UnityEngine.Events;

public class ResonanceExperimentController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource tuningForkAudio;

    [Header("Sound Waves")]
    [SerializeField] private SoundWaveEmitter waveEmitter;

    [Header("Tuning Fork Animation")]
    [SerializeField] private Animator tuningForkAnimator;
    [SerializeField] private string vibrationTriggerName = "Hit";

    [Header("Response Settings")]
    [SerializeField] private float maximumExpectedHitSpeed = 3f;
    [SerializeField] private float minimumAudioVolume = 0.55f;
    [SerializeField] private float maximumAudioVolume = 1f;

    [Header("Interaction")]
    [SerializeField] private float activationCooldown = 1f;

    [Header("Experiment Events")]
    [SerializeField] private UnityEvent onSuccessfulHit;

    private float lastActivationTime = -100f;

    // هذا نستخدمه الآن مع الكنترولر
    public void TriggerResonance()
    {
        TryTriggerExperiment(0.75f);
    }

    // نخليه موجود لو رجعنا نستخدم المطرقة لاحقًا
    public void RegisterHit(float hitSpeed)
    {
        float hitIntensity = Mathf.Clamp01(
            hitSpeed / maximumExpectedHitSpeed
        );

        TryTriggerExperiment(hitIntensity);
    }

    private void TryTriggerExperiment(float intensity)
    {
        if (Time.time - lastActivationTime < activationCooldown)
            return;

        lastActivationTime = Time.time;

        PlayTuningForkSound(intensity);
        PlayTuningForkAnimation();
        PlaySoundWaves(intensity);

        onSuccessfulHit?.Invoke();

        Debug.Log("RESONANCE TRIGGERED");
    }

    private void PlayTuningForkSound(float intensity)
    {
        if (tuningForkAudio == null)
        {
            Debug.LogWarning(
                "ResonanceExperimentController: Audio Source is missing."
            );
            return;
        }

        if (tuningForkAudio.clip == null)
        {
            Debug.LogWarning(
                "ResonanceExperimentController: Audio Clip is missing."
            );
            return;
        }

        tuningForkAudio.Stop();
        tuningForkAudio.time = 0f;

        tuningForkAudio.volume = Mathf.Lerp(
            minimumAudioVolume,
            maximumAudioVolume,
            intensity
        );

        tuningForkAudio.Play();
    }

    private void PlayTuningForkAnimation()
    {
        if (tuningForkAnimator == null)
        {
            Debug.LogWarning(
                "ResonanceExperimentController: Animator is missing."
            );
            return;
        }

        tuningForkAnimator.SetTrigger(vibrationTriggerName);
    }

    private void PlaySoundWaves(float intensity)
    {
        if (waveEmitter == null)
        {
            Debug.LogWarning(
                "ResonanceExperimentController: Wave Emitter is missing."
            );
            return;
        }

        waveEmitter.Emit(intensity);
    }

    [ContextMenu("TEST Resonance")]
    public void TestExperiment()
    {
        TriggerResonance();
    }
}