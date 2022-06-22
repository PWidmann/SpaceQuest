using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 5;
    public Transform target;
    public float maxDistanceFromTarget = 4;
    public float minDistanceFromTarget = 0.5f;

    public float currentDistanceFromTarget;
    public Vector2 pitchMinMax = new Vector2(0, 85);

    Camera cam;

    public float rotationSmoothTime = 0.08f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public LayerMask collisionLayer;

    float yaw;
    float pitch;

    private void Start()
    {
        cam = Camera.main;
        currentDistanceFromTarget = maxDistanceFromTarget;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * -1;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * currentDistanceFromTarget;
    }
}
