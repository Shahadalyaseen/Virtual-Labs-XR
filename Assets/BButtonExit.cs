using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class BButtonExit : MonoBehaviour
{
    [Header("Scene Manager Reference")]
    public SceneTransitionManager transitionManager;

    private void Update()
    {
        // نتحقق من أجهزة الإدخال لليد اليمنى
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        foreach (var device in devices)
        {
            // فحص ما إذا تم ضغط الزر الثانوي (زر B) في هذا الفريم
            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed) && isPressed)
            {
                if (transitionManager != null)
                {
                    transitionManager.ExitToMainMenu();
                }
                else
                {
                    Debug.LogWarning("لم يتم ربط SceneTransitionManager!");
                }
                break;
            }
        }
    }
}