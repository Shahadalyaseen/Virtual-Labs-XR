using System.Collections.Generic;
using UnityEngine;

public class ElectronShellOrbit : MonoBehaviour
{
    [Header("Electron")]
    public GameObject electronPrefab;

    [Header("Orbit")]
    public float radius = 1.2f;
    public float speed = 70f;
    public float electronSize = 0.07f;

    public bool reverse = false;

    private readonly List<Transform> electrons =
        new List<Transform>();


    // =========================================================
    // CREATE ELECTRONS
    // =========================================================

    public void CreateElectrons(
        int count)
    {
        if (electronPrefab == null)
        {
            Debug.LogWarning(
                "Electron Prefab is missing."
            );

            return;
        }

        if (count <= 0)
            return;


        for (int i = 0; i < count; i++)
        {
            GameObject electron =
                Instantiate(
                    electronPrefab,
                    transform
                );


            // Evenly distribute electrons
            // around the orbit.

            float angle =
                (360f / count) * i;

            float radians =
                angle * Mathf.Deg2Rad;


            float x =
                Mathf.Cos(radians) *
                radius;

            float y =
                Mathf.Sin(radians) *
                radius;


            electron.transform.localPosition =
                new Vector3(
                    x,
                    y,
                    0f
                );


            electron.transform.localRotation =
                Quaternion.identity;


            // IMPORTANT:
            // Small size instead of 45
            electron.transform.localScale =
                Vector3.one *
                electronSize;


            electrons.Add(
                electron.transform
            );
        }
    }


    // =========================================================
    // ROTATION
    // =========================================================

    private void Update()
    {
        if (electrons.Count == 0)
            return;


        float direction =
            reverse ? -1f : 1f;


        transform.Rotate(
            Vector3.forward,
            speed *
            direction *
            Time.deltaTime,
            Space.Self
        );
    }
}