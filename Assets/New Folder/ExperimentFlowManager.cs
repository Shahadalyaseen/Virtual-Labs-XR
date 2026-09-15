using UnityEngine;
using TMPro;

public class ExperimentFlowManager : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // UI Text Component

    [Header("Step Tracking")]
    public bool isCandlePlaced = false;
    public bool isCandleLit = false;
    public bool isCupPlaced = false;

    void Start()
    {
        UpdateUI();
    }

    // Step 1: Triggered when candle is socketed
    public void OnCandlePlaced()
    {
        isCandlePlaced = true;
        UpdateUI();
    }

    // Step 2: Triggered when candle is ignited
    public void OnCandleLit()
    {
        if (isCandlePlaced)
        {
            isCandleLit = true;
            UpdateUI();
        }
    }

    // Step 3: Triggered when cup is socketed
    public void OnCupPlaced()
    {
        if (isCandleLit)
        {
            isCupPlaced = true;
            UpdateUI();
        }
    }

    // Dynamic UI Updates based on current progress
    void UpdateUI()
    {
        if (!isCandlePlaced)
        {
            instructionText.text = "Step 1: Place the candle onto the base holder.";
        }
        else if (!isCandleLit)
        {
            instructionText.text = "Step 2: Use the lighter to ignite the candle.";
        }
        else if (!isCupPlaced)
        {
            instructionText.text = "Step 3: Place the paper cup over the candle to enclose it.";
        }
        else
        {
            instructionText.text = "Scientific Explanation: Trapped air heats up and expands, generating lift force. As oxygen runs out, the flame extinguishes, cooling the air and causing the lantern to descend.";
        }
    }
}