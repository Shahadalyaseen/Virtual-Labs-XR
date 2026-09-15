using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ToolGrabStepTrigger : MonoBehaviour
{
    public LabInteractiveGuide guideCanvas;
    [Tooltip("رقم فهرس الخطوة التالية في المصفوفة (1 تعني الخطوة الثانية)")]
    public int stepToTrigger = 1; 

    private XRBaseInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRBaseInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.firstSelectEntered.AddListener(OnGrabbed);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.firstSelectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (guideCanvas != null)
        {
            guideCanvas.AdvanceStep(stepToTrigger);
        }
    }
}