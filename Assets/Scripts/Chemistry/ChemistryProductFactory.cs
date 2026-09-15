using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Factory class responsible ONLY for instantiating existing reaction Prefabs 
/// directly from the project resources without altering or adding meshes.
/// </summary>
public static class ChemistryProductFactory
{
    // دالة لاستدعاء الـ Prefab الأصلي كما هو بدون أي تعديل برمجي
    private static GameObject LoadAndInstantiate(string[] possibleNames)
{
    foreach (var name in possibleNames)
    {
        var prefab = Resources.Load<GameObject>(name) 
                  ?? Resources.Load<GameObject>($"Prefabs/Chemistry/{name}")
                  ?? Resources.Load<GameObject>($"Chemistry/{name}");

        if (prefab != null)
        {
            return Object.Instantiate(prefab);
        }
    }

    Debug.LogWarning($"[ChemistryProductFactory] لم يتم العثور على Prefab بالأسماء: {string.Join(", ", possibleNames)}");
    return null; // إلغاء إنشاء GameObject فارغ يخرب الـ Prefab
}

    // --- استدعاء مجسمات التفاعلات المباشرة ---

    public static GameObject CreateWaterPrefabRoot() 
        => LoadAndInstantiate(new[] { "H2O_Water", "H2O_Water_Liquid" });

    public static GameObject CreateSaltPrefabRoot() 
        => LoadAndInstantiate(new[] { "NaCl_SaltCrystal", "NaCl_Salt" });

    public static GameObject CreateCarbonDioxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "CO2_GasSmoke", "CO2_Gas" });

    public static GameObject CreateHydrogenPeroxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "H2O2_Peroxide", "H2O2_Liquid" });

    public static GameObject CreateMagnesiumOxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "MgO_BurnFlash", "MgO_Burn" });

    public static GameObject CreateIronSulfidePrefabRoot() 
        => LoadAndInstantiate(new[] { "FeS_Solid" });

    public static GameObject CreateAmmoniaPrefabRoot() 
        => LoadAndInstantiate(new[] { "NH3_GasSmoke", "NH3_Gas" });

    public static GameObject CreateFailEffectRoot() 
        => LoadAndInstantiate(new[] { "ReactionFail", "FailEffect" });

public static GameObject CreateCalciumHydroxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "CaOH2_Slurry", "CaOH2" });
    public static GameObject CreateKohPrefabRoot() 
        => LoadAndInstantiate(new[] { "KOH_Alkali", "KOH" });

    

    public static GameObject CreateLithiumFluoridePrefabRoot() 
        => LoadAndInstantiate(new[] { "LiF_Crystal", "LiF" });

    public static GameObject CreateBerylliumChloridePrefabRoot() 
        => LoadAndInstantiate(new[] { "BeCl2_Needles", "BeCl2" });

    public static GameObject CreateBoronTrifluoridePrefabRoot() 
        => LoadAndInstantiate(new[] { "BF3_Gas", "BF3" });

    public static GameObject CreateAluminumChloridePrefabRoot() 
        => LoadAndInstantiate(new[] { "AlCl3_Powder", "AlCl3" });

    public static GameObject CreateSilicaPrefabRoot() 
        => LoadAndInstantiate(new[] { "SiO2_Sand", "SiO2" });

    public static GameObject CreatePhosphorusPentoxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "P4O10_WhiteSmoke", "P4O10" });

    public static GameObject CreateSulfurDioxidePrefabRoot() 
        => LoadAndInstantiate(new[] { "SO2_BlueFlame", "SO2_Gas", "SO2" });

    public static GameObject CreatePhosphorusTrichloridePrefabRoot() 
        => LoadAndInstantiate(new[] { "PCl3_Liquid", "PCl3" });

    public static GameObject CreateCalciumFluoridePrefabRoot() 
        => LoadAndInstantiate(new[] { "CaF2_Fluorite", "CaF2" });

    public static GameObject CreateAluminumSulfidePrefabRoot() 
        => LoadAndInstantiate(new[] { "Al2S3_Solid", "Al2S3" });

    public static GameObject CreateSodiumSulfidePrefabRoot() 
        => LoadAndInstantiate(new[] { "Na2S_Crystal", "Na2S" });

    public static GameObject CreateMagnesiumChloridePrefabRoot() 
        => LoadAndInstantiate(new[] { "MgCl2_Crystal", "MgCl2" });

    // --- استدعاء مكعبات العناصر ---

    public static GameObject CreateElementCube(ChemistryElementCatalog.ElementInfo info, Material material)
    {
        var cubePrefab = Resources.Load<GameObject>($"Element_{info.AtomicNumber:00}_{info.Symbol}")
                      ?? Resources.Load<GameObject>($"Prefabs/Chemistry/Element_{info.AtomicNumber:00}_{info.Symbol}");

        if (cubePrefab != null)
        {
            return Object.Instantiate(cubePrefab);
        }

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = $"Element_{info.AtomicNumber:00}_{info.Symbol}";
        cube.transform.localScale = Vector3.one * 0.42f;

        var script = cube.AddComponent<ChemistryElementCube>();
        script.elementSymbol = info.Symbol;
        script.EnsureCollider();

        return cube;
    }

    // --- الدوال المطلوبة من الكلاسات الأخرى لإنشاء المواد (Materials) ---

    public static Material MakeLit(Color color, bool transparent, float metallic, float smoothness)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        var mat = new Material(shader) { name = $"ChemMat_{color.r:0.00}_{color.g:0.00}_{color.b:0.00}" };

        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);

        if (transparent)
        {
            mat.SetFloat("_Surface", 1f);
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)RenderQueue.Transparent;
        }

        return mat;
    }

    public static Material MakeParticleMaterial(Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Particles/Standard Unlit") ?? Shader.Find("Sprites/Default");
        var mat = new Material(shader) { name = "ChemParticle" };
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        return mat;
    }
}

public class ChemistryFlashLight : MonoBehaviour
{
    public float duration = 0.6f;
    Light _light;
    float _start;
    float _t;

    void Awake()
    {
        _light = GetComponent<Light>();
        _start = _light != null ? _light.intensity : 0f;
    }

    void Update()
    {
        if (_light == null) return;
        _t += Time.deltaTime;
        float k = 1f - Mathf.Clamp01(_t / duration);
        _light.intensity = _start * k * k;
        if (_t >= duration) _light.enabled = false;
    }
}