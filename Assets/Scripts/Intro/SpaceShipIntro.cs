using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// This class handles the intro animations of the spaceship and gives a transition into the player game start
/// </summary>
public class SpaceShipIntro : MonoBehaviour
{
    #region Members
    [SerializeField] private GameObject[] wayPoints;
    [SerializeField] private Transform touchpoint1;
    [SerializeField] private Transform touchpoint2;

    private int currentWayPoint = 0;
    private Vector3 currentVelocity = Vector3.zero;
    private float timeToReachTargetWayPoint = 3f;
    private float introTimer = 0;
    private PlanetGenerator planetGenerator;
    private bool playerspawned = false;
    private bool introActive = false;
    private FadeScreen fadeScreen;
    private bool fadeOut = false;
    private bool hasFadeScreen = false;
    private float zSpawnOffsetPos;
    private float xSpawnOffsetPos;
    #endregion

    #region Unity Methods
    void Start()
    {
        GameObject camGO = GameObject.Find("Camera");
        camGO.GetComponent<CameraController>().SetCameraRotationActive(false);
        planetGenerator = GameObject.Find("PlanetGenerator").GetComponent<PlanetGenerator>();
        zSpawnOffsetPos = -20f;
        xSpawnOffsetPos = 0;
    }

    void Update()
    {
        CheckForFadeScreen();

        if (introActive)
        {
            introTimer += Time.deltaTime;

            // Start fading out towards the end of the animation
            if (currentWayPoint == wayPoints.Length - 3 && introTimer >= 3 && !playerspawned)
            {
                if (!fadeOut)
                {
                    fadeScreen.FadeOut();
                    fadeOut = true;
                }
            }

            // When the ship has almost landed
            if (currentWayPoint == wayPoints.Length - 2 && introTimer >= 3f && !playerspawned)
            {
                playerspawned = true;
                introActive = false;

                // Set spaceship land position & spawn Player
                LandSpaceShip();
                planetGenerator.SpawnPlayer();
            }

            // After 5 seconds, switch to the next waypoint
            if (introTimer >= 5f && currentWayPoint < wayPoints.Length - 1)
            {
                currentWayPoint++;
                introTimer = 0;
            }

            // Smooth movement and rotation
            if (currentWayPoint < wayPoints.Length - 1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, wayPoints[currentWayPoint + 1].transform.position, ref currentVelocity, timeToReachTargetWayPoint);
                transform.rotation = Quaternion.Slerp(transform.rotation, wayPoints[currentWayPoint + 1].transform.rotation, (timeToReachTargetWayPoint / 6) * Time.deltaTime);
            }
        }
    }
    #endregion

    #region Public Methods
    public void StartIntro()
    {
        StartAnimation();
    }
    
    public void StartAnimation()
    {
        transform.position = wayPoints[0].transform.position;
        transform.rotation = wayPoints[0].transform.rotation;
        currentWayPoint = 0;
        currentVelocity = Vector3.zero;
        introTimer = 0;
        introActive = true;
    }

    public void LandSpaceShip()
    {
        GameObject spaceShip = GameObject.Find("SpaceShip");
        spaceShip.transform.position = SearchLandPosition();
        Vector3 toAttractorDir = (transform.position - Vector3.zero).normalized;
        Vector3 bodyUp = transform.up;
        transform.rotation = Quaternion.FromToRotation(bodyUp, toAttractorDir) * transform.rotation;
    }
    #endregion

    #region Private Methods
    private void CheckForFadeScreen()
    {
        if (!hasFadeScreen)
        {
            try
            {
                fadeScreen = GameObject.Find("FadeCanvas").GetComponent<FadeScreen>();
            }
            catch
            {

            }
        }
    }

    private Vector3 SearchLandPosition()
    {
        if (zSpawnOffsetPos < -100)
        {
            xSpawnOffsetPos += 10f;
            zSpawnOffsetPos = -20f;
        }

        Vector3 outputPoint = Vector3.zero;
        Ray ray = new Ray(new Vector3(xSpawnOffsetPos, 400f, zSpawnOffsetPos), Vector3.down);
        int ground = 1 << LayerMask.NameToLayer("PlanetGround");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance: 500f, ground))
        {
            if (hit.transform.gameObject.tag != "Lava")
            {
                // If it doesn't find obstructing objects in the landing space
                if (!Physics.CheckSphere(hit.point + new Vector3(0, 6.1f, 0), 6f))
                {
                    outputPoint = hit.point + new Vector3(0, 3.5f, 0);
                }
                else
                {
                    zSpawnOffsetPos -= 5f;
                    outputPoint = SearchLandPosition();
                }
            }
            else
            {
                zSpawnOffsetPos -= 5f;
                outputPoint = SearchLandPosition();
            }
        }

        if (outputPoint == Vector3.zero)
        {
            outputPoint = SearchLandPosition();
        }

        return outputPoint;
    }
    #endregion
}
