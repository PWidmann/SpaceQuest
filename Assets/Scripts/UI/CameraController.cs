using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Members

    // Main Settings
    [SerializeField] private float mouseSensitivity = 3;
    private float maxDistanceFromTarget = 700;
    private float minDistanceFromTarget = 300f;

    // Camera zoom
    private float currentCameraZoom;
    private float targetCameraZoom;
    private float cameraZoomRate = 25f;
    private float cameraSmoothing = 0.1f;

    //Camera Rotation
    private float rotationSmoothTime = 0.1f;
    private Vector2 pitchMinMax = new Vector2(-89, 89);
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private float yaw;
    private float pitch;

    private bool mouseButtonPressed = false;
    private Vector3 target;
    private bool canRotate = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        // Initialize
        target = Vector3.zero;
        currentCameraZoom = maxDistanceFromTarget;
        targetCameraZoom = maxDistanceFromTarget;

        // Starting camera rotation
        yaw = 0;
        pitch = 0;
        targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;
    }

    private void Update()
    {
        if (canRotate)
        {
            if (Input.GetMouseButton(1))
            {
                mouseButtonPressed = true;
            }
            else
            {
                mouseButtonPressed = false;
            }
        }
    }
    void LateUpdate()
    {
        if (canRotate)
        {
            // Get mouse input and create target rotation
            CreateTargetRotation();

            // Scroll wheel input to camera zoom value
            SetZoomValue();  
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets camera rotation mode around planet active (right click hold to rotate).
    /// </summary>
    public void SetCameraRotationActive()
    {
        canRotate = true;
    }

    private void CreateTargetRotation()
    {
        if (mouseButtonPressed)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * -1;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            targetRotation = new Vector3(pitch, yaw);  
        }

        // Set camera rotation
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
    }

    private void SetZoomValue()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            if (targetCameraZoom > minDistanceFromTarget)
                targetCameraZoom -= cameraZoomRate;
        }
        else if (d < 0f)
        {
            if (targetCameraZoom < maxDistanceFromTarget)
                targetCameraZoom += cameraZoomRate;
        }

        // Set camera distance from target (zoom)
        transform.position = target - transform.forward * currentCameraZoom;
        currentCameraZoom = Mathf.Lerp(currentCameraZoom, targetCameraZoom, cameraSmoothing);
    }

    #endregion
}
