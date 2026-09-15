using System.Collections;
using UnityEngine;
using TMPro;

public class LabTutorialFlow : MonoBehaviour
{
    [System.Serializable]
    public class HighlightTimedTrigger
    {
        [Tooltip("المجسم المراد إضاءته (مثل نور زر Information)")]
        public GameObject highlightObject;
        
        [Tooltip("زمن التأخير قبل بدء الومضات بالثواني")]
        public float delayToEnable = 1.0f;
        
        [Tooltip("مدة إضاءة النور في الومضة الواحدة بالثواني")]
        public float blinkOnDuration = 0.3f;

        [Tooltip("مدة إطفاء النور في الومضة الواحدة بالثواني")]
        public float blinkOffDuration = 0.3f;

        [Tooltip("عدد مرات التكرار (الومضات)")]
        public int blinkCount = 3;
    }

    [System.Serializable]
    public class TutorialStep
    {
        [Tooltip("مجموعة الأسهم الخاصة بهذه المرحلة")]
        public GameObject arrowGroup;

        [Tooltip("قائمة الأضواء/الأسهم المراد إضاءتها")]
        public HighlightTimedTrigger[] wordHighlights;

        [Tooltip("الكانفس الذي سيظهر للمرحلة")]
        public GameObject stepCanvas;

        [Tooltip("مربع النص الذي سيتم كتابة الكلام فيه بتأثير الآلة الكاتبة")]
        public TextMeshProUGUI stepTextUI;

        [TextArea(3, 5)]
        [Tooltip("النص الذي سيتم كتابته حرفاً بحرف")]
        public string textContent;
        
        [Tooltip("الصوت الخاص بهذه المرحلة (شرح/كلام)")]
        public AudioClip stepAudio;

        [HideInInspector]
        public CanvasGroup canvasGroup;
    }

    [Header("Audio Clips")]
    public AudioClip startAudio;       // صوت التنبيه الأول فور تصادم اللاعب بالـ Trigger
    public AudioClip completionAudio;  // صوت التهنئة النهائي عند انتهاء جميع المراحل

    [Header("Tutorial Steps (3 Steps)")]
    public TutorialStep[] steps = new TutorialStep[3];

    [Header("Settings")]
    [Tooltip("سرعة كتابة الأحرف بالثواني")]
    public float typingSpeed = 0.04f;
    [Tooltip("وقت الانتظار بالثواني بعد انتهاء الصوت والكلام قبل التلاشي")]
    public float waitDelayAfterFinish = 2.0f;
    [Tooltip("سرعة التلاشي (الظهور والإخفاء) بالثواني")]
    public float fadeDuration = 0.8f;

    private AudioSource _audioSource;
    private int _currentStepIndex = 0;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.Stop();

        InitAllSteps();
    }

    void InitAllSteps()
    {
        if (steps == null) return;

        foreach (var step in steps)
        {
            if (step == null) continue;

            if (step.arrowGroup != null) step.arrowGroup.SetActive(false);
            
            if (step.wordHighlights != null)
            {
                foreach (var h in step.wordHighlights)
                {
                    if (h != null && h.highlightObject != null)
                        h.highlightObject.SetActive(false);
                }
            }
            
            if (step.stepCanvas != null)
            {
                step.canvasGroup = step.stepCanvas.GetComponent<CanvasGroup>();
                if (step.canvasGroup == null)
                    step.canvasGroup = step.stepCanvas.AddComponent<CanvasGroup>();

                step.canvasGroup.alpha = 0f;
                step.stepCanvas.SetActive(false);
            }

            if (step.stepTextUI != null) step.stepTextUI.text = "";
        }
    }

    /// <summary>
    /// يتم استدعاؤها فور التصادم بالـ Trigger
    /// </summary>
    public void StartLabFlow()
    {
        StartCoroutine(StartFlowRoutine());
    }

    private IEnumerator StartFlowRoutine()
    {
        // 1. تشغيل صوت البداية أولاً (إن وجد)
        if (startAudio != null && _audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = startAudio;
            _audioSource.Play();

            // الانتظار حتى ينتهي صوت البداية تماماً لمنع التداخل
            while (_audioSource.isPlaying)
            {
                yield return null;
            }
        }

        // 2. الانطلاق بالخطوات واحدة تلو الأخرى
        _currentStepIndex = 0;
        StartCoroutine(PlayStepSequence(_currentStepIndex));
    }

    private IEnumerator PlayStepSequence(int index)
    {
        var currentStep = steps[index];
        if (currentStep == null) yield break;

        // إظهار أسهم المرحلة
        if (currentStep.arrowGroup != null)
            currentStep.arrowGroup.SetActive(true);

        // إظهار الكانفس بالتلاشي
        if (currentStep.stepCanvas != null)
        {
            currentStep.stepCanvas.SetActive(true);
            yield return StartCoroutine(FadeCanvas(currentStep.canvasGroup, 0f, 1f, fadeDuration));
        }

        // تشغيل النور الومّاض
        if (currentStep.wordHighlights != null)
        {
            foreach (var h in currentStep.wordHighlights)
            {
                if (h != null && h.highlightObject != null)
                    StartCoroutine(HandleHighlightTiming(h));
            }
        }

        // تشغيل صوت الكلام الخاص بهذه المرحلة حصراً
        if (currentStep.stepAudio != null && _audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = currentStep.stepAudio;
            _audioSource.Play();
        }

        // تأثير الكتابة
        if (currentStep.stepTextUI != null && !string.IsNullOrEmpty(currentStep.textContent))
        {
            currentStep.stepTextUI.text = "";
            foreach (char letter in currentStep.textContent.ToCharArray())
            {
                currentStep.stepTextUI.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // الانتظار الجبري لحين انتهاء صوت هذه المرحلة
        if (_audioSource != null && _audioSource.isPlaying)
        {
            while (_audioSource.isPlaying)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(waitDelayAfterFinish);

        // إخفاء الكانفس والأسهم
        if (currentStep.stepCanvas != null)
        {
            yield return StartCoroutine(FadeCanvas(currentStep.canvasGroup, 1f, 0f, fadeDuration));
            currentStep.stepCanvas.SetActive(false);
        }

        if (currentStep.arrowGroup != null)
            currentStep.arrowGroup.SetActive(false);

        _currentStepIndex++;

        // الانتقال للمرحلة القادمة
        if (_currentStepIndex < steps.Length)
        {
            StartCoroutine(PlayStepSequence(_currentStepIndex));
        }
        else if (completionAudio != null && _audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(completionAudio);
        }
    }

    private IEnumerator HandleHighlightTiming(HighlightTimedTrigger h)
    {
        yield return new WaitForSeconds(h.delayToEnable);

        for (int i = 0; i < h.blinkCount; i++)
        {
            if (h.highlightObject != null)
                h.highlightObject.SetActive(true);

            yield return new WaitForSeconds(h.blinkOnDuration);

            if (h.highlightObject != null)
                h.highlightObject.SetActive(false);

            yield return new WaitForSeconds(h.blinkOffDuration);
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        if (cg == null) yield break;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }
        cg.alpha = endAlpha;
    }
}