using UnityEngine;
using UnityEngine.Events;

public class MalletStrikeTrigger : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string sourceForkTag = "SourceFork";

    [Header("Settings")]
    [SerializeField] private float hitCooldown = 0.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent onForkHit;

    private float lastHitTime = -100f;

    private void OnTriggerEnter(Collider other)
    {
        GameObject target;

        if (other.attachedRigidbody != null)
        {
            target = other.attachedRigidbody.gameObject;
        }
        else
        {
            target = other.transform.root.gameObject;
        }

        if (!target.CompareTag(sourceForkTag))
            return;

        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;

        Debug.Log("SOURCE FORK HIT");

        onForkHit?.Invoke();
    }
}