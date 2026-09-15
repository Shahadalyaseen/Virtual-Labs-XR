using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResonanceFrequencyExperiment : MonoBehaviour
{
    public enum FrequencyChoice
    {
        None,
        Hz440,
        Hz512
    }

    [Header("Selected Frequency")]
    [SerializeField] private FrequencyChoice selectedFrequency = FrequencyChoice.None;

    [Header("Source Fork")]
    [SerializeField] private Animator sourceForkAnimator;
    [SerializeField] private string sourceHitTrigger = "Hit";

    [Header("Receiver Fork")]
    [SerializeField] private Animator receiverForkAnimator;
    [SerializeField] private string receiverResonanceTrigger = "Resonate";

    [Header("Hanging Ball")]
    [SerializeField] private Animator ballAnimator;
    [SerializeField] private string ballMoveTrigger = "Move";

    [Header("Audio")]
    [SerializeField] private AudioSource sourceAudio;
    [SerializeField] private AudioSource receiverAudio;

    [SerializeField] private AudioClip frequency440Clip;
    [SerializeField] private AudioClip frequency512Clip;

    [Header("Resonance Timing")]
    [SerializeField] private float resonanceDelay = 0.35f;

    [Header("Final Explanation")]
    [SerializeField] private float explanationDelay = 2.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent on440Selected;
    [SerializeField] private UnityEvent on512Selected;
    [SerializeField] private UnityEvent onResonanceOccurred;
    [SerializeField] private UnityEvent onNoResonance;

    // جديد
    [SerializeField] private UnityEvent onExperimentCompleted;

    private bool strikeAllowed = false;
    private bool strikeInProgress = false;

    private Coroutine explanationCoroutine;

    public void Select440()
    {
        selectedFrequency = FrequencyChoice.Hz440;
        strikeAllowed = true;

        Debug.Log("440 Hz selected");

        on440Selected?.Invoke();
    }

    public void Select512()
    {
        selectedFrequency = FrequencyChoice.Hz512;
        strikeAllowed = true;

        Debug.Log("512 Hz selected");

        on512Selected?.Invoke();
    }

    public void StrikeSourceFork()
    {
        if (!strikeAllowed)
        {
            Debug.LogWarning("Select a frequency first.");
            return;
        }

        if (strikeInProgress)
            return;

        StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        strikeInProgress = true;

        FrequencyChoice frequencyAtStrike = selectedFrequency;

        // اهتزاز الشوكة المصدر
        if (sourceForkAnimator != null)
        {
            sourceForkAnimator.SetTrigger(sourceHitTrigger);
        }

        // تشغيل الصوت
        PlaySelectedFrequency(frequencyAtStrike);

        yield return new WaitForSeconds(resonanceDelay);

        if (frequencyAtStrike == FrequencyChoice.Hz440)
        {
            TriggerResonance();
        }
        else if (frequencyAtStrike == FrequencyChoice.Hz512)
        {
            TriggerNoResonance();
        }

        yield return new WaitForSeconds(0.5f);

        strikeInProgress = false;
    }

    private void PlaySelectedFrequency(FrequencyChoice frequency)
    {
        if (sourceAudio == null)
            return;

        if (frequency == FrequencyChoice.Hz440)
        {
            sourceAudio.clip = frequency440Clip;
        }
        else if (frequency == FrequencyChoice.Hz512)
        {
            sourceAudio.clip = frequency512Clip;
        }

        if (sourceAudio.clip != null)
        {
            sourceAudio.Stop();
            sourceAudio.time = 0f;
            sourceAudio.Play();
        }
    }

    private void TriggerResonance()
    {
        Debug.Log("RESONANCE OCCURRED - 440 Hz");

        if (receiverForkAnimator != null)
        {
            receiverForkAnimator.SetTrigger(receiverResonanceTrigger);
        }

        if (ballAnimator != null)
        {
            ballAnimator.SetTrigger(ballMoveTrigger);
        }

        if (receiverAudio != null)
        {
            receiverAudio.clip = frequency440Clip;
            receiverAudio.Stop();
            receiverAudio.time = 0f;
            receiverAudio.Play();
        }

        // الموجود عندك يبقى
        onResonanceOccurred?.Invoke();

        // الجديد: بعد ظهور النتيجة ننتظر ثم نظهر التفسير
        StartExplanationCountdown();
    }

    private void TriggerNoResonance()
    {
        Debug.Log("NO RESONANCE - Frequencies do not match");

        // الموجود عندك يبقى
        onNoResonance?.Invoke();

        // الجديد
        StartExplanationCountdown();
    }

    private void StartExplanationCountdown()
    {
        if (explanationCoroutine != null)
        {
            StopCoroutine(explanationCoroutine);
        }

        explanationCoroutine = StartCoroutine(ShowExplanationAfterDelay());
    }

    private IEnumerator ShowExplanationAfterDelay()
    {
        yield return new WaitForSeconds(explanationDelay);

        onExperimentCompleted?.Invoke();

        explanationCoroutine = null;

        Debug.Log("RESONANCE EXPERIMENT COMPLETED");
    }

    public void ResetExperiment()
    {
        StopAllCoroutines();

        explanationCoroutine = null;

        selectedFrequency = FrequencyChoice.None;
        strikeAllowed = false;
        strikeInProgress = false;

        if (sourceAudio != null)
        {
            sourceAudio.Stop();
        }

        if (receiverAudio != null)
        {
            receiverAudio.Stop();
        }

        Debug.Log("Resonance experiment reset.");
    }
}