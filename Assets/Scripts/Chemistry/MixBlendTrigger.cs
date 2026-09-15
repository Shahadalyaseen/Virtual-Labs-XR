using UnityEngine;

/// <summary>
/// 3D Mix / Blend trigger. The lab manager raycasts this collider and runs Mix().
/// </summary>
[RequireComponent(typeof(Collider))]
public class MixBlendTrigger : MonoBehaviour
{
    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }
}
