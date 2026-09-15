using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public class TutorialStep
{
    [Header("Step Text")]
    [TextArea]
    public string instructionText;

    [Header("Socket - Leave Empty For Object Step")]
    public XRSocketInteractor targetSocket;

    [Header("Object Interaction - Only For Step 5")]
    public GameObject interactionObject;

    [Header("Step Image")]
    public GameObject stepImage;

    [Header("Step Arrows")]
    public GameObject[] stepArrows;
}

public class TutorialManager : MonoBehaviour
{
    [Header("Start Menu UI")]
    public GameObject welcomeCanvas;
    public GameObject tutorialUI;

    [Header("End Menu UI")]
    public GameObject endCanvas;
    public string mainMenuSceneName = "MainMenuScene";

    [Header("Tutorial UI")]
    public TextMeshProUGUI uiText;

    [Header("IoT Settings")]
    public string espIP = "10.61.1.168";

    [Header("Power Button Object")]
    public GameObject powerButtonObject;

    [Header("Audio Sources")]
    public AudioSource welcomeAudioSource; // <-- أضفنا هذا المتغير للصوت الترحيبي
    public AudioSource snapAudioSource;
    public AudioSource buttonAudioSource;
    public AudioSource successAudioSource;

    [Header("Audio Clips")]
    public AudioClip snapSound;
    public AudioClip buttonClickSound;
    public AudioClip successSound;

    [Header("Tutorial Steps")]
    public TutorialStep[] steps;

    private int currentStepIndex = 0;
    private bool isLedOn = false;
    private bool isProcessingStep = false;


    private void Start()
    {
        currentStepIndex = 0;
        isProcessingStep = false;

        if (welcomeCanvas != null) welcomeCanvas.SetActive(true);
        if (tutorialUI != null) tutorialUI.SetActive(false);
        if (endCanvas != null) endCanvas.SetActive(false);
        if (powerButtonObject != null) powerButtonObject.SetActive(false);

        foreach (TutorialStep step in steps)
        {
            if (step.targetSocket != null) step.targetSocket.gameObject.SetActive(false);
            if (step.interactionObject != null) step.interactionObject.SetActive(false);
            if (step.stepImage != null) step.stepImage.SetActive(false);
            if (step.stepArrows != null)
            {
                foreach (GameObject arrow in step.stepArrows)
                {
                    if (arrow != null) arrow.SetActive(false);
                }
            }
        }
    }


    public void StartExperience()
    {
        // <-- هذا السطر اللي يوقف الصوت الترحيبي فوراً أول ما يضغط ستارت
        if (welcomeAudioSource != null && welcomeAudioSource.isPlaying)
        {
            welcomeAudioSource.Stop();
        }

        if (welcomeCanvas != null) welcomeCanvas.SetActive(false);
        if (tutorialUI != null) tutorialUI.SetActive(true);

        PlayButtonSound();

        currentStepIndex = 0;
        isProcessingStep = false;

        PlayCurrentStep();
    }


    private void PlayCurrentStep()
    {
        if (currentStepIndex >= steps.Length) return;

        TutorialStep currentStep = steps[currentStepIndex];

        if (uiText != null) uiText.text = currentStep.instructionText;

        HideAllStepImages();
        if (currentStep.stepImage != null) currentStep.stepImage.SetActive(true);

        HideAllArrows();
        if (currentStep.stepArrows != null)
        {
            foreach (GameObject arrow in currentStep.stepArrows)
            {
                if (arrow != null) arrow.SetActive(true);
            }
        }

        if (powerButtonObject != null) powerButtonObject.SetActive(false);

        if (currentStep.targetSocket != null)
        {
            currentStep.targetSocket.gameObject.SetActive(true);
        }
        else if (currentStep.interactionObject != null)
        {
            currentStep.interactionObject.SetActive(true);
        }
    }


    public void OnComponentPlaced()
    {
        if (isProcessingStep || currentStepIndex >= steps.Length) return;

        TutorialStep currentStep = steps[currentStepIndex];

        if (currentStep.targetSocket == null || !currentStep.targetSocket.hasSelection) return;

        isProcessingStep = true;
        PlaySnapSound();

        if (currentStep.stepArrows != null)
        {
            foreach (GameObject arrow in currentStep.stepArrows)
            {
                if (arrow != null) arrow.SetActive(false);
            }
        }

        IXRSelectInteractable selected = currentStep.targetSocket.firstInteractableSelected;

        if (selected != null)
        {
            StartCoroutine(LockPlacedObject(selected.transform.gameObject));
        }
        else
        {
            GoToNextStep();
        }
    }


    private IEnumerator LockPlacedObject(GameObject obj)
    {
        yield return new WaitForSeconds(0.25f);

        if (obj != null)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            XRGrabInteractable grab = obj.GetComponent<XRGrabInteractable>();
            if (grab != null) grab.enabled = false;
        }

        if (currentStepIndex < steps.Length)
        {
            XRSocketInteractor socket = steps[currentStepIndex].targetSocket;
            if (socket != null) socket.gameObject.SetActive(false);
        }

        GoToNextStep();
    }


    private void GoToNextStep()
    {
        currentStepIndex++;
        isProcessingStep = false;

        if (currentStepIndex < steps.Length) PlayCurrentStep();
        else FinishTutorial();
    }


    public void OnPowerButtonSelected()
    {
        if (currentStepIndex >= steps.Length) return;
        TutorialStep currentStep = steps[currentStepIndex];

        if (currentStep.interactionObject == null ||
           (powerButtonObject != null && currentStep.interactionObject != powerButtonObject)) return;

        PlayButtonSound();
        ToggleLED();
    }


    public void ToggleLED()
    {
        isLedOn = !isLedOn;
        StartCoroutine(SendIoTCommand(isLedOn));
    }


    private IEnumerator SendIoTCommand(bool turnOn)
    {
        string command = turnOn ? "ledon" : "ledoff";
        string url = "http://" + espIP + "/" + command;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) Debug.Log("IoT Command Sent: " + command);
            else Debug.LogError("ESP Connection Error: " + request.error);
        }
    }


    private void FinishTutorial()
    {
        HideAllStepImages();
        HideAllArrows();

        if (powerButtonObject != null) powerButtonObject.SetActive(false);
        if (uiText != null) uiText.text = "Great job! The circuit is complete.";
        if (endCanvas != null) endCanvas.SetActive(true);

        PlaySuccessSound();
    }


    private void HideAllStepImages()
    {
        foreach (TutorialStep step in steps)
        {
            if (step.stepImage != null) step.stepImage.SetActive(false);
        }
    }


    private void HideAllArrows()
    {
        foreach (TutorialStep step in steps)
        {
            if (step.stepArrows != null)
            {
                foreach (GameObject arrow in step.stepArrows)
                {
                    if (arrow != null) arrow.SetActive(false);
                }
            }
        }
    }


    private void PlaySnapSound()
    {
        if (snapAudioSource != null && snapSound != null) snapAudioSource.PlayOneShot(snapSound);
    }

    private void PlayButtonSound()
    {
        if (buttonAudioSource != null && buttonClickSound != null) buttonAudioSource.PlayOneShot(buttonClickSound);
    }

    private void PlaySuccessSound()
    {
        if (successAudioSource != null && successSound != null) successAudioSource.PlayOneShot(successSound);
    }

    public void RestartExperience()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        PlayButtonSound();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}