using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Text debugText;


    private Animator animator;
    private CharacterController controller;

    // Movement
    private Vector2 inputDir;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float speedSmoothTime = 0.10f;
    //private float turnSmoothTime = 0.1f;
    private float runSpeed = 6f;
    private float crouchSpeed = 2.3f;
    private float currentSpeed;
    private float targetSpeed;
    private float animationSpeedPercent;

    // Animation
    private float speedPercent;

    private Rigidbody rigidBody;

    // Camera
    private Vector2 cameraPitchClamp;
    private float cameraYaw;
    private float cameraPitch;

    private Transform cameraArm;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        cameraArm = transform.GetChild(3);
        cameraPitchClamp = new Vector2(-50f, 70f); // cameraArm X rotation clamp
        cameraPitch = 0;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        // Rotate camera
        cameraPitch += Input.GetAxis("Mouse Y") * -2; // mouse Y input amount per frame (-1 to 1 in 0.05 steps)
        cameraYaw = Input.GetAxis("Mouse X") * 2;

        
        // Movement
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(transform.up * 400f);
            animator.SetTrigger("jump");
        }

        targetSpeed = crouchSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        
        if (currentSpeed < 0.05f)
            currentSpeed = 0;

        animationSpeedPercent = currentSpeed / crouchSpeed;

        // Animation handling
        animator.SetFloat("xAxis", inputDir.x);
        animator.SetFloat("zAxis", inputDir.y);
        animator.SetFloat("speedPercent", animationSpeedPercent);

        // Rotate character yaw with mouse X input
        transform.Rotate(new Vector3(0, cameraYaw, 0));
        
        
    }

    private void LateUpdate()
    {
        cameraPitch = Math.Clamp(cameraPitch, cameraPitchClamp.x, cameraPitchClamp.y);
        debugText.text = cameraPitch.ToString();
        cameraArm.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * inputDir.y + transform.right * inputDir.x;
        moveDirection.Normalize();
        rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }
}
