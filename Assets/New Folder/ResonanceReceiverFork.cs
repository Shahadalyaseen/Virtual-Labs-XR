using UnityEngine;

public class ResonanceReceiverFork : MonoBehaviour
{
    public AudioSource receiverAudio;
    public Rigidbody pingPongBallRb;
    public Vector3 kickDirection = new Vector3(1f, 0.4f, 0f);
    public float kickForce = 0.35f;

    public void OnResonanceTriggered()
    {
        if (receiverAudio != null)
            receiverAudio.Play();

        if (pingPongBallRb != null)
        {
            pingPongBallRb.AddForce(kickDirection.normalized * kickForce, ForceMode.Impulse);
        }
    }
}