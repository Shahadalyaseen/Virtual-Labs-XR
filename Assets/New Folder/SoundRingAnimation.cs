using UnityEngine;

public class SoundRingAnimation : MonoBehaviour
{
    [Header("Scale Expansion")]
    public Vector3 startScale = new Vector3(0.05f, 0.05f, 0.05f); // تبدأ صغيرة جداً
    public Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f);       // تتوسع في الهواء

    [Header("Movement Settings")]
    public float moveSpeed = 1.2f;      // سرعة الانطلاق للأمام نحو الشوكة المقابلة
    public float lifetime = 0.9f;       // مدة بقاء الحلقة قبل أن تختفي
    public Vector3 moveDirection = Vector3.forward; // اتجاه الحركة للأمام

    private float timer = 0f;
    private Material matInstance;

    void Awake()
    {
        transform.localScale = startScale;
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            matInstance = rend.material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / lifetime);

        // 1. التوسع في الحجم من الصغير إلى الكبير
        transform.localScale = Vector3.Lerp(startScale, endScale, progress);

        // 2. التحرك في الهواء مبتعدة عن الشوكة
        transform.position += transform.TransformDirection(moveDirection) * (moveSpeed * Time.deltaTime);

        // 3. التلاشي التدريجي للشفافية
        if (matInstance != null && matInstance.HasProperty("_BaseColor"))
        {
            Color c = matInstance.GetColor("_BaseColor");
            c.a = Mathf.Lerp(0.6f, 0f, progress);
            matInstance.SetColor("_BaseColor", c);
        }

        // 4. الحذف التلقائي بعد انتهاء المدة
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}