using UnityEngine;
using System.Collections;

public class CandleIgnite : MonoBehaviour
{
    public GameObject flame;        // مجسم النار
    public GameObject smoke;        // مجسم الدخان
    public Light candleLight;       // إضاءة الشمعة
    public float smokeDelay = 1.5f; // وقت تأخير الدخان بالثواني

    // اسم مجسم الولاعة أو الكولايدر الخاص بها في الـ Hierarchy
    public string lighterObjectName = "Vefects Candle 02"; 

    private bool isLit = false;

    private void OnTriggerEnter(Collider other)
    {
        // التحقق من أن الكولايدر الملامس يتبع مجسم الولاعة تحديداً
        if ((other.gameObject.name == lighterObjectName || other.transform.root.name == lighterObjectName) && !isLit)
        {
            isLit = true;
            
            // 1. تشغيل النار والإضاءة فوراً
            if (flame != null) flame.SetActive(true);
            if (candleLight != null) candleLight.enabled = true;

            // 2. تشغيل الدخان بعد الوقت المحدد
            StartCoroutine(StartSmoke());
        }
    }

    IEnumerator StartSmoke()
    {
        yield return new WaitForSeconds(smokeDelay);
        if (smoke != null) smoke.SetActive(true);
    }
    // دالة إطفاء الشمعة عند انتهاء الأكسجين
public void Extinguish()
{
    isLit = false;
    if (flame != null) flame.SetActive(false);
    if (candleLight != null) candleLight.enabled = false;
}
}