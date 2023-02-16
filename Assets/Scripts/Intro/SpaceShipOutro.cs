using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShipOutro : MonoBehaviour
{
    #region Members
    [SerializeField] private Transform camPosition;
    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform wayPoint2;

    public bool outroStarted = false;
    private Vector3 currentVelocity = Vector3.zero;
    private float timeToReachTargetWayPoint = 3f;
    private FadeScreen fadeScreen;
    private float outroTimer = 0;
    #endregion

    #region Unity Methods
    private void Start()
    {
        fadeScreen = GameObject.Find("FadeCanvas").GetComponent<FadeScreen>();
    }
    private void Update()
    {
        if (outroStarted)
        {
            outroTimer += Time.deltaTime;

            if (outroTimer > 10)
            {
                SceneManager.LoadScene("GameScene");
            }

            Camera.main.transform.LookAt(transform.position);

            transform.position = Vector3.SmoothDamp(transform.position, waypoint1.transform.position, ref currentVelocity, timeToReachTargetWayPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, waypoint1.transform.rotation, timeToReachTargetWayPoint * Time.deltaTime);
        }
    }
    #endregion

    #region Public Methods
    public void StartOutro()
    {
        Debug.Log("Started outro");
        outroStarted = true;
        Camera.main.transform.parent = null;
        Camera.main.transform.position = camPosition.position;
        fadeScreen.FadeOut();
    }
    #endregion
}
