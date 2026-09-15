using UnityEngine;

public class ArrowBounce : MonoBehaviour
{
    public float bounceSpeed = 4f; // سرعة النطوطة
    public float bounceHeight = 0.015f; // مسافة الارتفاع (سنتي ونص تقريباً)
    public bool rotateArrow = true; // تبغينه يلف حول نفسه؟
    public float rotationSpeed = 45f; // سرعة اللف

    private Vector3 startPos;

    void OnEnable()
    {
        // نحفظ مكان السهم الأساسي أول ما يشتغل
        startPos = transform.position;
    }

    void Update()
    {
        // حركة الارتفاع والانخفاض السلسة
        float newY = startPos.y + (Mathf.Sin(Time.time * bounceSpeed) * bounceHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // حركة الدوران
        if (rotateArrow)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}