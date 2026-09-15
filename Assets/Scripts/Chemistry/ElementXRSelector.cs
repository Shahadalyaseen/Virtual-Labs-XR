using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ElementXRSelector : MonoBehaviour
{
    [Header("References")]
    public ElementUIManager elementUIManager;
    public ElementModeManager elementModeManager;

    private XRSimpleInteractable simple;


    private void Start()
    {
        simple = GetComponent<XRSimpleInteractable>();

        if (simple != null)
        {
            simple.selectEntered.AddListener(SelectElement);
        }
        else
        {
            Debug.LogWarning(
                "XRSimpleInteractable not found on " +
                gameObject.name
            );
        }
    }


    private void OnDestroy()
    {
        if (simple != null)
        {
            simple.selectEntered.RemoveListener(SelectElement);
        }
    }


    public void SelectElement(SelectEnterEventArgs args)
    {
        // ==========================================
        // IMPORTANT:
        // لا تسمح بفتح العنصر إلا في Information Mode
        // ==========================================

        if (elementModeManager == null)
        {
            Debug.LogError(
                "ElementModeManager is not assigned on " +
                gameObject.name
            );

            return;
        }


        if (!elementModeManager.informationMode)
        {
            Debug.Log(
                "Element pressed but Information Mode is OFF: " +
                gameObject.name
            );

            return;
        }


        // ==========================================
        // التأكد من ElementUIManager
        // ==========================================

        if (elementUIManager == null)
        {
            Debug.LogError(
                "ElementUIManager is not assigned on " +
                gameObject.name
            );

            return;
        }


        // ==========================================
        // فتح معلومات العنصر
        // ==========================================

        string symbol = gameObject.name;

        Debug.Log(
            "XR SELECTED ELEMENT: " +
            symbol
        );

        elementUIManager.ShowElement(symbol);
    }
}