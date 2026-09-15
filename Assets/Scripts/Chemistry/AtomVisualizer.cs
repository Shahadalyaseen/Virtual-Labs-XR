using TMPro;
using UnityEngine;

public class AtomVisualizer : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public GameObject protonPrefab;
    public GameObject neutronPrefab;
    public GameObject electronPrefab;

    [Header("Atom References")]
    public Transform nucleus;
    public Transform electronOrbit;

    [Header("Nucleus Settings")]
    public float nucleusRadius = 0.45f;
    public float nucleusParticleSize = 0.08f;

    [Header("Electron Settings")]
    public float electronDistance = 1.2f;
    public float electronSize = 0.07f;
    public float electronSpeed = 70f;

    [Header("Atom Rotation")]
    public float atomRotationSpeed = 10f;

    [Header("UI Counters")]
    public TMP_Text protonCountText;
    public TMP_Text neutronCountText;
    public TMP_Text electronCountText;


    private void Update()
    {
        // Rotate the whole electron system
        if (electronOrbit != null)
        {
            electronOrbit.Rotate(
                Vector3.up,
                atomRotationSpeed * Time.deltaTime,
                Space.Self
            );
        }
    }


    public void BuildElement(
        int protons,
        int neutrons,
        int electrons)
    {
        ClearAtom();

        // -------------------------
        // PROTONS
        // -------------------------

        for (int i = 0; i < protons; i++)
        {
            CreateNucleusParticle(
                protonPrefab,
                i,
                protons
            );
        }

        // -------------------------
        // NEUTRONS
        // -------------------------

        for (int i = 0; i < neutrons; i++)
        {
            CreateNucleusParticle(
                neutronPrefab,
                i,
                neutrons
            );
        }

        // -------------------------
        // ELECTRONS
        // -------------------------

        int[] shellCounts =
            GetShellCounts(electrons);

        for (int shell = 0; shell < shellCounts.Length; shell++)
        {
            if (shellCounts[shell] <= 0)
                continue;

            CreateShell(
                shell,
                shellCounts[shell]
            );
        }

        // -------------------------
        // COUNTERS
        // -------------------------

        UpdateCounters(
            protons,
            neutrons,
            electrons
        );
    }


    public void BuildHydrogen()
    {
        BuildElement(1, 0, 1);
    }


    // =========================================================
    // NUCLEUS
    // =========================================================

    private void CreateNucleusParticle(
        GameObject prefab,
        int index,
        int total)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Particle prefab is missing.");
            return;
        }

        if (nucleus == null)
        {
            Debug.LogWarning("Nucleus Transform is missing.");
            return;
        }

        GameObject particle =
            Instantiate(
                prefab,
                nucleus
            );

        // Fibonacci sphere distribution
        float phi =
            Mathf.Acos(
                1f -
                2f *
                (index + 0.5f) /
                Mathf.Max(1, total)
            );

        float theta =
            Mathf.PI *
            (1f + Mathf.Sqrt(5f)) *
            index;

        float x =
            Mathf.Sin(phi) *
            Mathf.Cos(theta);

        float y =
            Mathf.Sin(phi) *
            Mathf.Sin(theta);

        float z =
            Mathf.Cos(phi);

        // Keep particles inside a compact nucleus
        float radius =
            nucleusRadius *
            Mathf.Pow(
                (index + 1f) /
                Mathf.Max(1f, total),
                0.33f
            );

        particle.transform.localPosition =
            new Vector3(
                x * radius,
                y * radius,
                z * radius
            );

        particle.transform.localRotation =
            Random.rotation;

        // IMPORTANT:
        // Small realistic visual size
        particle.transform.localScale =
            Vector3.one *
            nucleusParticleSize;
    }


    // =========================================================
    // ELECTRON SHELLS
    // =========================================================

    private void CreateShell(
        int shellIndex,
        int electronCount)
    {
        if (electronOrbit == null)
        {
            Debug.LogWarning(
                "Electron Orbit Transform is missing."
            );

            return;
        }

        GameObject shellObject =
            new GameObject(
                "ElectronShell_" +
                (shellIndex + 1)
            );

        shellObject.transform.SetParent(
            electronOrbit,
            false
        );

        shellObject.transform.localPosition =
            Vector3.zero;

        shellObject.transform.localScale =
            Vector3.one;


        // Different 3D orientations
        // for each electron shell.

        Quaternion[] rotations =
        {
            Quaternion.Euler(0f, 0f, 0f),

            Quaternion.Euler(
                65f,
                20f,
                0f
            ),

            Quaternion.Euler(
                110f,
                -35f,
                25f
            ),

            Quaternion.Euler(
                35f,
                70f,
                45f
            ),

            Quaternion.Euler(
                145f,
                -40f,
                20f
            ),

            Quaternion.Euler(
                70f,
                35f,
                75f
            ),

            Quaternion.Euler(
                125f,
                -30f,
                55f
            )
        };


        shellObject.transform.localRotation =
            rotations[
                Mathf.Min(
                    shellIndex,
                    rotations.Length - 1
                )
            ];


        ElectronShellOrbit orbit =
            shellObject.AddComponent<
                ElectronShellOrbit
            >();

        orbit.electronPrefab =
            electronPrefab;

        orbit.electronSize =
            electronSize;

        // Each shell gets further away
        orbit.radius =
            electronDistance +
            shellIndex * 0.65f;

        // Outer shells move slightly slower
        orbit.speed =
            electronSpeed *
            Mathf.Clamp(
                1f - shellIndex * 0.08f,
                0.45f,
                1f
            );

        orbit.reverse =
            shellIndex % 2 == 1;


        orbit.CreateElectrons(
            electronCount
        );
    }


    // =========================================================
    // ELECTRON CONFIGURATION
    // =========================================================

    private int[] GetShellCounts(
        int electrons)
    {
        int[] shells =
        {
            0, 0, 0, 0, 0, 0, 0
        };

        int remaining =
            electrons;

        int[,] orbitalOrder =
        {
            { 1, 2 },
            { 2, 2 },
            { 2, 6 },
            { 3, 2 },
            { 3, 6 },
            { 4, 2 },
            { 3, 10 },
            { 4, 6 },
            { 5, 2 },
            { 4, 10 },
            { 5, 6 },
            { 6, 2 },
            { 4, 14 },
            { 5, 10 },
            { 6, 6 },
            { 7, 2 },
            { 5, 14 },
            { 6, 10 },
            { 7, 6 }
        };


        for (
            int i = 0;
            i < orbitalOrder.GetLength(0);
            i++
        )
        {
            int shell =
                orbitalOrder[i, 0] - 1;

            int capacity =
                orbitalOrder[i, 1];

            int amount =
                Mathf.Min(
                    remaining,
                    capacity
                );

            shells[shell] += amount;

            remaining -= amount;

            if (remaining <= 0)
                break;
        }


        return shells;
    }


    // =========================================================
    // UI COUNTERS
    // =========================================================

    private void UpdateCounters(
        int protons,
        int neutrons,
        int electrons)
    {
        if (protonCountText != null)
        {
            protonCountText.text =
                protons.ToString();
        }

        if (neutronCountText != null)
        {
            neutronCountText.text =
                neutrons.ToString();
        }

        if (electronCountText != null)
        {
            electronCountText.text =
                electrons.ToString();
        }
    }


    // =========================================================
    // CLEAR
    // =========================================================

    public void ClearAtom()
    {
        if (nucleus != null)
        {
            for (
                int i = nucleus.childCount - 1;
                i >= 0;
                i--
            )
            {
                Destroy(
                    nucleus.GetChild(i).gameObject
                );
            }
        }


        if (electronOrbit != null)
        {
            for (
                int i = electronOrbit.childCount - 1;
                i >= 0;
                i--
            )
            {
                Destroy(
                    electronOrbit.GetChild(i).gameObject
                );
            }
        }
    }
}