using UnityEngine;

/// <summary>
/// Generates a Sci-Fi / High-Tech Chemistry Reactor base procedurally.
/// Compatible with URP & Built-in Render Pipelines.
/// </summary>
public class SciFiChemistryReactor : MonoBehaviour
{
    [Header("Color Theme")]
    public Color glowColor = new Color(0f, 0.85f, 1f, 0.9f); // أزرق متوهج
    public Color metallicColor = new Color(0.12f, 0.14f, 0.18f);

    private GameObject _innerRing;
    private GameObject _outerRing;

    void Start()
    {
        BuildTechBase();
    }

    void Update()
    {
        if (_innerRing != null)
            _innerRing.transform.Rotate(Vector3.up * 45f * Time.deltaTime);

        if (_outerRing != null)
            _outerRing.transform.Rotate(Vector3.up * -25f * Time.deltaTime);
    }

    void BuildTechBase()
    {
        // 1. قاعدة المفاعل المعدنية
        var baseCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        baseCylinder.name = "TechPedestal";
        baseCylinder.transform.SetParent(transform, false);
        baseCylinder.transform.localPosition = Vector3.zero;
        baseCylinder.transform.localScale = new Vector3(1.2f, 0.04f, 1.2f);
        
        Material metalMat = CreateSafeMaterial(metallicColor, false);
        metalMat.SetFloat("_Metallic", 0.85f);
        metalMat.SetFloat("_Smoothness", 0.75f);
        baseCylinder.GetComponent<Renderer>().sharedMaterial = metalMat;

        // 2. الحلقة الداخلية
        _innerRing = new GameObject("InnerTechRing");
        _innerRing.transform.SetParent(transform, false);
        _innerRing.transform.localPosition = new Vector3(0f, 0.045f, 0f);

        CreateSegmentedRing(_innerRing.transform, 0.45f, 8, 0.02f);

        // 3. الحلقة الخارجية
        _outerRing = new GameObject("OuterTechRing");
        _outerRing.transform.SetParent(transform, false);
        _outerRing.transform.localPosition = new Vector3(0f, 0.045f, 0f);

        CreateSegmentedRing(_outerRing.transform, 0.55f, 16, 0.015f);

        // 4. إضاءة مركزية
        var lightGo = new GameObject("ReactorPointLight");
        lightGo.transform.SetParent(transform, false);
        lightGo.transform.localPosition = new Vector3(0f, 0.2f, 0f);
        var pLight = lightGo.AddComponent<Light>();
        pLight.type = LightType.Point;
        pLight.color = glowColor;
        pLight.intensity = 2.5f;
        pLight.range = 2.5f;
    }

    void CreateSegmentedRing(Transform parent, float radius, int segments, float nodeScale)
    {
        Material glowMat = CreateSafeMaterial(glowColor, true);

        for (int i = 0; i < segments; i++)
        {
            float angle = i * (360f / segments) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            var node = GameObject.CreatePrimitive(PrimitiveType.Cube);
            node.name = $"TechNode_{i}";
            node.transform.SetParent(parent, false);
            node.transform.localPosition = pos;
            node.transform.localScale = new Vector3(nodeScale, 0.01f, nodeScale * 2.5f);
            node.transform.localRotation = Quaternion.LookRotation(pos);

            node.GetComponent<Renderer>().sharedMaterial = glowMat;
            Destroy(node.GetComponent<Collider>());
        }
    }

    // دالة لتوليد خام متوافق مع URP تلقائياً ومنع اللون الوردي
    Material CreateSafeMaterial(Color color, bool isEmission)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");

        Material mat = new Material(shader);
        mat.color = color;
        mat.SetColor("_BaseColor", color);

        if (isEmission)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color * 2.5f);
        }

        return mat;
    }
}