using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LensSocketController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserOpticsController opticsController;

    [Header("Lenses")]
    [SerializeField] private GameObject convexLens;
    [SerializeField] private GameObject concaveLens;

    public void LensInserted(SelectEnterEventArgs args)
    {
        GameObject insertedObject = args.interactableObject.transform.gameObject;

        if (insertedObject == convexLens)
        {
            Debug.Log("CONVEX LENS INSERTED");

            if (opticsController != null)
                opticsController.ShowConvexEffect();
        }
        else if (insertedObject == concaveLens)
        {
            Debug.Log("CONCAVE LENS INSERTED");

            if (opticsController != null)
                opticsController.ShowConcaveEffect();
        }
    }

    public void LensRemoved(SelectExitEventArgs args)
    {
        Debug.Log("LENS REMOVED");

        if (opticsController != null)
            opticsController.ShowStraightBeams();
    }
}