using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    #region member

    [SerializeField] private bool autoGenerate;
    [SerializeField] private Material desertMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material iceMaterial;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerObject;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (autoGenerate)
        {
            GenerateNewPlanet();
            SpawnPlayer();
        }
    }

    #endregion

    #region Public Methods

    public void GenerateNewPlanet()
    {
        DeleteOldPlanet();
        CreatePlanetObject();

        Debug.Log("Generated New Planet");
    }
    public void SpawnPlayer()
    {
        GameObject player = Instantiate(playerObject);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0, 215f, 0);

        Destroy(Camera.main.gameObject);
    }

    #endregion

    #region private Methods
    private void CreatePlanetObject()
    {
        Planet planet = new Planet();

        // Create planet GameObject
        GameObject planetObject = new GameObject("Planet");
        planetObject.transform.position = Vector3.zero;
        planetObject.AddComponent<GravityAttractor>();
        planetObject.tag = "Planet";

        // Add created planet face meshes to planet object, 6 sides
        for (int i = 0; i < planet.faceMeshes.Length; i++)
        {
            GameObject planetFace = planet.faceMeshes[i];
            planetFace.transform.parent = planetObject.transform;

            // Add material to planet face
            planetFace.GetComponent<MeshRenderer>().sharedMaterial = desertMaterial;
        }
    }
    private void DeleteOldPlanet()
    {
        // If there is an old planet, destroy it and create a new one
        GameObject planet;
        if (planet = GameObject.Find("Planet"))
        {
            Destroy(planet);
        }
    }

    #endregion
}
