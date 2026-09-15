using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ResonanceMallet : MonoBehaviour
{
    public ResonanceSourceFork leftSourceFork; // اسحبي الشوكة اليسار هنا
    public float minHitSpeed = 0.05f; // تقليل عتبة السرعة لتلتقط الضربات في الـ VR

    [Header("Wave Duration Settings")]
    [Tooltip("مدة استمرار تأثير الضربة بالثواني قبل السماح بضربة جديدة")]
    public float strikeCooldown = 3.0f;

    private XRGrabInteractable grab;
    private bool canStrike = true; // لمنع التكرار إذا علقت المطرقة بالشوكة

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject, collision.relativeVelocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        // دعم التصادم إذا كان الكوليدر مضبوطاً كـ Trigger
        HandleHit(other.gameObject, 1.0f);
    }

    private void HandleHit(GameObject hitObj, float hitSpeed)
    {
        if (!canStrike) return;

        bool isHeld = grab == null || grab.isSelected;

        // فحص التاج الخاص بالشوكة اليسرى على المجسم أو الكائن الأب
        bool isLeftFork = hitObj.CompareTag("leftFork") || 
                          hitObj.CompareTag("TuningForkLeft") ||
                          (hitObj.transform.root != null && (hitObj.transform.root.CompareTag("leftFork") || hitObj.transform.root.CompareTag("TuningForkLeft")));

        if (isHeld && isLeftFork)
        {
            if (hitSpeed >= minHitSpeed)
            {
                StartCoroutine(ExecuteStrikeRoutine());
            }
        }
    }

    private IEnumerator ExecuteStrikeRoutine()
    {
        canStrike = false;

        // استدعاء دالة الشوكة اليسرى للتحقق من التردد وقذف الكرة
        if (leftSourceFork != null)
        {
            leftSourceFork.OnHitByMallet();
        }

        yield return new WaitForSeconds(strikeCooldown);

        canStrike = true;
    }
}