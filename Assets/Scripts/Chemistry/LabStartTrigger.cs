using UnityEngine;

public class LabStartTrigger : MonoBehaviour
{
    [Header("Tutorial Manager Reference")]
    public LabTutorialFlow tutorialFlow;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;

            if (tutorialFlow != null)
            {
                tutorialFlow.StartLabFlow();
            }

            // إخفاء مجسم التريجر حتى لا يتكرر
            gameObject.SetActive(false);
        }
    }
}