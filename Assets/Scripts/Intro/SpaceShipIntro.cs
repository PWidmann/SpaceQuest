using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public struct WayPointPosition
{
    public Vector3 position;
    public Vector3 rotation;
}

public class SpaceShipIntro : MonoBehaviour
{
    [SerializeField] private GameObject[] wayPoints;

    private int currentWayPoint = 0;
    private Vector3 currentVelocity = Vector3.zero;
    private float timeToReachTargetWayPoint = 3f;
    private float introTimer = 0;
    private PlanetGenerator planetGenerator;
    private bool playerspawned = false;
    private bool introActive = false;

    void Start()
    {
        GameObject camGO = GameObject.Find("Camera");
        camGO.GetComponent<CameraController>().SetCameraRotationActive(false);

        planetGenerator = GameObject.Find("PlanetGenerator").GetComponent<PlanetGenerator>();

        
    }

    public void StartIntro()
    {
        ResetIntro();
    }
    
    void Update()
    {
        if (introActive)
        {
            introTimer += Time.deltaTime;

            if (currentWayPoint == wayPoints.Length - 2 && introTimer >= 3f && !playerspawned)
            {
                playerspawned = true;
                introActive = false;
                planetGenerator.planetGeneratorPanel.SetActive(false);
                planetGenerator.SpawnPlayer();
                
            }

            if (introTimer >= 5f && currentWayPoint < wayPoints.Length - 1)
            {
                currentWayPoint++;
                introTimer = 0;
            }

            if (currentWayPoint < wayPoints.Length - 1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, wayPoints[currentWayPoint + 1].transform.position, ref currentVelocity, timeToReachTargetWayPoint);
                transform.rotation = Quaternion.Slerp(transform.rotation, wayPoints[currentWayPoint + 1].transform.rotation, (timeToReachTargetWayPoint / 6) * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (currentWayPoint < wayPoints.Length - 1)
                    currentWayPoint++;
            }
        }
    }

    public void ResetIntro()
    {
        transform.position = wayPoints[0].transform.position;
        transform.rotation = wayPoints[0].transform.rotation;
        currentWayPoint = 0;
        currentVelocity = Vector3.zero;
        introTimer = 0;
        introActive = true;
    }
}
