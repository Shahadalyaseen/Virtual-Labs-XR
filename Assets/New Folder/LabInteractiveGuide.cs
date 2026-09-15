using UnityEngine;
using TMPro;

public class LabInteractiveGuide : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleDisplay;
    public TextMeshProUGUI instructionDisplay;

    [Header("Experiment Info")]
    public string experimentTitle = "EXPERIMENT TITLE";

    [TextArea(2, 6)]
    public string[] experimentSteps;

    [Header("Reset Setup (Tools)")]
    public GameObject[] labTools;

    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;

    private int currentStep = 0;

    void Start()
    {
        if (titleDisplay != null)
        {
            titleDisplay.text = experimentTitle;
        }

        // حفظ الأماكن والتدوير الابتدائي للأدوات
        if (labTools != null && labTools.Length > 0)
        {
            initialPositions = new Vector3[labTools.Length];
            initialRotations = new Quaternion[labTools.Length];

            for (int i = 0; i < labTools.Length; i++)
            {
                if (labTools[i] != null)
                {
                    initialPositions[i] = labTools[i].transform.position;
                    initialRotations[i] = labTools[i].transform.rotation;
                }
            }
        }

        ShowCurrentStep();
    }

    public void AdvanceStep(int stepNumber)
    {
        if (stepNumber == currentStep + 1)
        {
            currentStep = stepNumber;
            ShowCurrentStep();
        }
    }

    public void ForceStep(int stepIndex)
    {
        if (experimentSteps == null || experimentSteps.Length == 0)
            return;

        if (stepIndex < 0 || stepIndex >= experimentSteps.Length)
            return;

        currentStep = stepIndex;
        ShowCurrentStep();
    }

    // جديد:
    // يعرض آخر عنصر في Experiment Steps تلقائيًا
    public void ShowFinalExplanation()
    {
        if (experimentSteps == null || experimentSteps.Length == 0)
            return;

        currentStep = experimentSteps.Length - 1;
        ShowCurrentStep();

        Debug.Log("FINAL EXPERIMENT EXPLANATION SHOWN");
    }

    public void ResetLab()
    {
        currentStep = 0;
        ShowCurrentStep();

        if (labTools != null &&
            initialPositions != null &&
            initialRotations != null)
        {
            for (int i = 0; i < labTools.Length; i++)
            {
                if (labTools[i] != null)
                {
                    labTools[i].transform.position = initialPositions[i];
                    labTools[i].transform.rotation = initialRotations[i];

                    Rigidbody rb = labTools[i].GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
            }
        }
    }

    private void ShowCurrentStep()
    {
        if (instructionDisplay != null &&
            experimentSteps != null &&
            currentStep >= 0 &&
            currentStep < experimentSteps.Length)
        {
            instructionDisplay.text = experimentSteps[currentStep];
        }
    }
}