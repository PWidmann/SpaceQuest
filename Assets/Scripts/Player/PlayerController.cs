using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Members

    [SerializeField] private Text debugText;
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject cameraArm;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject pointerObjectPrefab;
    [SerializeField] GameObject chest; // For rotating aiming animation
    [SerializeField] Transform gunEndPoint;
    [SerializeField] GameObject laserBeamPrefab;

    private GameObject pointerObject;
    private Vector3 aimPoint;

    private Animator animator;
    private Rigidbody rigidBody;
    private CharacterController controller;

    // Movement
    private Vector2 inputDir;
    private Vector3 velocity;
    private bool crouching = false;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float speedSmoothTime = 0.05f;
    //private float turnSmoothTime = 0.1f;
    private float runSpeed = 6f;
    private float crouchSpeed = 2.3f;
    private float mySpeed = 0;
    private float currentSpeed;
    private float targetSpeed;
    private float animationSpeedPercent;
    private bool grounded = true;

    // Animation
    private float speedPercent;
    

    // Camera
    private Vector2 cameraPitchClamp;
    private float cameraYaw;
    private float cameraPitch;


    // Body aiming
    private Vector3 aimRotOffset = new Vector3(2f, 0, 0); // Best compromise of far and near target aiming rotation
    private Quaternion aimRotation;
    private RaycastHit hit;
    private Vector3 target;
    

    #endregion

    #region UnityMethods

    private void Start()
    {
        InitializePlayerController();

        
    }

    private void Update()
    {
        CheckForGrounded();
        GetInput();

        AnimationHandling();

        Shooting();

    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject beam = Instantiate(laserBeamPrefab, gunEndPoint.position, Quaternion.identity);
            beam.transform.LookAt(target);
            beam.GetComponent<LaserBeam>().direction = pointerObject.transform.position - gunEndPoint.position;
        }

        Debug.DrawLine(gunEndPoint.position, target, Color.red, 0.5f);
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * inputDir.y + transform.right * inputDir.x;
        moveDirection.Normalize();
        rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);

    }

    private void LateUpdate()
    {
        cameraPitch = Math.Clamp(cameraPitch, cameraPitchClamp.x, cameraPitchClamp.y);
        cameraArm.transform.localRotation = Quaternion.Euler(cameraPitch, -10f, 0);

        CreateAimPoint();
    }

    #endregion

    #region Methods

    private void InitializePlayerController()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        cameraPitchClamp = new Vector2(-50f, 70f); // cameraArm X rotation clamp
        cameraPitch = 20f;
        mySpeed = runSpeed;
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        aimPoint = new Vector3();
    }
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("J_Fire1"))
        {
            crouching = !crouching;

            if (crouching)
            {
                animator.SetBool("crouching", true);
                mySpeed = crouchSpeed;
            }
            else
            {
                animator.SetBool("crouching", false);
                mySpeed = runSpeed;
            }
        }

        //debugText.text = "Crouching: " + crouching + "\n" + "Grounded: " + grounded;

        // Rotate camera
        cameraPitch += Input.GetAxis("Mouse Y") * -2; // mouse Y input amount per frame (-1 to 1 in 0.05 steps)
        cameraYaw = Input.GetAxis("Mouse X") * 2;
        // Movement
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rigidBody.AddForce(transform.up * 400f);
            animator.SetTrigger("jump");
        }
    }
    void CheckForGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);
        if (hit.distance > 0.2f)
        {
            grounded = false;
            animator.SetBool("grounded", false);
        }
        else
        {
            grounded = true;
            animator.SetBool("grounded", true);
        }
    }
    private void AnimationHandling()
    {
        targetSpeed = mySpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (currentSpeed < 0.05f)
            currentSpeed = 0;

        // Animation handling
        animationSpeedPercent = currentSpeed / mySpeed;
        animator.SetFloat("xAxis", inputDir.x);
        animator.SetFloat("zAxis", inputDir.y);
        animator.SetFloat("speedPercent", animationSpeedPercent);

        // Rotate character yaw with mouse X input
        transform.Rotate(new Vector3(0, cameraYaw, 0));
    }
    void CreateAimPoint()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        

        if (pointerObject == null)
            pointerObject = Instantiate(pointerObjectPrefab);

        int ground = 1 << LayerMask.NameToLayer("PlanetGround");

        if (Physics.Raycast(ray, out hit, maxDistance: 300f, ground))
        {
            target = hit.point;
            Debug.Log("hit ground");
            aimPoint = target;
            pointerObject.transform.position = aimPoint;
            rifle.transform.LookAt(target);
        }
        

        //Vector3 direction = aimPoint - rifle.transform.position;

        aimRotation = chest.transform.rotation * Quaternion.Euler(cameraPitch * 0.7f, 0, 0);
        chest.transform.rotation = aimRotation;
    }

    #endregion
}
