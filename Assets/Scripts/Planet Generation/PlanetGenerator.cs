using Unity.VisualScripting;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    #region member

    [Header("General Settings")]
    [SerializeField] private bool autoGenerateAndPlay;
    [SerializeField] private bool combinePlanetFaces;

    [Header("Planet Materials")]
    [SerializeField] private Material surfaceMaterial;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject lavaSpherePrefab;
    [SerializeField] private GameObject targetNavPoint;
    [SerializeField] private GameObject clericPrefab;

    [Header("Planet Configurations")]
    [SerializeField] private PlanetScriptableObject[] configurations;

    private PlanetScriptableObject currentPlanetConfiguration;
    private Texture2D texture;
    private int textureResolution = 50;
    private GameObject planetObject;
    private Planet planet;
    private bool hasLava;

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

    #endregion

    #region Public Methods

    public void GenerateNewPlanet()
    {
        // Get random planet configuration from scriptable objects and initialize planet texture
        currentPlanetConfiguration = configurations[UnityEngine.Random.Range(0, configurations.Length)];
        texture = new Texture2D(textureResolution, 1);

        DeleteOldPlanet();
        CreatePlanetObject();
        UpdateColors();

        Debug.Log("Created new " + currentPlanetConfiguration.PlanetType.ToString() + " Planet");

        GenerateFoliage();
    }

    private void GenerateFoliage()
    {
        
    }

    public void SpawnPlayer()
    {

        //GenerateDestinationPoint();

        // Set current planet view camera inactive
        Camera.main.gameObject.SetActive(false);

        

        GameObject player = Instantiate(playerObject);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = MostNordPointOnPlanetTerrain();
        GameObject go = GameObject.Find("GameInterfaceCanvas");
        go.GetComponent<FadeInScreen>().fadeStarted = true;
        go.GetComponentInChildren<GameGUI>().ShowPlayerHUD();
    }

    public void SpawnEnemyRandomPositionOnPlanet()
    {
        GameObject enemy = Instantiate(clericPrefab);
        enemy.name = "Cleric";
        enemy.transform.position = RandomPlanetSpawnPoint();

        Debug.Log("Cleric spawned!");
    }

    #endregion

    #region private Methods

    private Vector3 MostNordPointOnPlanetTerrain()
    {
        Vector3 outputPoint = Vector3.zero;

        Ray ray = new Ray(new Vector3(0, 400f, 0), Vector3.down);
        int ground = 1 << LayerMask.NameToLayer("PlanetGround");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance: 500f, ground))
        {
            outputPoint = hit.point;
        }
        else
        {
            
        }

        return outputPoint;
    }

    private Vector3 RandomPlanetSpawnPoint()
    {
        // Planet terrain max height + margin
        float radius = 300f;

        // Generate a random angle for the x and y axes.
        float angleX = Random.Range(0.0f, 360.0f);
        float angleY = Random.Range(0.0f, 360.0f);

        // Calculate the x, y, and z coordinates of the point on the sphere using the input radius and the generated angles.
        float x = radius * Mathf.Sin(angleX) * Mathf.Cos(angleY);
        float y = radius * Mathf.Sin(angleX) * Mathf.Sin(angleY);
        float z = radius * Mathf.Cos(angleX);

        Vector3 point = new Vector3(x, y, z);

        

        // Return the point on the sphere as a Vector3.
        return SpawnPointTowardsPlanet(point);
    }

    private Vector3 SpawnPointTowardsPlanet(Vector3 origin)
    {
        // Create a raycast from the input point to Vector3.Zero.
        Ray ray = new Ray(origin, Vector3.zero - origin);
        Vector3 output = Vector3.zero;
        // Perform the raycast and store the result in a RaycastHit object.
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        output = hit.point;

        Debug.Log("Enemy Spawn Point generated: " + output);

        return output;
    }

    private void CreatePlanetObject()
    {
        planet = new Planet(currentPlanetConfiguration.TerrainHeightCurve, currentPlanetConfiguration.exposeBaseSurface);  

        // Create planet GameObject
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
            planetFace.transform.parent = planetObject.transform;

            // Add random material to planet face
            planetFace.GetComponent<MeshRenderer>().sharedMaterial = surfaceMaterial;
        }

        if (lavaSpherePrefab && hasLava)
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
            colors[i] = currentPlanetConfiguration.TerrainHeightColor.Evaluate(i / (textureResolution - 1f));
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
