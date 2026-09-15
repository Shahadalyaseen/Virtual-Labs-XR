using UnityEngine;

public class JitterMotion : MonoBehaviour
{
    [Header("قائمة الجزيئات")]
    [Tooltip("حددي عدد العناصر واسحبي الكرات هنا")]
    public Transform[] crystals;

    [Header("إعدادات الحركة")]
    [Tooltip("سرعة صعود وهبوط الجزيئات")]
    public float speed = 2.0f;

    [Tooltip("مدى ارتفاع الحركة فوق وتحت")]
    public float height = 0.2f;

    private Vector3[] initialPositions;

    void Start()
    {
        if (crystals == null || crystals.Length == 0) return;

        // حفظ المواقع الأصلية لكل عنصر
        initialPositions = new Vector3[crystals.Length];
        for (int i = 0; i < crystals.Length; i++)
        {
            if (crystals[i] != null)
            {
                initialPositions[i] = crystals[i].localPosition;
            }
        }
    }

    void Update()
    {
        if (crystals == null || crystals.Length == 0) return;

        for (int i = 0; i < crystals.Length; i++)
        {
            if (crystals[i] != null)
            {
                // إعطاء كل عنصر زاوية (Phase) مختلفة بناءً على ترتيبه
                // هذا يجعل العناصر تتحرك عكس بعضها وفي موجة متناسقة
                float offset = i * (Mathf.PI / 2f);
                float newY = Mathf.Sin(Time.time * speed + offset) * height;

                // تطبيق الحركة على محور Y فوق وتحت
                crystals[i].localPosition = initialPositions[i] + new Vector3(0f, newY, 0f);
            }
        }
    }
}