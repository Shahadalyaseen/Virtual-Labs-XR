using UnityEngine;
using UnityEngine.InputSystem;

public class VRMenuController : MonoBehaviour
{
    [Header("Menu & Camera References")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private Transform headCamera;

    [Header("Positioning Settings")]
    [SerializeField] private float distanceFromHead = 1.5f;
    [SerializeField] private float heightOffset = 0f;

    [Header("Input Action")]
    [SerializeField] private InputActionProperty menuButtonAction;

    private void OnEnable()
    {
        if (menuButtonAction.action != null)
        {
            menuButtonAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (menuButtonAction.action != null)
        {
            menuButtonAction.action.Disable();
        }
    }

    private void Update()
    {
        if (menuButtonAction.action != null &&
            menuButtonAction.action.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (menuCanvas == null || headCamera == null)
        {
            Debug.LogWarning("Menu Canvas or Head Camera is not assigned.");
            return;
        }

        bool shouldOpen = !menuCanvas.activeSelf;

        if (shouldOpen)
        {
            PositionMenuInFront();
            menuCanvas.SetActive(true);

            Debug.Log("VR MENU OPENED");
        }
        else
        {
            menuCanvas.SetActive(false);

            Debug.Log("VR MENU CLOSED");
        }
    }

    private void PositionMenuInFront()
    {
        Vector3 forwardDirection = headCamera.forward;

        // نخلي المنيو مستقيم وما يتبع ميلان الرأس لفوق وتحت
        forwardDirection.y = 0f;

        if (forwardDirection.sqrMagnitude < 0.001f)
        {
            forwardDirection = headCamera.transform.forward;
            forwardDirection.y = 0f;
        }

        forwardDirection.Normalize();

        Vector3 targetPosition =
            headCamera.position +
            (forwardDirection * distanceFromHead);

        targetPosition.y =
            headCamera.position.y + heightOffset;

        menuCanvas.transform.position = targetPosition;

        // نفس اتجاه نظر المستخدم أفقيًا
        menuCanvas.transform.rotation =
            Quaternion.Euler(
                0f,
                headCamera.eulerAngles.y,
                0f
            );
    }
}