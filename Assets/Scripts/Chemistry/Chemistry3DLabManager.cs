using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chemistry3DLabManager : MonoBehaviour
{
    public static Chemistry3DLabManager Instance { get; private set; }

    [Header("Spawn Points")]
    public Transform productSpawnPoint;

    [Header("UI Text")]
    public Text resultText;

    [Header("Mode Control")]
    public ElementModeManager elementModeManager;

    [Header("Audio")]
    public AudioClip customSuccessClip;
    public AudioClip customFailClip;

    [Header("Element Prefabs")]
    public GameObject[] elementPrefabs;

    [Header("Product Prefabs")]
    public GameObject waterPrefab;
    public GameObject saltPrefab;
    public GameObject carbonDioxidePrefab;
    public GameObject magnesiumOxidePrefab;
    public GameObject ironSulfidePrefab;
    public GameObject ammoniaPrefab;
    public GameObject kohPrefab;
    public GameObject calciumHydroxidePrefab;
    public GameObject lithiumFluoridePrefab;
    public GameObject berylliumChloridePrefab;
    public GameObject boronTrifluoridePrefab;
    public GameObject aluminumChloridePrefab;
    public GameObject silicaPrefab;
    public GameObject phosphorusPentoxidePrefab;
    public GameObject sulfurDioxidePrefab;
    public GameObject phosphorusTrichloridePrefab;
    public GameObject calciumFluoridePrefab;
    public GameObject aluminumSulfidePrefab;
    public GameObject sodiumSulfidePrefab;
    public GameObject magnesiumChloridePrefab;
    public GameObject failEffectPrefab;

    private readonly Dictionary<string, int> _counts =
        new Dictionary<string, int>();

    private readonly List<Recipe> _recipes =
        new List<Recipe>();

    private GameObject _lastProduct;
    private AudioSource _audio;
    private AudioClip _generatedSuccessClip;
    private AudioClip _generatedFailClip;
    private float _clickLock;

    private struct Recipe
    {
        public string Id;
        public string DisplayName;
        public string Formula;
        public Dictionary<string, int> Ratio;
        public System.Func<GameObject> PrefabGetter;
    }

    private void Awake()
    {
        Instance = this;

        EnsureAudio();
        BuildRecipes();

        SetResult(
            "Press Reactions, select elements, then press Mix.",
            new Color(0.85f, 0.9f, 1f)
        );
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        if (_clickLock > 0f)
            _clickLock -= Time.deltaTime;
    }

    // يستقبل ضغطة Trigger على العنصر أو على مجسم الخلط.
    public void OnObjectSelected(GameObject selectedObject)
    {
        if (_clickLock > 0f || selectedObject == null)
            return;

        // يمنع إضافة عناصر للـMix في وضع Information.
        if (elementModeManager != null &&
            !elementModeManager.reactionMode)
        {
            return;
        }

        MixBlendTrigger mix =
            selectedObject.GetComponentInParent<MixBlendTrigger>();

        if (mix != null)
        {
            Mix();
            return;
        }

        ChemistryElementCube cube =
            selectedObject.GetComponentInParent<ChemistryElementCube>();

        if (cube != null)
        {
            AddElement(cube);
        }
    }

    public void AddElement(ChemistryElementCube cube)
    {
        if (cube == null || string.IsNullOrEmpty(cube.elementSymbol))
            return;

        if (elementModeManager != null &&
            !elementModeManager.reactionMode)
        {
            return;
        }

        string symbol = cube.elementSymbol;

        _counts.TryGetValue(symbol, out int current);
        current++;

        _counts[symbol] = current;

        cube.PulseHighlight();
        cube.RefreshCount(current);

        SetResult(
            $"Added {symbol}  →  {symbol} x{current}",
            new Color(0.7f, 1f, 0.75f)
        );

        PlayTone(
            customSuccessClip != null
                ? customSuccessClip
                : _generatedSuccessClip,
            0.25f
        );

        _clickLock = 0.08f;
    }

    public void Mix()
    {
        // يمنع زر Mix من العمل خارج وضع Reactions.
        if (elementModeManager != null &&
            !elementModeManager.reactionMode)
        {
            SetResult(
                "Open Reactions first.",
                new Color(1f, 0.75f, 0.2f)
            );

            return;
        }

        _clickLock = 0.2f;

        if (_counts.Count == 0)
        {
            Fail("No elements selected.");
            return;
        }

        foreach (Recipe recipe in _recipes)
        {
            if (MatchesExact(recipe.Ratio))
            {
                Succeed(recipe);
                ClearCounters();
                return;
            }
        }

        Fail("No matching reaction. Check the exact ratios.");
        ClearCounters();
    }

    public void ClearCounters()
    {
        _counts.Clear();

        ChemistryElementCube[] cubes =
            FindObjectsOfType<ChemistryElementCube>(true);

        foreach (ChemistryElementCube cube in cubes)
        {
            if (cube != null)
                cube.RefreshCount(0);
        }
    }

    private bool MatchesExact(Dictionary<string, int> ratio)
    {
        if (_counts.Count != ratio.Count)
            return false;

        foreach (KeyValuePair<string, int> pair in ratio)
        {
            if (!_counts.TryGetValue(pair.Key, out int have) ||
                have != pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    private void Succeed(Recipe recipe)
    {
        if (_lastProduct != null)
            Destroy(_lastProduct);

        GameObject prefab =
            recipe.PrefabGetter != null
                ? recipe.PrefabGetter()
                : null;

        if (prefab != null && productSpawnPoint != null)
        {
            _lastProduct = Instantiate(
                prefab,
                productSpawnPoint.position,
                productSpawnPoint.rotation,
                productSpawnPoint
            );

            _lastProduct.transform.localPosition = Vector3.zero;
            _lastProduct.SetActive(true);
        }

        SetResult(
            $"Created {recipe.DisplayName} ({recipe.Formula})",
            new Color(0.45f, 1f, 0.7f)
        );

        PlayTone(
            customSuccessClip != null
                ? customSuccessClip
                : _generatedSuccessClip,
            0.7f
        );
    }

    private void Fail(string message)
    {
        if (failEffectPrefab != null && productSpawnPoint != null)
        {
            GameObject fx = Instantiate(
                failEffectPrefab,
                productSpawnPoint.position,
                Quaternion.identity,
                productSpawnPoint
            );

            Destroy(fx, 2f);
        }

        SetResult(
            $"Reaction failed — {message}",
            new Color(1f, 0.35f, 0.32f)
        );

        PlayTone(
            customFailClip != null
                ? customFailClip
                : _generatedFailClip,
            0.85f
        );
    }

    private void BuildRecipes()
    {
        _recipes.Clear();

        _recipes.Add(Make("H2O", "Water", "H₂O",
            Dict("H", 2, "O", 1), () => waterPrefab));

        _recipes.Add(Make("NaCl", "Table Salt", "NaCl",
            Dict("Na", 1, "Cl", 1), () => saltPrefab));

        _recipes.Add(Make("CO2", "Carbon Dioxide", "CO₂",
            Dict("C", 1, "O", 2), () => carbonDioxidePrefab));

        _recipes.Add(Make("MgO", "Magnesium Oxide", "MgO",
            Dict("Mg", 1, "O", 1), () => magnesiumOxidePrefab));

        _recipes.Add(Make("FeS", "Iron Sulfide", "FeS",
            Dict("Fe", 1, "S", 1), () => ironSulfidePrefab));

        _recipes.Add(Make("NH3", "Ammonia", "NH₃",
            Dict("N", 1, "H", 3), () => ammoniaPrefab));

        _recipes.Add(Make("KOH", "Potassium Hydroxide", "KOH",
            Dict("K", 1, "O", 1, "H", 1), () => kohPrefab));

        _recipes.Add(Make("CaOH2", "Calcium Hydroxide", "Ca(OH)₂",
            Dict("Ca", 1, "O", 2, "H", 2), () => calciumHydroxidePrefab));

        _recipes.Add(Make("LiF", "Lithium Fluoride", "LiF",
            Dict("Li", 1, "F", 1), () => lithiumFluoridePrefab));

        _recipes.Add(Make("BeCl2", "Beryllium Chloride", "BeCl₂",
            Dict("Be", 1, "Cl", 2), () => berylliumChloridePrefab));

        _recipes.Add(Make("BF3", "Boron Trifluoride", "BF₃",
            Dict("B", 1, "F", 3), () => boronTrifluoridePrefab));

        _recipes.Add(Make("AlCl3", "Aluminum Chloride", "AlCl₃",
            Dict("Al", 1, "Cl", 3), () => aluminumChloridePrefab));

        _recipes.Add(Make("SiO2", "Silica / Sand", "SiO₂",
            Dict("Si", 1, "O", 2), () => silicaPrefab));

        _recipes.Add(Make("P4O10", "Phosphorus Pentoxide", "P₄O₁₀",
            Dict("P", 4, "O", 10), () => phosphorusPentoxidePrefab));

        _recipes.Add(Make("SO2", "Sulfur Dioxide", "SO₂",
            Dict("S", 1, "O", 2), () => sulfurDioxidePrefab));

        _recipes.Add(Make("PCl3", "Phosphorus Trichloride", "PCl₃",
            Dict("P", 1, "Cl", 3), () => phosphorusTrichloridePrefab));

        _recipes.Add(Make("CaF2", "Calcium Fluoride", "CaF₂",
            Dict("Ca", 1, "F", 2), () => calciumFluoridePrefab));

        _recipes.Add(Make("Al2S3", "Aluminum Sulfide", "Al₂S₃",
            Dict("Al", 2, "S", 3), () => aluminumSulfidePrefab));

        _recipes.Add(Make("Na2S", "Sodium Sulfide", "Na₂S",
            Dict("Na", 2, "S", 1), () => sodiumSulfidePrefab));

        _recipes.Add(Make("MgCl2", "Magnesium Chloride", "MgCl₂",
            Dict("Mg", 1, "Cl", 2), () => magnesiumChloridePrefab));
    }

    private static Recipe Make(
        string id,
        string name,
        string formula,
        Dictionary<string, int> ratio,
        System.Func<GameObject> getter)
    {
        return new Recipe
        {
            Id = id,
            DisplayName = name,
            Formula = formula,
            Ratio = ratio,
            PrefabGetter = getter
        };
    }

    private static Dictionary<string, int> Dict(params object[] kv)
    {
        Dictionary<string, int> dictionary =
            new Dictionary<string, int>();

        for (int i = 0; i < kv.Length; i += 2)
            dictionary[(string)kv[i]] = (int)kv[i + 1];

        return dictionary;
    }

    private void EnsureAudio()
    {
        _audio = GetComponent<AudioSource>();

        if (_audio == null)
            _audio = gameObject.AddComponent<AudioSource>();

        _audio.playOnAwake = false;

        _generatedSuccessClip = MakeTone(880f, 0.12f);
        _generatedFailClip = MakeBuzz(140f, 0.28f);
    }

    private void PlayTone(AudioClip clip, float volume)
    {
        if (_audio == null || clip == null)
            return;

        _audio.PlayOneShot(clip, volume);
    }

    private static AudioClip MakeTone(float freq, float duration)
    {
        const int rate = 44100;

        int samples = Mathf.CeilToInt(rate * duration);
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)rate;
            float env = 1f - i / (float)samples;

            data[i] =
                Mathf.Sin(2f * Mathf.PI * freq * t) * env;
        }

        AudioClip clip = AudioClip.Create(
            "ChemSuccess",
            samples,
            1,
            rate,
            false
        );

        clip.SetData(data, 0);

        return clip;
    }

    private static AudioClip MakeBuzz(float freq, float duration)
    {
        const int rate = 44100;

        int samples = Mathf.CeilToInt(rate * duration);
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)rate;
            float env = 1f - i / (float)samples;

            float square =
                Mathf.Sign(Mathf.Sin(2f * Mathf.PI * freq * t));

            data[i] = square * env * 0.45f;
        }

        AudioClip clip = AudioClip.Create(
            "ChemFail",
            samples,
            1,
            rate,
            false
        );

        clip.SetData(data, 0);

        return clip;
    }

    private void SetResult(string message, Color color)
    {
        if (resultText == null)
            return;

        resultText.text = message;
        resultText.color = color;
    }
}