using UnityEngine;

public class RightResonanceFork : MonoBehaviour
{
    [Header("Physics & Audio")]
    public Rigidbody pingPongBallRb;
    public AudioSource forkAudio;
    public float impulseForce = 2.0f;

    [Header("UI Guide Reference")]
    public LabInteractiveGuide guideCanvasPart2; // مرجع شاشة الجزء الثاني

    public void ReactToResonance()
    {
        // 1. تشغيل صوت الرنين
        if (forkAudio != null)
        {
            forkAudio.Play();
        }

        // 2. تفعيل فيزياء الكرة وقذفها
        if (pingPongBallRb != null)
        {
            pingPongBallRb.isKinematic = false;
            pingPongBallRb.useGravity = true;

            // توجيه الدفع للأمام مع زاوية خفيفة للأعلى لقوس حركي جميل
            Vector3 pushDirection = transform.forward + (Vector3.up * 0.2f);
            pingPongBallRb.AddForce(pushDirection.normalized * impulseForce, ForceMode.Impulse);
        }

        // 3. إتمام التجربة وانتقال الشاشة للخطوة الأخيرة (Element 3: Completed)
        if (guideCanvasPart2 != null)
        {
            guideCanvasPart2.AdvanceStep(3);
        }
    }
}