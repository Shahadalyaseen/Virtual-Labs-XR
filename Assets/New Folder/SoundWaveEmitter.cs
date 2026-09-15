using System.Collections;
using UnityEngine;

public class SoundWaveEmitter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject wavePrefab;
    [SerializeField] private Transform waveOrigin;
    [SerializeField] private Transform waveParent;

    [Header("Wave Sequence")]
    [SerializeField] private int waveCount = 6;
    [SerializeField] private float delayBetweenWaves = 0.15f;

    [Header("Wave Appearance")]
    [SerializeField] private float waveDuration = 1.3f;
    [SerializeField] private float startScale = 0.08f;
    [SerializeField] private float endScale = 3f;
    [SerializeField] private float maximumAlpha = 0.5f;

    public void Emit(float hitIntensity)
    {
        StartCoroutine(EmitRoutine(hitIntensity));
    }

    private IEnumerator EmitRoutine(float hitIntensity)
    {
        if (wavePrefab == null)
        {
            Debug.LogWarning("SoundWaveEmitter: Wave Prefab is missing.");
            yield break;
        }

        if (waveOrigin == null)
        {
            Debug.LogWarning("SoundWaveEmitter: Wave Origin is missing.");
            yield break;
        }

        for (int i = 0; i < waveCount; i++)
        {
            GameObject newWave;

            if (waveParent != null)
            {
                newWave = Instantiate(
                    wavePrefab,
                    waveOrigin.position,
                    waveOrigin.rotation,
                    waveParent
                );
            }
            else
            {
                newWave = Instantiate(
                    wavePrefab,
                    waveOrigin.position,
                    waveOrigin.rotation
                );
            }

            SoundWavePulse pulse =
                newWave.GetComponent<SoundWavePulse>();

            if (pulse != null)
            {
                float alpha =
                    Mathf.Lerp(
                        maximumAlpha * 0.65f,
                        maximumAlpha,
                        hitIntensity
                    );

                pulse.Play(
                    waveDuration,
                    startScale,
                    endScale,
                    alpha
                );
            }
            else
            {
                Debug.LogWarning(
                    "Wave Prefab does not contain SoundWavePulse."
                );

                Destroy(newWave);
            }

            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }
}