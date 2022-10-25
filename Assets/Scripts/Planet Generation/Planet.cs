using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Global Enums
public enum PlanetType { Desert, Green, Ice }
public enum PlanetWeather { Clear, Cloudy, Storm, Foggy}
public enum PlanetShape { Flat, BigMountain}
#endregion

public class Planet
{
    #region Members

    public GameObject[] faceMeshes;

    private PlanetType planetType;
    //private int seed = 1337;
    private Material planetMaterial;
    private int planetFaceResolution = 256;
    private MeshFilter[] meshFilters;
    private Terrainface[] terrainFaces;
    private ShapeGenerator shapeGenerator;
    private ShapeSettings shapeSettings;
    

    #endregion

    public Planet()
    {
        GeneratePlanet();
    }

    public void GeneratePlanet()
    {
        Initialize();
        CreateMeshObjects();
        GenerateMesh();
    }

    private void Initialize()
    {
        shapeSettings = new ShapeSettings();
        //SetShapeSettings();
        SetRandomShapeSettings();
        
        shapeGenerator = new ShapeGenerator(shapeSettings);
        
        meshFilters = new MeshFilter[6];
        terrainFaces = new Terrainface[6];
        faceMeshes = new GameObject[6];
    }

    private void SetShapeSettings()
    {
        // "Normal" settings with light mountains
        // 
        // Layer 1 | Mask Layer
        shapeSettings.planetRadius = 200;
        shapeSettings.noiseSettingsL1.useFirstLayerAsMask = false;

        // Layer 2 
        shapeSettings.noiseSettingsL2.filterType = NoiseSettings.FilterType.Rigid;
        shapeSettings.noiseSettingsL2.useFirstLayerAsMask = true;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.strength = 3.05f;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.numLayers = 5;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.baseRoughness = 1.71f;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.roughness = -0.34f;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.persistence = 0.5f;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.minValue = 0.6f;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.weightMultiplier = 0.5f;

    }

    private void SetRandomShapeSettings()
    {
        // Layer 1 | Mask Layer
        shapeSettings.planetRadius = 200;
        shapeSettings.noiseSettingsL1.filterType = NoiseSettings.FilterType.Simple;
        shapeSettings.noiseSettingsL1.useFirstLayerAsMask = false;

        float rnd = UnityEngine.Random.Range(0f, 2f);
        shapeSettings.noiseSettingsL1.simpleNoiseSettings.centre = new Vector3(rnd, rnd, rnd);
        



        // Layer 2 
        shapeSettings.noiseSettingsL2.filterType = NoiseSettings.FilterType.Rigid;
        shapeSettings.noiseSettingsL2.useFirstLayerAsMask = true;

        shapeSettings.noiseSettingsL2.rigidNoiseSettings.strength = UnityEngine.Random.Range(1.0f, 2f);
        //Debug.Log("strength: " + shapeSettings.noiseSettingsL2.rigidNoiseSettings.strength);

        shapeSettings.noiseSettingsL2.rigidNoiseSettings.baseRoughness = UnityEngine.Random.Range(1f, 2f);
        //Debug.Log("base roughness: " + shapeSettings.noiseSettingsL2.rigidNoiseSettings.baseRoughness);

        shapeSettings.noiseSettingsL2.rigidNoiseSettings.roughness = UnityEngine.Random.Range(1.2f, 1.8f);
        //Debug.Log("roughness: " + shapeSettings.noiseSettingsL2.rigidNoiseSettings.roughness);

        shapeSettings.noiseSettingsL2.rigidNoiseSettings.minValue = UnityEngine.Random.Range(0.1f, 1.1f);
        //Debug.Log("minvalue: " + shapeSettings.noiseSettingsL2.rigidNoiseSettings.minValue);

        shapeSettings.noiseSettingsL2.rigidNoiseSettings.weightMultiplier = UnityEngine.Random.Range(0.9f, 1.5f);
        //Debug.Log("weightMultiplier: " + shapeSettings.noiseSettingsL2.rigidNoiseSettings.weightMultiplier);

        float rnd2 = UnityEngine.Random.Range(0f, 50f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.centre = new Vector3(rnd2, rnd2, rnd2);
        
    }

    private void CreateMeshObjects()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {

                faceMeshes[i] = new GameObject("PlanetFace");
                faceMeshes[i].AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("HDRP/Lit"));
                meshFilters[i] = faceMeshes[i].AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();

                faceMeshes[i].AddComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;
                meshFilters[i].sharedMesh.RecalculateNormals();
            }

            terrainFaces[i] = new Terrainface(shapeGenerator, meshFilters[i].sharedMesh, planetFaceResolution, directions[i]);
            faceMeshes[i].GetComponent<MeshFilter>().sharedMesh = terrainFaces[i].mesh;
            faceMeshes[i].GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        }
    }

    private void GenerateMesh()
    {
        //Debug.Log("Before GenerateMesh: " + DateTime.Now.Second + "," + DateTime.Now.Millisecond);
        foreach (Terrainface face in terrainFaces)
        {
            face.ConstructMesh();
        }
        //Debug.Log("After GenerateMesh: " + DateTime.Now.Second + "," + DateTime.Now.Millisecond);
    }
}

