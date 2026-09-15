using System.Collections;
using UnityEngine;

public class ReactionEffect : MonoBehaviour
{
    [Header("Components")]
    public Transform basePlate;          // مجسم الصحن الدوار
    public ParticleSystem beamParticles; // نظام الجسيمات للشعاع
    public Transform spawnPoint;         // مكان ظهور البريفاب الناتج

    [Header("Settings")]
    public float rotationSpeed = 40f;    // سرعة دوران المجسم (بشويش)
    public float floatSpeed = 2f;        // سرعة الصعود والهبوط
    public float floatAmount = 0.08f;    // مسافة الحركة فوق وتحت

    private GameObject _currentProduct;  
    private Coroutine _activeReaction;   

    void Start()
    {
        if (beamParticles != null && !beamParticles.isPlaying)
        {
            beamParticles.Play();
        }
    }

    public void TriggerSynthesis(GameObject resultPrefab)
    {
        // إيقاف التفاعل السابق وتدمير المجسم القديم فوراً
        if (_activeReaction != null)
            StopCoroutine(_activeReaction);

        if (_currentProduct != null)
            Destroy(_currentProduct);

        if (resultPrefab != null)
        {
            _activeReaction = StartCoroutine(StartReactionRoutine(resultPrefab));
        }
    }

    private IEnumerator StartReactionRoutine(GameObject resultPrefab)
    {
        // 1. إنشاء المجسم الجديد
        Transform targetPoint = spawnPoint != null ? spawnPoint : transform;
        _currentProduct = Instantiate(resultPrefab, targetPoint.position, targetPoint.rotation, targetPoint);
        _currentProduct.transform.localPosition = Vector3.zero;
        _currentProduct.SetActive(true);

        Vector3 startPos = _currentProduct.transform.localPosition;

        // 2. تدوير الصحن (اختياري)
        if (basePlate != null)
        {
            basePlate.Rotate(Vector3.up * 30f * Time.deltaTime);
        }

        // 3. حلقة حركية مستمرة: دوران المجسم مع صعود وهبوط ناعم
        while (_currentProduct != null)
        {
            // حركة الدوران
            _currentProduct.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

            // حركة الفوق والتحت بشويش باستعمال الموجة الجيبية (Sin)
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            _currentProduct.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

            yield return null;
        }
    }
}