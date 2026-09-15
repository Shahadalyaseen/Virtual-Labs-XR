using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class VRSceneInitializer : MonoBehaviour
{
    [Header("References")]
    public InputActionManager inputActionManager;
    public LocomotionMediator locomotionMediator;
    public XRBodyTransformer bodyTransformer;

    private void Awake()
    {
        if (inputActionManager != null)
            inputActionManager.enabled = true;

        if (locomotionMediator != null)
            locomotionMediator.enabled = true;

        if (bodyTransformer != null)
            bodyTransformer.enabled = true;
    }

    private void Start()
    {
        Debug.Log("VR Scene initialized successfully.");
    }
}