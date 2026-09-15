using System.Collections;
using UnityEngine;

public class DelayedActivate : MonoBehaviour
{
    public GameObject flameObject;
    public float delayTime = 0.2f; // مدة التأخير بالثواني

    public void TriggerFlameWithDelay()
    {
        StartCoroutine(EnableRoutine());
    }

    private IEnumerator EnableRoutine()
    {
        yield return new WaitForSeconds(delayTime);
        if (flameObject != null) flameObject.SetActive(true);
    }
}