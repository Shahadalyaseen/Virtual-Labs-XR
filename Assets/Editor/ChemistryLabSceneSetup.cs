using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class ChemistryLabSceneSetup
{
    const string PrefabFolder = "Assets/Prefabs/Chemistry";
    const string MaterialsFolder = "Assets/Materials/Chemistry";
    const string ElementMatFolder = "Assets/Materials/Chemistry/Elements";
    const string ProductMatFolder = "Assets/Materials/Chemistry/Products";

    [InitializeOnLoadMethod]
    static void AutoGeneratePrefabs()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                return;
            if (SessionState.GetBool("ChemistryLab.Prefabs20v3", false))
                return;
            GenerateAllPrefabsAndMaterials();
            SessionState.SetBool("ChemistryLab.Prefabs20v3", true);
        };
    }

    [MenuItem("Chemistry/Generate 20 Elements + 20 Reaction Prefabs")]
    public static void GenerateAllPrefabsAndMaterials()
    {
        EnsureFolders();

        var urpLit = Shader.Find("Universal Render Pipeline/Lit");
        var particleShader = Shader.Find("Universal Render Pipeline/Particles/Unlit")
                             ?? Shader.Find("Sprites/Default");
        if (urpLit == null)
        {
            Debug.LogError("URP Lit shader not found. Cannot create chemistry materials.");
            return;
        }

        var elementPrefabs = new List<GameObject>();
        foreach (var info in ChemistryElementCatalog.FirstTwenty)
        {
            var mat = SaveLitMaterial($"{ElementMatFolder}/Element_{info.Symbol}.mat", info.Color, false, 0.08f, 0.42f, urpLit);
            var cube = ChemistryProductFactory.CreateElementCube(info, mat);
            var prefab = SavePrefab(cube, $"Element_{info.AtomicNumber:00}_{info.Symbol}.prefab");
            elementPrefabs.Add(prefab);
        }

        var feMat = SaveLitMaterial($"{ElementMatFolder}/Element_Fe.mat", new Color(0.88f, 0.4f, 0.2f), false, 0.35f, 0.45f, urpLit);
        var feInfo = new ChemistryElementCatalog.ElementInfo(26, "Fe", "Iron", new Color(0.88f, 0.4f, 0.2f));
        SavePrefab(ChemistryProductFactory.CreateElementCube(feInfo, feMat), "Element_26_Fe.prefab");

        var products = new Dictionary<string, GameObject>
        {
            ["water"] = BakeProduct(ChemistryProductFactory.CreateWaterPrefabRoot(), "H2O_Water.prefab", urpLit, particleShader),
            ["salt"] = BakeProduct(ChemistryProductFactory.CreateSaltPrefabRoot(), "NaCl_SaltCrystal.prefab", urpLit, particleShader),
            ["co2"] = BakeProduct(ChemistryProductFactory.CreateCarbonDioxidePrefabRoot(), "CO2_GasSmoke.prefab", urpLit, particleShader),
            ["mgo"] = BakeProduct(ChemistryProductFactory.CreateMagnesiumOxidePrefabRoot(), "MgO_BurnFlash.prefab", urpLit, particleShader),
            ["fes"] = BakeProduct(ChemistryProductFactory.CreateIronSulfidePrefabRoot(), "FeS_Solid.prefab", urpLit, particleShader),
            ["nh3"] = BakeProduct(ChemistryProductFactory.CreateAmmoniaPrefabRoot(), "NH3_GasSmoke.prefab", urpLit, particleShader),
            ["koh"] = BakeProduct(ChemistryProductFactory.CreateKohPrefabRoot(), "KOH_Alkali.prefab", urpLit, particleShader),
            ["caoh2"] = BakeProduct(ChemistryProductFactory.CreateCalciumHydroxidePrefabRoot(), "CaOH2_Slurry.prefab", urpLit, particleShader),
            ["lif"] = BakeProduct(ChemistryProductFactory.CreateLithiumFluoridePrefabRoot(), "LiF_Crystal.prefab", urpLit, particleShader),
            ["becl2"] = BakeProduct(ChemistryProductFactory.CreateBerylliumChloridePrefabRoot(), "BeCl2_Needles.prefab", urpLit, particleShader),
            ["bf3"] = BakeProduct(ChemistryProductFactory.CreateBoronTrifluoridePrefabRoot(), "BF3_Gas.prefab", urpLit, particleShader),
            ["alcl3"] = BakeProduct(ChemistryProductFactory.CreateAluminumChloridePrefabRoot(), "AlCl3_Powder.prefab", urpLit, particleShader),
            ["sio2"] = BakeProduct(ChemistryProductFactory.CreateSilicaPrefabRoot(), "SiO2_Sand.prefab", urpLit, particleShader),
            ["p4o10"] = BakeProduct(ChemistryProductFactory.CreatePhosphorusPentoxidePrefabRoot(), "P4O10_WhiteSmoke.prefab", urpLit, particleShader),
            ["so2"] = BakeProduct(ChemistryProductFactory.CreateSulfurDioxidePrefabRoot(), "SO2_BlueFlame.prefab", urpLit, particleShader),
            ["pcl3"] = BakeProduct(ChemistryProductFactory.CreatePhosphorusTrichloridePrefabRoot(), "PCl3_Liquid.prefab", urpLit, particleShader),
            ["caf2"] = BakeProduct(ChemistryProductFactory.CreateCalciumFluoridePrefabRoot(), "CaF2_Fluorite.prefab", urpLit, particleShader),
            ["al2s3"] = BakeProduct(ChemistryProductFactory.CreateAluminumSulfidePrefabRoot(), "Al2S3_Solid.prefab", urpLit, particleShader),
            ["na2s"] = BakeProduct(ChemistryProductFactory.CreateSodiumSulfidePrefabRoot(), "Na2S_Crystal.prefab", urpLit, particleShader),
            ["mgcl2"] = BakeProduct(ChemistryProductFactory.CreateMagnesiumChloridePrefabRoot(), "MgCl2_Crystal.prefab", urpLit, particleShader),
            ["fail"] = BakeProduct(ChemistryProductFactory.CreateFailEffectRoot(), "ReactionFail.prefab", urpLit, particleShader)
        };

        WireManager(elementPrefabs.ToArray(), products);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Generated 20 element prefabs + Fe, 20 reaction prefabs, and URP Lit materials (no pink placeholders).");
    }

    static void WireManager(GameObject[] elementPrefabs, Dictionary<string, GameObject> products)
    {
        var manager = Object.FindFirstObjectByType<Chemistry3DLabManager>();
        var scene = SceneManager.GetActiveScene();
        if (manager == null)
        {
            var lab = GameObject.Find("Chemistry3DLab");
            if (lab == null)
                lab = new GameObject("Chemistry3DLab");
            manager = lab.AddComponent<Chemistry3DLabManager>();
        }

        Undo.RecordObject(manager, "Wire chemistry prefabs");
        manager.elementPrefabs = elementPrefabs;
        manager.waterPrefab = products["water"];
        manager.saltPrefab = products["salt"];
        manager.carbonDioxidePrefab = products["co2"];
        manager.magnesiumOxidePrefab = products["mgo"];
        manager.ironSulfidePrefab = products["fes"];
        manager.ammoniaPrefab = products["nh3"];
        manager.kohPrefab = products["koh"];
        manager.calciumHydroxidePrefab = products["caoh2"];
        manager.lithiumFluoridePrefab = products["lif"];
        manager.berylliumChloridePrefab = products["becl2"];
        manager.boronTrifluoridePrefab = products["bf3"];
        manager.aluminumChloridePrefab = products["alcl3"];
        manager.silicaPrefab = products["sio2"];
        manager.phosphorusPentoxidePrefab = products["p4o10"];
        manager.sulfurDioxidePrefab = products["so2"];
        manager.phosphorusTrichloridePrefab = products["pcl3"];
        manager.calciumFluoridePrefab = products["caf2"];
        manager.aluminumSulfidePrefab = products["al2s3"];
        manager.sodiumSulfidePrefab = products["na2s"];
        manager.magnesiumChloridePrefab = products["mgcl2"];
        manager.failEffectPrefab = products["fail"];
        EditorUtility.SetDirty(manager);
        if (scene.IsValid())
            EditorSceneManager.MarkSceneDirty(scene);
    }

    static GameObject BakeProduct(GameObject instance, string fileName, Shader lit, Shader particle)
    {
        SaveEmbeddedMaterials(instance, Path.GetFileNameWithoutExtension(fileName), lit, particle);
        return SavePrefab(instance, fileName);
    }

    static void SaveEmbeddedMaterials(GameObject root, string prefix, Shader lit, Shader particle)
    {
        int i = 0;
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            var current = renderer.sharedMaterial;
            Color color = Color.white;
            bool transparent = false;
            if (current != null)
            {
                if (current.HasProperty("_BaseColor"))
                    color = current.GetColor("_BaseColor");
                else if (current.HasProperty("_Color"))
                    color = current.GetColor("_Color");
                transparent = current.GetTag("RenderType", false) == "Transparent" || current.renderQueue >= (int)RenderQueue.Transparent;
            }

            bool isParticle = renderer is ParticleSystemRenderer;
            var shader = isParticle && particle != null ? particle : lit;
            var mat = new Material(shader) { name = $"{prefix}_{renderer.gameObject.name}_{i}" };
            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", color);
            if (!isParticle)
                ConfigureLit(mat, color, transparent, 0.08f, 0.45f);

            string path = $"{ProductMatFolder}/{mat.name}.mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(mat, path);
            renderer.sharedMaterial = mat;
            i++;
        }
    }

    static Material SaveLitMaterial(string path, Color color, bool transparent, float metallic, float smoothness, Shader shader)
    {
        var mat = new Material(shader) { name = Path.GetFileNameWithoutExtension(path) };
        ConfigureLit(mat, color, transparent, metallic, smoothness);
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
        {
            EditorUtility.CopySerialized(mat, existing);
            Object.DestroyImmediate(mat);
            EditorUtility.SetDirty(existing);
            return existing;
        }

        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static void ConfigureLit(Material mat, Color color, bool transparent, float metallic, float smoothness)
    {
        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);
        if (mat.HasProperty("_Metallic"))
            mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness"))
            mat.SetFloat("_Smoothness", smoothness);
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
        else
        {
            mat.SetFloat("_Surface", 0f);
            mat.SetOverrideTag("RenderType", "Opaque");
            mat.renderQueue = (int)RenderQueue.Geometry;
        }
    }

    static GameObject SavePrefab(GameObject instance, string fileName)
    {
        string path = Path.Combine(PrefabFolder, fileName).Replace('\\', '/');
        var prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);
        Object.DestroyImmediate(instance);
        return prefab;
    }

    static void EnsureFolders()
    {
        CreateFolder("Assets/Prefabs");
        CreateFolder("Assets/Prefabs/Chemistry");
        CreateFolder("Assets/Materials");
        CreateFolder("Assets/Materials/Chemistry");
        CreateFolder("Assets/Materials/Chemistry/Elements");
        CreateFolder("Assets/Materials/Chemistry/Products");
    }

    static void CreateFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;
        var parent = Path.GetDirectoryName(path).Replace('\\', '/');
        var name = Path.GetFileName(path);
        AssetDatabase.CreateFolder(parent, name);
    }
}
