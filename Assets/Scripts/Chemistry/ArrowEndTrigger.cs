using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ArrowEndTrigger : MonoBehaviour
{
    public UnityEvent onPlayerReachedEnd;

    private bool _hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        // التأكد أن الذي وصل هو اللاعب/النظارة
        if (!_hasTriggered && (other.CompareTag("Player") || other.CompareTag("MainCamera")))
        {
            _hasTriggered = true;
            onPlayerReachedEnd?.Invoke();
        }
    }

    void OnEnable()
    {
        _hasTriggered = false; // إعادة التعيين عند تفعيل المرحلة
    }
}