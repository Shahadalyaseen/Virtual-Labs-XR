using UnityEngine;

public class ElementModeManager : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject mainCanvas;

    [Header("Reaction UI")]
    public GameObject reactionUI;

    [Header("Element Information")]
    public GameObject informationUI;

    [Header("All Element Information UIs")]
    public GameObject[] allElementInformationUIs;

    [Header("Reaction System")]
    public Chemistry3DLabManager chemistryLabManager;

    [HideInInspector]
    public bool informationMode = false;

    [HideInInspector]
    public bool reactionMode = false;

    private void Start()
    {
        informationMode = false;
        reactionMode = false;

        CloseAllElementInformation();
        ResetReactionSelection();

        if (informationUI != null)
            informationUI.SetActive(false);

        if (reactionUI != null)
            reactionUI.SetActive(false);
    }

    // زر Information
    public void OpenInformation()
    {
        informationMode = true;
        reactionMode = false;

        // يلغي أي عناصر كانت مختارة للتفاعل.
        ResetReactionSelection();

        if (reactionUI != null)
            reactionUI.SetActive(false);

        if (informationUI != null)
            informationUI.SetActive(true);

        CloseAllElementInformation();

        Debug.Log("INFORMATION MODE ON - Select an element.");
    }

    // زر Reactions
    public void OpenReaction()
    {
        informationMode = false;
        reactionMode = true;

        // يبدأ تفاعل جديد ونظيف.
        ResetReactionSelection();

        CloseAllElementInformation();

        if (informationUI != null)
            informationUI.SetActive(false);

        if (reactionUI != null)
            reactionUI.SetActive(true);

        Debug.Log("REACTION MODE ON - Select elements, then press Mix.");
    }

    public void CloseAllElementInformation()
    {
        if (allElementInformationUIs == null)
            return;

        foreach (GameObject ui in allElementInformationUIs)
        {
            if (ui != null)
                ui.SetActive(false);
        }
    }

    public void CloseInformation()
    {
        informationMode = false;

        CloseAllElementInformation();

        if (informationUI != null)
            informationUI.SetActive(false);

        Debug.Log("INFORMATION MODE OFF");
    }

    public void CloseReaction()
    {
        reactionMode = false;

        ResetReactionSelection();

        if (reactionUI != null)
            reactionUI.SetActive(false);

        Debug.Log("REACTION MODE OFF");
    }

    public void ExitUI()
    {
        informationMode = false;
        reactionMode = false;

        CloseAllElementInformation();
        ResetReactionSelection();

        if (informationUI != null)
            informationUI.SetActive(false);

        if (reactionUI != null)
            reactionUI.SetActive(false);

        if (mainCanvas != null)
            mainCanvas.SetActive(false);

        Debug.Log("EXIT UI");
    }

    private void ResetReactionSelection()
    {
        if (chemistryLabManager != null)
            chemistryLabManager.ClearCounters();
    }
}