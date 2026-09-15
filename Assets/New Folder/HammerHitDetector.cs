using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[Serializable]
public class FloatEvent : UnityEvent<float> { }

[RequireComponent(typeof(Rigidbody))]
public class HammerHitDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tuningForkRoot;
    [SerializeField] private XRGrabInteractable grabInteractable;

    [Header("Hit Settings")]
    [SerializeField] private float minimumHitSpeed = 0.6f;
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private bool requireHammerToBeGrabbed = true;

    [Header("Events")]
    [SerializeField] private FloatEvent onValidHit;

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    private float lastHitTime = -100f;

    private void Awake()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tuningForkRoot == null)
        {
            if (showDebugMessages)
            {
                Debug.LogWarning(
                    "HammerHitDetector: Tuning Fork Root is not assigned."
                );
            }

            return;
        }

        Transform hitObject = collision.collider.transform;

        bool hitTuningFork =
            hitObject == tuningForkRoot ||
            hitObject.IsChildOf(tuningForkRoot);

        if (!hitTuningFork)
            return;

        if (requireHammerToBeGrabbed)
        {
            if (grabInteractable == null || !grabInteractable.isSelected)
                return;
        }

        if (Time.time - lastHitTime < hitCooldown)
            return;

        float hitSpeed = collision.relativeVelocity.magnitude;

        if (hitSpeed < minimumHitSpeed)
        {
            if (showDebugMessages)
            {
                Debug.Log(
                    "Tuning fork touched, but hit was too weak. Speed = "
                    + hitSpeed
                );
            }

            return;
        }

        lastHitTime = Time.time;

        if (showDebugMessages)
        {
            Debug.Log(
                "VALID TUNING FORK HIT! Speed = "
                + hitSpeed
            );
        }

        onValidHit?.Invoke(hitSpeed);
    }
}