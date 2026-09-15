using UnityEngine;
using TMPro;

public class FlyingCupGuide : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private TextMeshProUGUI instructionDisplay;

    [Header("Experiment Title")]
    [SerializeField] private string experimentTitle = "Flying Cup Experiment";

    [Header("Experiment Steps")]

    [TextArea(2, 4)]
    [SerializeField] private string step1 =
        "Step 1: Place the candle in the correct position.";

    [TextArea(2, 4)]
    [SerializeField] private string step2 =
        "Step 2: Use the lighter to light the candle.";

    [TextArea(2, 4)]
    [SerializeField] private string step3 =
        "Step 3: Place the cup correctly over the candle.";

    [TextArea(2, 4)]
    [SerializeField] private string watchEffectStep =
        "Watch the effect!";

    [TextArea(4, 8)]
    [SerializeField] private string finalStep =
        "Experiment Completed! ✓\n\n" +
        "Why did the cup rise?\n" +
        "Heating the air makes it expand and become less dense. " +
        "The surrounding cooler air creates an upward buoyant force. " +
        "When the buoyant force becomes greater than the cup’s weight, the cup rises.";

    private int currentStep = 0;

    private void Start()
    {
        if (titleDisplay != null)
            titleDisplay.text = experimentTitle;

        ShowCurrentStep();
    }

    // Step 1 completed: candle placed
    public void CandlePlaced()
    {
        if (currentStep != 0)
            return;

        currentStep = 1;
        ShowCurrentStep();

        Debug.Log("STEP 1 COMPLETE - CANDLE PLACED");
    }

    // Step 2 completed: candle lit
    public void CandleLit()
    {
        if (currentStep != 1)
            return;

        currentStep = 2;
        ShowCurrentStep();

        Debug.Log("STEP 2 COMPLETE - CANDLE LIT");
    }

    // Step 3 completed: cup placed correctly
    public void CupPlaced()
    {
        if (currentStep != 2)
            return;

        currentStep = 3;
        ShowCurrentStep();

        Debug.Log("STEP 3 COMPLETE - CUP PLACED");
    }

    // Call this ONLY when the cup actually rises
    public void ExperimentCompleted()
    {
        if (currentStep != 3)
            return;

        currentStep = 4;
        ShowCurrentStep();

        Debug.Log("EXPERIMENT COMPLETE - CUP RAISED");
    }

    public void ResetGuide()
    {
        currentStep = 0;
        ShowCurrentStep();

        Debug.Log("FLYING CUP GUIDE RESET");
    }

    private void ShowCurrentStep()
    {
        if (instructionDisplay == null)
            return;

        switch (currentStep)
        {
            case 0:
                instructionDisplay.text = step1;
                break;

            case 1:
                instructionDisplay.text = step2;
                break;

            case 2:
                instructionDisplay.text = step3;
                break;

            case 3:
                instructionDisplay.text = watchEffectStep;
                break;

            case 4:
                instructionDisplay.text = finalStep;
                break;
        }
    }
}