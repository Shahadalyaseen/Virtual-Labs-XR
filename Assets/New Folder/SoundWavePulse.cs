using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWavePulse : MonoBehaviour
{
    private Vector3 originalScale;
    private Material[] runtimeMaterials;

    private void Awake()
    {
        originalScale = transform.localScale;

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        List<Material> materials = new List<Material>();

        foreach (Renderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        runtimeMaterials = materials.ToArray();
    }

    public void Play(
        float duration,
        float startScaleMultiplier,
        float endScaleMultiplier,
        float startingAlpha)
    {
        StartCoroutine(
            AnimateWave(
                duration,
                startScaleMultiplier,
                endScaleMultiplier,
                startingAlpha
            )
        );
    }

    private IEnumerator AnimateWave(
        float duration,
        float startScaleMultiplier,
        float endScaleMultiplier,
        float startingAlpha)
    {
        float elapsed = 0f;

        Vector3 startScale = originalScale * startScaleMultiplier;
        Vector3 endScale = originalScale * endScaleMultiplier;

        transform.localScale = startScale;

        SetAlpha(startingAlpha);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            // يجعل الحركة أنعم
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale =
                Vector3.Lerp(startScale, endScale, smoothT);

            float currentAlpha =
                Mathf.Lerp(startingAlpha, 0f, t);

            SetAlpha(currentAlpha);

            yield return null;
        }

        SetAlpha(0f);

        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        if (runtimeMaterials == null)
        {
            return;
        }

        foreach (Material material in runtimeMaterials)
        {
            if (material == null)
            {
                continue;
            }

            // URP
            if (material.HasProperty("_BaseColor"))
            {
                Color color = material.GetColor("_BaseColor");
                color.a = alpha;
                material.SetColor("_BaseColor", color);
            }
            // Built-in shaders
            else if (material.HasProperty("_Color"))
            {
                Color color = material.GetColor("_Color");
                color.a = alpha;
                material.SetColor("_Color", color);
            }
        }
    }
}