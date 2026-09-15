using UnityEngine;

public class ElementClick : MonoBehaviour
{
    [Header("Mode Manager")]
    public ElementModeManager modeManager;

    [Header("Element Information")]
    public GameObject elementInformationUI;


    public void SelectElement()
    {
        // إذا Information غير مفعّل، لا تسوي أي شيء
        if (modeManager == null)
        {
            Debug.LogWarning("ElementClick: Mode Manager is not assigned.");
            return;
        }

        if (!modeManager.informationMode)
        {
            Debug.Log("Information Mode is OFF. Element cannot be opened.");
            return;
        }

        // إظهار معلومات العنصر
        if (elementInformationUI != null)
        {
            elementInformationUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Element Information UI is not assigned.");
        }
    }


    public void CloseElementInformation()
    {
        if (elementInformationUI != null)
            elementInformationUI.SetActive(false);
    }
}