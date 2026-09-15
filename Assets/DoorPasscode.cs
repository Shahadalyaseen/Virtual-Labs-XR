using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorPasscode : MonoBehaviour
{
    [Header("Proximity")]
    public Collider proximityTrigger;
    public string playerTag = "Player";

    [Header("Right Door Animator")]
    public Animator rightDoorAnim;
    public string rightDoorStateName = "right door";

    [Header("Left Door Transform")]
    public Transform leftDoorTransform;
    public float leftSlideDistance = 1.0f;
    public float leftSlideSpeed = 3.0f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSFX;
    public AudioClip closeSFX;

    private Vector3 leftClosedPos;
    private Vector3 leftOpenPos;
    private bool isOpen = false;

    private readonly HashSet<int> playerCollidersInRange = new HashSet<int>();

    private void Awake()
    {
        if (proximityTrigger == null)
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>(true))
            {
                if (collider.isTrigger)
                {
                    proximityTrigger = collider;
                    break;
                }
            }
        }

        Rigidbody rigidbodyComponent = GetComponent<Rigidbody>();
        if (rigidbodyComponent == null)
            rigidbodyComponent = gameObject.AddComponent<Rigidbody>();

        rigidbodyComponent.isKinematic = true;
        rigidbodyComponent.useGravity = false;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (leftDoorTransform != null)
        {
            leftClosedPos = leftDoorTransform.localPosition;
            leftOpenPos = leftClosedPos + Vector3.left * leftSlideDistance;
        }

        if (rightDoorAnim != null)
            rightDoorAnim.enabled = false;
    }

    private void Update()
    {
        if (leftDoorTransform == null) return;

        Vector3 targetPos = isOpen ? leftOpenPos : leftClosedPos;
        leftDoorTransform.localPosition = Vector3.MoveTowards(leftDoorTransform.localPosition, targetPos, Time.deltaTime * leftSlideSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        if (playerCollidersInRange.Add(other.GetInstanceID()) && playerCollidersInRange.Count == 1)
            OpenDoors();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;

        if (playerCollidersInRange.Remove(other.GetInstanceID()) && playerCollidersInRange.Count == 0)
            CloseDoors();
    }

    public void OpenDoors()
    {
        isOpen = true;
        PlayAnimation(rightDoorAnim, rightDoorStateName);
        PlayClip(openSFX);
    }

    public void CloseDoors()
    {
        isOpen = false;
        PlayAnimation(rightDoorAnim, rightDoorStateName);
        PlayClip(closeSFX);
    }

    private bool IsPlayer(Collider other)
    {
        if (other == null) return false;
        return other.transform.root.CompareTag(playerTag) || 
               other.CompareTag("MainCamera") || 
               other.GetComponentInParent<CharacterController>() != null;
    }

    private static void PlayAnimation(Animator animator, string stateName)
    {
        if (animator == null) return;
        animator.enabled = true;
        animator.Play(stateName, 0, 0f);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}