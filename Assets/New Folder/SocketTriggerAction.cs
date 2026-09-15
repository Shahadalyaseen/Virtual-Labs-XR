using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTriggerAction : MonoBehaviour
{
    [Header("النقاط أو المجسمات المراد تحريكها")]
    public Transform attachPoint1;
    public Transform attachPoint2;
    public Transform object3;

    [Header("إعدادات الصعود")]
    [Tooltip("مسافة صعود الكوب")]
    public float moveUpDistance = 0.5f;

    [Tooltip("مدة الصعود")]
    public float moveDuration = 2.0f;

    [Tooltip("مدة الانتظار قبل بدء الحركة")]
    public float delayBeforeMove = 3.0f;

    [Header("إعدادات الميلان")]
    [Tooltip("مقدار الميلان يمين ويسار بالدرجات")]
    public float rotationAmount = 8f;

    [Tooltip("سرعة الميلان")]
    public float rotationSpeed = 2f;

    [Tooltip("مدة الميلان قبل السقوط")]
    public float rotationDuration = 1.5f;

    [Header("إعدادات النزول")]
    [Tooltip("مدة النزول - كلما قل الرقم كان السقوط أسرع")]
    public float fallDuration = 0.4f;

    public void OnSocketEntered(SelectEnterEventArgs args)
    {
        StartCoroutine(AnimateUpRotateAndFall());
    }

    private System.Collections.IEnumerator AnimateUpRotateAndFall()
    {
        // الانتظار قبل الحركة
        yield return new WaitForSeconds(delayBeforeMove);

        // حفظ المكان والدوران الأصلي
        Vector3 startPos1 = attachPoint1 != null ? attachPoint1.position : Vector3.zero;
        Vector3 startPos2 = attachPoint2 != null ? attachPoint2.position : Vector3.zero;
        Vector3 startPos3 = object3 != null ? object3.position : Vector3.zero;

        Quaternion startRot1 = attachPoint1 != null ? attachPoint1.rotation : Quaternion.identity;
        Quaternion startRot2 = attachPoint2 != null ? attachPoint2.rotation : Quaternion.identity;
        Quaternion startRot3 = object3 != null ? object3.rotation : Quaternion.identity;

        // تحديد مكان الأعلى
        Vector3 topPos1 = startPos1 + Vector3.up * moveUpDistance;
        Vector3 topPos2 = startPos2 + Vector3.up * moveUpDistance;
        Vector3 topPos3 = startPos3 + Vector3.up * moveUpDistance;

        // ==========================================
        // 1 - الصعود
        // ==========================================

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            if (attachPoint1 != null)
                attachPoint1.position = Vector3.Lerp(startPos1, topPos1, smoothT);

            if (attachPoint2 != null)
                attachPoint2.position = Vector3.Lerp(startPos2, topPos2, smoothT);

            if (object3 != null)
                object3.position = Vector3.Lerp(startPos3, topPos3, smoothT);

            yield return null;
        }

        // التأكد من الوصول للأعلى
        if (attachPoint1 != null)
            attachPoint1.position = topPos1;

        if (attachPoint2 != null)
            attachPoint2.position = topPos2;

        if (object3 != null)
            object3.position = topPos3;

        // ==========================================
        // 2 - الميل يمين ويسار
        // ==========================================

        elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;

            float angle = Mathf.Sin(elapsedTime * rotationSpeed * Mathf.PI * 2f)
                         * rotationAmount;

            Quaternion tilt = Quaternion.Euler(0f, 0f, angle);

            if (attachPoint1 != null)
                attachPoint1.rotation = startRot1 * tilt;

            if (attachPoint2 != null)
                attachPoint2.rotation = startRot2 * tilt;

            if (object3 != null)
                object3.rotation = startRot3 * tilt;

            yield return null;
        }

        // ==========================================
        // 3 - الرجوع للوضع الطبيعي قبل السقوط
        // ==========================================

        if (attachPoint1 != null)
            attachPoint1.rotation = startRot1;

        if (attachPoint2 != null)
            attachPoint2.rotation = startRot2;

        if (object3 != null)
            object3.rotation = startRot3;

        // ==========================================
        // 4 - السقوط
        // ==========================================

        elapsedTime = 0f;

        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fallDuration);

            // سقوط سريع في البداية ثم تباطؤ بسيط
            float fallT = t * t;

            if (attachPoint1 != null)
                attachPoint1.position = Vector3.Lerp(topPos1, startPos1, fallT);

            if (attachPoint2 != null)
                attachPoint2.position = Vector3.Lerp(topPos2, startPos2, fallT);

            if (object3 != null)
                object3.position = Vector3.Lerp(topPos3, startPos3, fallT);

            yield return null;
        }

        // ==========================================
        // 5 - التأكد من الرجوع للمكان الأصلي
        // ==========================================

        if (attachPoint1 != null)
        {
            attachPoint1.position = startPos1;
            attachPoint1.rotation = startRot1;
        }

        if (attachPoint2 != null)
        {
            attachPoint2.position = startPos2;
            attachPoint2.rotation = startRot2;
        }

        if (object3 != null)
        {
            object3.position = startPos3;
            object3.rotation = startRot3;
        }
    }
}