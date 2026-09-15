using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LaserOpticsController : MonoBehaviour
{
    [Header("Laser Beam Groups")]
    [SerializeField] private GameObject straightBeams;
    [SerializeField] private GameObject convexBeams;
    [SerializeField] private GameObject concaveBeams;

    [Header("Optional Laser Audio")]
    [SerializeField] private AudioSource laserAudio;

    [Header("Final Explanation")]
    [SerializeField] private float explanationDelay = 1.5f;

    [Header("Experiment Events")]
    [SerializeField] private UnityEvent onLaserTurnedOn;
    [SerializeField] private UnityEvent onConvexLensDetected;
    [SerializeField] private UnityEvent onConcaveLensDetected;
    [SerializeField] private UnityEvent onLensRemoved;

    // جديد
    [SerializeField] private UnityEvent onExperimentCompleted;

    private bool laserIsOn = false;

    // جديد: نتابع هل المستخدم جرب العدستين
    private bool convexTested = false;
    private bool concaveTested = false;
    private bool experimentCompleted = false;

    private Coroutine completionCoroutine;

    public bool LaserIsOn => laserIsOn;

    private void Start()
    {
        SetAllBeams(false);
    }

    public void TurnOnLaser()
    {
        if (laserIsOn)
            return;

        laserIsOn = true;

        SetBeamState(
            straight: true,
            convex: false,
            concave: false
        );

        if (laserAudio != null)
        {
            if (!laserAudio.isPlaying)
                laserAudio.Play();
        }

        onLaserTurnedOn?.Invoke();

        Debug.Log("LASER ON");
    }

    public void TurnOffLaser()
    {
        laserIsOn = false;

        SetAllBeams(false);

        if (laserAudio != null)
            laserAudio.Stop();

        Debug.Log("LASER OFF");
    }

    public void ShowConvexEffect()
    {
        if (!laserIsOn)
        {
            Debug.LogWarning("Convex lens detected, but laser is OFF.");
            return;
        }

        SetBeamState(
            straight: false,
            convex: true,
            concave: false
        );

        // نسجل أن المستخدم جرّب المحدبة
        convexTested = true;

        onConvexLensDetected?.Invoke();

        Debug.Log("CONVEX LENS - RAYS CONVERGING");
    }

    public void ShowConcaveEffect()
    {
        if (!laserIsOn)
        {
            Debug.LogWarning("Concave lens detected, but laser is OFF.");
            return;
        }

        SetBeamState(
            straight: false,
            convex: false,
            concave: true
        );

        // نسجل أن المستخدم جرّب المقعرة
        concaveTested = true;

        onConcaveLensDetected?.Invoke();

        Debug.Log("CONCAVE LENS - RAYS DIVERGING");
    }

    public void ShowStraightBeams()
    {
        if (!laserIsOn)
            return;

        SetBeamState(
            straight: true,
            convex: false,
            concave: false
        );

        // إذا المستخدم جرّب العدستين
        if (convexTested && concaveTested && !experimentCompleted)
        {
            StartCompletionCountdown();
        }
        else
        {
            // إذا ما خلص التجربتين، يرجع للتعليمات العادية
            onLensRemoved?.Invoke();
        }

        Debug.Log("LENS REMOVED - STRAIGHT BEAMS");
    }

    private void StartCompletionCountdown()
    {
        if (completionCoroutine != null)
            return;

        completionCoroutine = StartCoroutine(CompleteExperimentAfterDelay());
    }

    private IEnumerator CompleteExperimentAfterDelay()
    {
        yield return new WaitForSeconds(explanationDelay);

        experimentCompleted = true;

        onExperimentCompleted?.Invoke();

        completionCoroutine = null;

        Debug.Log("OPTICS EXPERIMENT COMPLETED");
    }

    public void ResetExperiment()
    {
        StopAllCoroutines();

        completionCoroutine = null;

        convexTested = false;
        concaveTested = false;
        experimentCompleted = false;

        TurnOffLaser();

        Debug.Log("OPTICS EXPERIMENT RESET");
    }

    private void SetBeamState(
        bool straight,
        bool convex,
        bool concave)
    {
        if (straightBeams != null)
            straightBeams.SetActive(straight);

        if (convexBeams != null)
            convexBeams.SetActive(convex);

        if (concaveBeams != null)
            concaveBeams.SetActive(concave);
    }

    private void SetAllBeams(bool state)
    {
        if (straightBeams != null)
            straightBeams.SetActive(state);

        if (convexBeams != null)
            convexBeams.SetActive(state);

        if (concaveBeams != null)
            concaveBeams.SetActive(state);
    }
}