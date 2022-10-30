using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera Zoom 
    [Header("Camera Zoom")]

    public float mouseSensitivity = 5;
    public float maxDistanceFromTarget = 4;
    public float minDistanceFromTarget = 0.5f;

    private float currentCameraZoom;
    private float targetCameraZoom;
    private float cameraZoomRate = 20f;
    private float cameraSmoothing = 0.05f;

    //Camera Rotation
    private float rotationSmoothTime = 0.1f;
    Vector2 pitchMinMax = new Vector2(-89, 89);
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    Vector3 targetRotation;
    float yaw;
    float pitch;
    Vector2 saveMousePosition;

    private bool cameraRotationActive = false;
    private Vector3 target;
    private bool rotationActive = false;

    private void Start()
    {
        target = Vector3.zero;

        currentCameraZoom = maxDistanceFromTarget;
        targetCameraZoom = maxDistanceFromTarget;

        // Starting camera rotation
        yaw = 0;
        pitch = 0;
        targetRotation = new Vector3(pitch, yaw);

        // Camera smoothing depends on timescale
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
    }

    private void Update()
    {
        if (rotationActive)
        {
            if (Input.GetMouseButton(1))
            {
                cameraRotationActive = true;
                saveMousePosition = Input.mousePosition;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                cameraRotationActive = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    void LateUpdate()
    {
        if (rotationActive)
        {
            //Camera Rotation
            if (cameraRotationActive)
            {
                yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * -1;
                pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
                targetRotation = new Vector3(pitch, yaw);
            }

            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            // Camera Zoom
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

            transform.position = target - transform.forward * currentCameraZoom;
            currentCameraZoom = Mathf.Lerp(currentCameraZoom, targetCameraZoom, cameraSmoothing);
        }
    }

    public void SetCameraRotationActive()
    {
        rotationActive = true;
    }
}
