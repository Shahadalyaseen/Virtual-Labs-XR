using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChemistryElementCube : MonoBehaviour
{
    [Tooltip("رمز العنصر في الجدول الدوري، مثل H أو O")]
    public string elementSymbol = "H";

    [Tooltip("النص الذي سيظهر بالنظارة فوق المكعب")]
    public TextMesh countLabel;

    private Vector3 _baseScale;
    private bool _isScaleCached = false;
    private Coroutine _pressCoroutine;
    private Coroutine _labelAnimCoroutine;
    private Camera _mainCamera;

    void Awake()
    {
        CacheScale();

        if (string.IsNullOrWhiteSpace(elementSymbol))
            elementSymbol = ChemistryElementCatalog.ParseSymbol(name);

        _mainCamera = Camera.main;
        EnsureCollider();
        EnsureCountLabel();
        RefreshCount(0);
    }

    void Update()
    {
        // جعل النص يواجه كاميرا النظارة دائمًا بكل اتجاه تتحركين فيه
        if (countLabel != null && countLabel.gameObject.activeSelf)
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            
            if (_mainCamera != null)
            {
                // توجيه النص نحو الكاميرا مباشرة
                countLabel.transform.rotation = Quaternion.LookRotation(countLabel.transform.position - _mainCamera.transform.position);
            }
        }
    }

    void CacheScale()
    {
        if (!_isScaleCached)
        {
            _baseScale = transform.localScale;
            _isScaleCached = true;
        }
    }

    public void EnsureCollider()
    {
        if (GetComponent<Collider>() != null)
            return;

        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            var meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshCollider.convex = false;
            return;
        }

        gameObject.AddComponent<BoxCollider>();
    }

    public void EnsureCountLabel()
    {
        if (countLabel == null)
        {
            var labelGo = new GameObject("VR_CountLabel");
            labelGo.transform.SetParent(transform, false);
            
            // موقع النص فوق المكعب
            labelGo.transform.localPosition = new Vector3(0f, 0.7f, 0f); 

            countLabel = labelGo.AddComponent<TextMesh>();
            countLabel.characterSize = 0.12f;
            countLabel.fontSize = 45;
            countLabel.anchor = TextAnchor.MiddleCenter;
            countLabel.alignment = TextAlignment.Center;
            countLabel.fontStyle = FontStyle.Bold;
        }
    }

    public void RefreshCount(int count)
    {
        if (countLabel == null) return;

        if (count <= 0)
        {
            countLabel.text = "";
            countLabel.gameObject.SetActive(false);
        }
        else
        {
            countLabel.gameObject.SetActive(true);
            
            // يكتب مثلاً: H x1 أو H x2
            countLabel.text = $"{elementSymbol}  x{count}"; 
            
            // لون أصفر واضح جداً داخل النظارة
            countLabel.color = new Color(1f, 0.9f, 0.1f); 

            if (_labelAnimCoroutine != null)
                StopCoroutine(_labelAnimCoroutine);
            _labelAnimCoroutine = StartCoroutine(AnimateLabelPulse());
        }
    }

    public void PulseHighlight()
    {
        CacheScale();

        if (_pressCoroutine != null)
            StopCoroutine(_pressCoroutine);

        _pressCoroutine = StartCoroutine(AnimateButtonPress());
    }

    private IEnumerator AnimateButtonPress()
    {
        Vector3 targetScale = _baseScale * 0.75f;

        float elapsed = 0f;
        float duration = 0.07f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(_baseScale, targetScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        duration = 0.09f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, _baseScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = _baseScale;
    }

    private IEnumerator AnimateLabelPulse()
    {
        if (countLabel == null) yield break;

        Vector3 originalScale = Vector3.one;
        Vector3 bigScale = Vector3.one * 1.35f;

        float elapsed = 0f;
        float duration = 0.08f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            countLabel.transform.localScale = Vector3.Lerp(originalScale, bigScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            countLabel.transform.localScale = Vector3.Lerp(bigScale, originalScale, elapsed / duration);
            yield return null;
        }

        countLabel.transform.localScale = originalScale;
    }
}