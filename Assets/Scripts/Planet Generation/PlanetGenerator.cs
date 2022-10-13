using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetGenerator : MonoBehaviour
{
    #region member

    [Header("General Settings")]
    [SerializeField] private bool autoGenerateAndPlay;
    [SerializeField] private bool combinePlanetFaces;

    [Header("Planet Materials")]
    [SerializeField] private Material surfaceMaterial;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerObject;

    private GameObject planetObject;
    private long timerMS = 0;

    float yaw;
    float pitch;
    float mouseSensitivity = 150f;


    #endregion

    #region Unity Methods

    private void Start()
    {
        if (autoGenerateAndPlay)
        {
            GenerateNewPlanet();
            SpawnPlayer();
        }
    }

    private void Update()
    {
        GameObject player;
        if (player = GameObject.FindGameObjectWithTag("Player"))
        {

        }
        else
        {
            DebugPlanetRotation();
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
        if (IsFirstPlayerSpawn())
            Destroy(Camera.main.gameObject);

        GameObject player = Instantiate(playerObject);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0, 215f, 0);
    }

    #endregion

    #region private Methods

    private void DebugPlanetRotation()
    {
        if (planetObject != null && Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            planetObject.transform.eulerAngles = new Vector3(-pitch, -yaw, 0);
        }
    }


    private void CreatePlanetObject()
    {
        
        
        Planet planet = new Planet();
        
        

        // Create planet GameObject
        planetObject = new GameObject("Planet");
        planetObject.transform.position = Vector3.zero;
        

        planetObject.AddComponent<GravityAttractor>();
        planetObject.tag = "Planet";

        // Add created planet face meshes to planet object, 6 sides


        for (int i = 0; i < planet.faceMeshes.Length; i++)
        {
            GameObject planetFace = planet.faceMeshes[i];
            planetFace.transform.parent = planetObject.transform;

            // Add random material to planet face
            planetFace.GetComponent<MeshRenderer>().sharedMaterial = surfaceMaterial;
        }
        

        if (combinePlanetFaces)
        {
            
            CombineFaceMeshes();
            
        }
    }

    private void CombineFaceMeshes()
    {
        MeshFilter[] meshObjectsToCombine = planetObject.GetComponentsInChildren<MeshFilter>();
        Material mat = planetObject.GetComponentInChildren<MeshRenderer>().material;
        CombineInstance[] combine = new CombineInstance[meshObjectsToCombine.Length];

        int i = 0;
        while (i < meshObjectsToCombine.Length)
        {
            combine[i].mesh = meshObjectsToCombine[i].sharedMesh;
            combine[i].transform = meshObjectsToCombine[i].transform.localToWorldMatrix;
            meshObjectsToCombine[i].gameObject.SetActive(false);

            i++;
        }

        planetObject.transform.AddComponent<MeshRenderer>().material = mat;
        planetObject.transform.AddComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        planetObject.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        //planetObject.transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        planetObject.transform.AddComponent<MeshCollider>().sharedMesh = planetObject.transform.GetComponent<MeshFilter>().mesh;
    }



    private bool IsFirstPlayerSpawn()
    {
        // If there is an old player, destroy it
        GameObject player;
        if (player = GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(player);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void DeleteOldPlanet()
    {
        // If there is an old planet, destroy it
        GameObject planet;
        if (planet = GameObject.Find("Planet"))
        {
            Destroy(planet);
        }
    }

    #endregion
}
