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
    private int seed = 1337;
    private Material planetMaterial;
    private int planetFaceResolution = 192;
    private MeshFilter[] meshFilters;
    private Mesh planetMesh;
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
        shapeSettings.planetRadius = 200;
        shapeGenerator = new ShapeGenerator(shapeSettings);

        // Make planet mesh support 4 billion vertices
        planetMesh = new Mesh();
        planetMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new Terrainface[6];
        faceMeshes = new GameObject[6];
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
        }
    }

    private void GenerateMesh()
    {
        foreach (Terrainface face in terrainFaces)
        {
            face.ConstructMesh();
        }    
    }

    //private void GenerateColors()
    //{
    //    float rnd = UnityEngine.Random.Range(0, 255);
    //    rnd = rnd / 255f;
    //    Debug.Log("Color value rnd: " + rnd);
    //
    //    foreach (GameObject m in faceMeshes)
    //    {
    //        
    //        m.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0.5f, rnd, rnd);
    //        
    //    }
    //}
}

