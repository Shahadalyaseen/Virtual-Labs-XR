using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExperimentArrivalZone : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform xrOrigin;

    [Header("Navigation")]
    [SerializeField] private GameObject arrowsGroup;

    [Header("Audio Settings")]
    [SerializeField] private bool playOnlyOnce = true;

    private AudioSource explanationAudio;
    private bool hasPlayed = false;

    private void Awake()
    {
        explanationAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (xrOrigin == null)
        {
            Debug.LogWarning("XR Origin is not assigned.");
            return;
        }

        bool playerEntered =
            other.transform == xrOrigin ||
            other.transform.IsChildOf(xrOrigin);

        if (!playerEntered)
            return;

        // إخفاء أسهم التجربة
        if (arrowsGroup != null)
        {
            arrowsGroup.SetActive(false);
        }

        // تشغيل صوت شرح التجربة
        if (explanationAudio != null &&
            explanationAudio.clip != null)
        {
            if (!playOnlyOnce || !hasPlayed)
            {
                explanationAudio.Stop();
                explanationAudio.time = 0f;
                explanationAudio.Play();

                hasPlayed = true;
            }
        }

        Debug.Log(
            "PLAYER ARRIVED - ARROWS HIDDEN + EXPLANATION AUDIO PLAYED"
        );
    }

    public void ResetArrivalZone()
    {
        hasPlayed = false;

        if (explanationAudio != null)
        {
            explanationAudio.Stop();
            explanationAudio.time = 0f;
        }

        Debug.Log("ARRIVAL ZONE RESET");
    }
}