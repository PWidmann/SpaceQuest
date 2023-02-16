using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;
using System.IO;
using UnityEditor;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PlanetGenerator : MonoBehaviour
{
    #region Members
    [Header("General Settings")]
    [SerializeField] private bool DevMode;
    [SerializeField] private bool combinePlanetFaces;

    [Header("UI Panel")]
    [SerializeField] public GameObject planetGeneratorPanel;
    [SerializeField] public GameObject fadeScreenCanvasPrefab;

    [Header("Planet Materials")]
    [SerializeField] private Material surfaceMaterial;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject lavaSpherePrefab;

    [Header("Planet Configurations")]
    [SerializeField] private PlanetScriptableObject[] configurations;

    private PlanetScriptableObject currentPlanetConfiguration;
    private Texture2D texture;
    private int textureResolution = 50;
    private GameObject planetObject;
    private Planet planet;
    private SpawnHelper spawnHelper;
    private Compass compass;
    private SpaceShipIntro spaceshipIntro;
    private FadeScreen fadeScreen;
    private GameGUI playerGUI;
    private float playerSpawnOffset = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private QuestManager questmanager;

    public PlanetScriptableObject CurrentPlanetConfiguration { get => currentPlanetConfiguration; set => currentPlanetConfiguration = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CheckForFadeScreen();
    }

    private void Start()
    {
        spawnHelper = GameObject.Find("QuestManager").GetComponent<SpawnHelper>();
        questmanager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        spaceshipIntro = GameObject.Find("SpaceShip").GetComponent<SpaceShipIntro>();
        playerGUI = GameObject.Find("PlayerGUI").GetComponent<GameGUI>();
        fadeScreen.FadeIn();
        SoundManager.instance.PlayMusic(1);

        if (DevMode) // Dev mode is for manually generating with individual steps
        {
            GameObject camGO = GameObject.Find("Camera");
            camGO.GetComponent<CameraController>().SetCameraRotationActive(true);
            planetGeneratorPanel.SetActive(true);
        }
        else
        {
            // Automatically generate planet and quest and start intro
            planetGeneratorPanel.SetActive(false);
            GenerateNewPlanet();
            GenerateFoliage();
            GeneratePickups();
            SpawnEnemies();
            questmanager.GenerateQuests();
            // Start intro
            spaceshipIntro.StartIntro();
        }
    }
    #endregion
    
    #region Public Methods
    public void GenerateNewPlanet()
    {
        // Get random planet configuration from scriptable objects and initialize planet texture
        CurrentPlanetConfiguration = configurations[UnityEngine.Random.Range(0, configurations.Length)];
        texture = new Texture2D(textureResolution, 1);
        
        // Prevent the same type of planet after another
        CheckForGeneratedPlanetType(CurrentPlanetConfiguration.PlanetType);

        DeleteOldPlanet();
        CreatePlanetObject();
        UpdateColors();
    }
    public void GenerateFoliage()
    {
        GameObject foliage = new GameObject("Foliage");
        foliage.transform.parent = planetObject.transform;

        // Spawn Trees
        if (CurrentPlanetConfiguration.TreePrefabs.Length > 0)
        {
            for (int i = 0; i < 300; i++)
            {
                int rnd = UnityEngine.Random.Range(0, CurrentPlanetConfiguration.TreePrefabs.Length);
                GameObject tree = Instantiate(CurrentPlanetConfiguration.TreePrefabs[rnd]);
                Vector3 spawnPoint = spawnHelper.GetRandomSurfaceSpawnPoint();
                if (spawnPoint != Vector3.zero)
                {
                    tree.transform.position = spawnPoint;
                    tree.transform.rotation = tree.transform.rotation * Quaternion.Euler(0, UnityEngine.Random.Range(0, 350), 0);

                    Vector3 toAttractorDir = (tree.transform.position - transform.position).normalized;
                    Vector3 bodyUp = tree.transform.up;
                    tree.transform.rotation = Quaternion.FromToRotation(bodyUp, toAttractorDir) * tree.transform.rotation;
                }

                tree.transform.parent = foliage.transform;
            }
        }
    }
    public void GeneratePickups()
    {
        GameObject pickups = new GameObject("Pickups");
        pickups.transform.parent = planetObject.transform;

        // Spawn Pickups
        if (CurrentPlanetConfiguration.Pickups.Length > 0)
        {
            for (int i = 0; i < 200; i++)
            {
                int rnd = UnityEngine.Random.Range(0, CurrentPlanetConfiguration.Pickups.Length);
                GameObject pickUpObject = Instantiate(CurrentPlanetConfiguration.Pickups[rnd]);
                Vector3 spawnPoint = spawnHelper.GetRandomSurfaceSpawnPoint();
                if (spawnPoint != Vector3.zero)
                {
                    pickUpObject.transform.position = spawnPoint;
                    pickUpObject.transform.rotation = pickUpObject.transform.rotation * Quaternion.Euler(0, UnityEngine.Random.Range(0, 350), 0);

                    Vector3 toAttractorDir = (pickUpObject.transform.position - transform.position).normalized;
                    Vector3 bodyUp = pickUpObject.transform.up;
                    pickUpObject.transform.rotation = Quaternion.FromToRotation(bodyUp, toAttractorDir) * pickUpObject.transform.rotation;

                    pickUpObject.transform.parent = pickups.transform;
                }
            }
        }
    }
    public void SpawnPlayer()
    {
        GameObject player = Instantiate(playerObject);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = MostNordPointOnPlanetTerrain();
        playerGUI.SetCompass(true);
        fadeScreen.FadeIn();

        // Set current planet view camera inactive
        Camera.main.gameObject.SetActive(false);
        planetGeneratorPanel.SetActive(false);

        GameObject.Find("Compass").GetComponent<Compass>().QuestGiver = GameObject.Find(questmanager.IntroQuest.QuestGiverName);

        // Set player GO in all NPCs
        foreach (GameObject enemy in spawnedEnemies)
        {
            enemy.GetComponent<SimpleEnemyController>().ActivateNPC(player);
        }

        playerGUI.SetPlayerHealthActive(true);
    }
    #endregion

    #region private Methods
    private void SpawnEnemies()
    {
        spawnedEnemies.Clear();
        GameObject enemiesParent = new GameObject("Enemies");
        enemiesParent.transform.SetParent(planetObject.transform);

        for (int i = 0; i < 100; i++)
        {
            GameObject enemy = Instantiate(CurrentPlanetConfiguration.randomEnemy);
            enemy.transform.position = spawnHelper.GetRandomSurfaceSpawnPoint();
            Vector3 toAttractorDir = (enemy.transform.position - Vector3.zero).normalized;
            Vector3 bodyUp = enemy.transform.up;
            enemy.transform.rotation = Quaternion.FromToRotation(bodyUp, toAttractorDir) * enemy.transform.rotation;
            enemy.transform.SetParent(enemiesParent.transform);
            spawnedEnemies.Add(enemy);
        }

        Debug.Log("common enemies spawned");
    }
    private void CheckForGeneratedPlanetType(PlanetType currentPlanetType)
    {
        List<int> availableConfigurations = new List<int>();
        for (int i = 0; i < configurations.Length; i++)
        {
            availableConfigurations.Add(i);
        }

        while (availableConfigurations.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableConfigurations.Count);
            CurrentPlanetConfiguration = configurations[availableConfigurations[randomIndex]];

            if (CurrentPlanetConfiguration.PlanetType != GameManager.LastPlanet)
            {
                GameManager.LastPlanet = CurrentPlanetConfiguration.PlanetType;
                break;
            }
            availableConfigurations.RemoveAt(randomIndex);
        }
    }
    private Vector3 MostNordPointOnPlanetTerrain()
    {
        Vector3 outputPoint = Vector3.zero;

        Ray ray = new Ray(new Vector3(0, 400f, -playerSpawnOffset), Vector3.down);
        int ground = 1 << LayerMask.NameToLayer("PlanetGround") | 1 << LayerMask.NameToLayer("Lava");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance: 500f, ground))
        {
            outputPoint = hit.point + new Vector3(0, 0, 0);

            if (hit.transform.gameObject.tag == "Lava")
            {
                Debug.Log("Spawned on lava, move spawn point");
                playerSpawnOffset += 10f;
                outputPoint = MostNordPointOnPlanetTerrain();
            }
        }

        return outputPoint;
    }
    private void CreatePlanetObject()
    {
        planet = new Planet(CurrentPlanetConfiguration.TerrainHeightCurve, CurrentPlanetConfiguration.exposeBaseSurface);

        // Create planet GameObject
        planetObject = null;
        planetObject = new GameObject("Planet");
        planetObject.tag = "Planet";
        planetObject.transform.position = Vector3.zero;
        planetObject.AddComponent<GravityAttractor>();
        planetObject.tag = "Planet";

        // Add created planet face meshes to planet object, 6 sides
        for (int i = 0; i < planet.FaceMeshes.Length; i++)
        {
            GameObject planetFace = planet.FaceMeshes[i];
            planetFace.layer = LayerMask.NameToLayer("PlanetGround");
            planetFace.tag = "Planet";
            planetFace.transform.parent = planetObject.transform;
            planetFace.GetComponent<MeshRenderer>().sharedMaterial = surfaceMaterial;
            planetFace.GetComponent<MeshCollider>().sharedMesh = planetFace.GetComponent<MeshFilter>().mesh;
        }

        // If lava planet, instantiate a lava sphere
        if (CurrentPlanetConfiguration.PlanetType == PlanetType.Lava)
        {
            GameObject lavaSphere = Instantiate(lavaSpherePrefab, Vector3.zero, Quaternion.identity);
            lavaSphere.transform.SetParent(planetObject.transform);
        }

        if (combinePlanetFaces)
        {
            CombineFaceMeshes();   
        }
    }
    private void UpdateColors()
    {
        // Create texture colors
        Color[] colors = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = CurrentPlanetConfiguration.TerrainHeightColor.Evaluate(i / (textureResolution - 1f));
        }
        texture.SetPixels(colors);
        texture.Apply();

        // Set shader values
        surfaceMaterial.SetTexture("_Texture", texture);
        surfaceMaterial.SetVector("_ElevationMinMax", new Vector2(planet.MinHeightValue, planet.MaxHeightValue));
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
        planetObject.transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        planetObject.transform.AddComponent<MeshCollider>().sharedMesh = planetObject.transform.GetComponent<MeshFilter>().mesh;
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
    private void CheckForFadeScreen()
    {
        try
        {
            fadeScreen = GameObject.Find("FadeCanvas").GetComponent<FadeScreen>();
        }
        catch
        {
            GameObject fadeCanvas = GameObject.Instantiate(fadeScreenCanvasPrefab);
            fadeScreen = fadeCanvas.GetComponent<FadeScreen>();
            fadeScreen.name = "FadeCanvas";
        }
    }
    #endregion
}
