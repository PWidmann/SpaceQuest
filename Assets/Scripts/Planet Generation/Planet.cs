using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Global Enums
public enum PlanetType { Green, Ice, Desert, Lava, Poison }
public enum PlanetWeather { Clear, Cloudy, Storm, Foggy }
#endregion

public class Planet
{
    #region Members

    private GameObject[] faceMeshes;
    private float minHeightValue = 300;
    private float maxHeightValue = 0;

    private PlanetType planetType;
    private Material planetMaterial;
    private int planetFaceResolution = 256;
    private MeshFilter[] meshFilters;
    private Terrainface[] terrainFaces;
    private ShapeGenerator shapeGenerator;
    private ShapeSettings shapeSettings;
    private AnimationCurve terrainHeightAnimCurve;
    private bool exposeBaseSurface;

    public float MinHeightValue { get => minHeightValue; }
    public float MaxHeightValue { get => maxHeightValue; }
    public GameObject[] FaceMeshes { get => faceMeshes; set => faceMeshes = value; }


    #endregion

    #region constructor

    public Planet(AnimationCurve _animationCurve, bool _exposeBaseSurface)
    {
        exposeBaseSurface = _exposeBaseSurface;
        terrainHeightAnimCurve = _animationCurve;
        GeneratePlanet();
    }

    #endregion

    #region Methods

    public void GeneratePlanet()
    {
        Initialize();
        CreateMeshObjects();
        GenerateAndApplyMeshData();
        SetMinMaxHeight();
    }

    private void Initialize()
    {
        shapeSettings = new ShapeSettings();
        SetRandomShapeSettings();
        
        shapeGenerator = new ShapeGenerator(shapeSettings);
        
        meshFilters = new MeshFilter[6];
        terrainFaces = new Terrainface[6];
        FaceMeshes = new GameObject[6];
    }

    private void CreateMeshObjects()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {

                FaceMeshes[i] = new GameObject("PlanetFace");
                FaceMeshes[i].AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshFilters[i] = FaceMeshes[i].AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();

                FaceMeshes[i].AddComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;
                FaceMeshes[i].GetComponent<MeshCollider>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

                meshFilters[i].sharedMesh.RecalculateNormals();
            }

            terrainFaces[i] = new Terrainface(shapeGenerator, meshFilters[i].sharedMesh, planetFaceResolution, directions[i], terrainHeightAnimCurve);
            FaceMeshes[i].GetComponent<MeshFilter>().sharedMesh = terrainFaces[i].Mesh;
            FaceMeshes[i].GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        }
    }

    private void GenerateAndApplyMeshData()
    {
        foreach (Terrainface face in terrainFaces)
        {
            face.GenerateMeshData();
            face.FlatShading();
            face.ApplyMesh();
        }
    }

    private void SetMinMaxHeight()
    {
        foreach (Terrainface face in terrainFaces)
        {
            // Set min terrain height
            if (face.MinHeightValue < minHeightValue)
                minHeightValue = face.MinHeightValue;
            // Set max terrain height
            if (face.MaxHeightValue > MaxHeightValue)
                maxHeightValue = face.MaxHeightValue;
        }
    }

    private void SetRandomShapeSettings()
    {
        // Layer 1 | Mask Layer
        shapeSettings.planetRadius = 200;
        shapeSettings.noiseSettingsL1.filterType = NoiseSettings.FilterType.Simple;
        shapeSettings.noiseSettingsL1.useFirstLayerAsMask = false;

        // How much base globe exposed to the surface
        // minvalue 0.3 = no water, 0,7 water
        if (shapeSettings.noiseSettingsL1.exposePlanetGround)
            shapeSettings.noiseSettingsL1.simpleNoiseSettings.minValue = 0.7f;
        else
            shapeSettings.noiseSettingsL1.simpleNoiseSettings.minValue = 0.3f;

        // Random noise movement
        float rnd = UnityEngine.Random.Range(0f, 2f);
        shapeSettings.noiseSettingsL1.simpleNoiseSettings.centre = new Vector3(rnd, rnd, rnd);

        // Layer 2 settings
        shapeSettings.noiseSettingsL2.filterType = NoiseSettings.FilterType.Rigid;
        shapeSettings.noiseSettingsL2.useFirstLayerAsMask = true;
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.strength = UnityEngine.Random.Range(1.0f, 2f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.baseRoughness = UnityEngine.Random.Range(1f, 2f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.roughness = UnityEngine.Random.Range(1.2f, 1.8f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.minValue = UnityEngine.Random.Range(0.1f, 1.1f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.weightMultiplier = UnityEngine.Random.Range(0.9f, 1.5f);

        // Random noise movement
        float rnd2 = UnityEngine.Random.Range(0f, 50f);
        shapeSettings.noiseSettingsL2.rigidNoiseSettings.centre = new Vector3(rnd2, rnd2, rnd2);
    }

    #endregion
}

