using UnityEngine;

public class LensDetectionZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserOpticsController opticsController;

    [Header("Lenses")]
    [SerializeField] private GameObject convexLens;
    [SerializeField] private GameObject concaveLens;

    private GameObject currentLens;

    private void OnTriggerEnter(Collider other)
    {
        GameObject detectedLens = GetLensObject(other);

        if (detectedLens == null)
            return;

        if (detectedLens == convexLens)
        {
            currentLens = convexLens;

            Debug.Log("CONVEX LENS DETECTED");

            if (opticsController != null)
                opticsController.ShowConvexEffect();

            return;
        }

        if (detectedLens == concaveLens)
        {
            currentLens = concaveLens;

            Debug.Log("CONCAVE LENS DETECTED");

            if (opticsController != null)
                opticsController.ShowConcaveEffect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject detectedLens = GetLensObject(other);

        if (detectedLens == null)
            return;

        if (detectedLens == currentLens)
        {
            currentLens = null;

            Debug.Log("LENS REMOVED");

            if (opticsController != null)
                opticsController.ShowStraightBeams();
        }
    }

    private GameObject GetLensObject(Collider other)
    {
        if (other.attachedRigidbody != null)
            return other.attachedRigidbody.gameObject;

        return other.gameObject;
    }
}