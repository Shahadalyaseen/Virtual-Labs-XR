using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MalletStrike : MonoBehaviour
{
    public AcousticLevitationLab forkLab;
    public LabInteractiveGuide guideCanvas; // مرجع شاشة التعليمات
    public float minHitSpeed = 0.05f;

    [Header("Wave Duration Settings")]
    [Tooltip("مدة استمرار الموجات الصوتية بالثواني قبل السماح بضربة جديدة")]
    public float strikeCooldown = 3.0f; 
    
    private XRGrabInteractable grabInteractable;
    private bool canStrike = true; // لمنع التكرار إذا علقت المطرقة

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject, collision.relativeVelocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject, 1.0f);
    }

    private void HandleHit(GameObject hitObj, float hitSpeed)
    {
        // إذا كانت الضربة ما زالت مستمرة، تجاهل أي تلامس جديد
        if (!canStrike) return;

        bool isHeld = grabInteractable == null || grabInteractable.isSelected;

        bool isFork = hitObj.CompareTag("TuningFork") || 
                      hitObj.CompareTag("tuningFork") || 
                      (hitObj.transform.root != null && (hitObj.transform.root.CompareTag("TuningFork") || hitObj.transform.root.CompareTag("tuningFork")));

        if (isHeld && isFork)
        {
            if (hitSpeed >= minHitSpeed)
            {
                // بدء تشغيل دورة الضربة الواحدة
                StartCoroutine(ExecuteStrikeRoutine());
            }
        }
    }

    private IEnumerator ExecuteStrikeRoutine()
    {
        canStrike = false; // قفل استقبال ضربات جديدة

        // 1. تشغيل الموجات والصوت لمرة واحدة
        if (forkLab != null)
        {
            forkLab.TriggerSoundWave();
        }

        // 2. تحديث الشاشة
        if (guideCanvas != null)
        {
            guideCanvas.AdvanceStep(2);
        }

        // 3. الانتظار للمدة المحددة (مثلاً 3 ثوانٍ)
        yield return new WaitForSeconds(strikeCooldown);

        // 4. السماح بضربة جديدة بعد انتهاء المدة
        canStrike = true;
    }
}