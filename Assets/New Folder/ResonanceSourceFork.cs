using UnityEngine;
using TMPro;

public class ResonanceSourceFork : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource forkAudio;

    [Header("Frequency Settings")]
    public float currentFrequency = 440f; 
    public TextMeshProUGUI activeFreqDisplay;

    [Header("Receiver Fork (Second Fork)")]
    public RightResonanceFork receiverFork;

    void Start()
    {
        UpdateDisplayText();
    }

    public void SelectFrequency440()
    {
        currentFrequency = 440f;
        UpdateDisplayText();
    }

    public void SelectFrequency512()
    {
        currentFrequency = 512f;
        UpdateDisplayText();
    }

    public void OnHitByMallet()
    {
        if (forkAudio != null)
        {
            forkAudio.pitch = currentFrequency / 440f; 
            forkAudio.Play();
        }

        if (Mathf.Approximately(currentFrequency, 440f))
        {
            if (receiverFork != null)
            {
                receiverFork.ReactToResonance();
            }
        }
        else
        {
            Debug.Log("512 Hz: لا يحدث رنين");
        }
    }

    void UpdateDisplayText()
    {
        if (activeFreqDisplay != null)
            activeFreqDisplay.text = "Freq: " + currentFrequency + " Hz";
    }
}