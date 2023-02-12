using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    #region Members

    [Header("Misc")]
    [SerializeField] private GameObject cameraArm;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject pointerObjectPrefab;
    [SerializeField] private GameObject chest; // For rotating aiming animation
    [SerializeField] private Transform gunEndPoint;
    [SerializeField] private GameObject laserBeamPrefab;
    [SerializeField] private GameObject flashLightGO;

    private Animator animator;
    private Rigidbody rigidBody;
    private CharacterController controller;

    // Movement
    private bool playerHasControl = true;
    private Vector2 inputDir;
    private Vector3 velocity;
    private bool crouching = false;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float speedSmoothTime = 0.05f;
    //private float turnSmoothTime = 0.1f;
    private float runSpeed = 8f; // about 8 intended
    private float crouchSpeed = 2.3f;
    private float mySpeed = 0;
    private float currentSpeed;
    private float targetSpeed;
    private float animationSpeedPercent;
    private bool grounded = true;

    // Animation
    private float speedPercent;

    // Camera
    private Camera playerCamera;
    private Vector2 cameraPitchClamp;
    private float cameraYaw;
    private float cameraPitch;

    // Body aiming
    private Vector3 aimRotOffset = new Vector3(2f, 0, 0); // Best compromise of far and near target aiming rotation
    private Quaternion aimRotation;
    private RaycastHit hit;
    private Vector3 aimTarget;

    // Shooting
    private float autoShootRate = 1.0f;
    private float shootTimer = 0f;
    private bool flashLight = false;
    private float standardFOV = 60f;
    private float targetFOV = 60f;
    private float currentFOV = 0;
    private float zoomFOV = 30f;

    // Floor checking
    private Ray floorcheckRay;
    private RaycastHit floorcheckHit;
    private float lavaTimer = 0;
    private bool death = false;

    private GameGUI playerGUI;
    private FadeScreen fadeScreen;

    private Vector3 spawnPoint = Vector3.zero;
    private float respawnTimer = 5f;
    private bool respawnStarted = false;

    public bool Death { get => death; set => death = value; }
    public Vector3 SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

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
        Zoom();

        CheckForLava();
        Respawn();
    }
    private void FixedUpdate()
    {
        if (playerHasControl)
        {
            // Player Movement
            Vector3 moveDirection = transform.forward * inputDir.y + transform.right * inputDir.x;
            moveDirection.Normalize();
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime); 
        }
    }

    private void LateUpdate()
    {
        if (playerHasControl && !death)
        {
            cameraPitch = Math.Clamp(cameraPitch, cameraPitchClamp.x, cameraPitchClamp.y);
            cameraArm.transform.localRotation = Quaternion.Euler(cameraPitch, -10f, 0);

            CreateAimPoint();
        }
    }

    #endregion

    #region Methods

    public void SetPlayerIsInControl(bool active)
    {
        playerHasControl = active;
        
    }

    public void TriggerDeath()
    {
        playerHasControl = false;

        int rnd = UnityEngine.Random.Range(0, 2);
        animator.SetTrigger("Death" + (rnd + 1));
        playerGUI.SetDeathPanel(true);
        fadeScreen.FadeOut();
        respawnStarted = true;
    }

    public void Respawn()
    {
        if (respawnStarted)
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer < 0)
            {
                death = false;
                playerHasControl = true;
                transform.position = spawnPoint;
                animator.Play("SpaceRanger_Rifle_Aim_Idle");
                playerGUI.SetDeathPanel(false);
                playerGUI.HideLavaMeter();
                fadeScreen.FadeIn();
                lavaTimer = 0;
                respawnStarted = false;
                respawnTimer = 5f;
            }
        }
    }

    private void CheckForLava()
    {
        if (playerHasControl)
        {
            LayerMask checkLayer = (1 << LayerMask.NameToLayer("PlanetGround") | 1 << LayerMask.NameToLayer("Lava"));
            bool isOnlava = false;

            Vector3 awayfromcenter = (transform.position - Vector3.zero).normalized;
            floorcheckRay.origin = transform.position + awayfromcenter * 5;
            floorcheckRay.direction = -awayfromcenter;

            if (Physics.Raycast(floorcheckRay, out floorcheckHit, 7f, checkLayer))
            {
                if (floorcheckHit.transform.gameObject.tag == "Lava")
                {
                    isOnlava = true;
                }
                else
                {
                    isOnlava = false;
                }
            }

            if (isOnlava)
            {
                lavaTimer += Time.deltaTime;
                if (lavaTimer > 2f && !death)
                {
                    death = true;
                    playerGUI.HideLavaMeter();
                    TriggerDeath();
                }

            }
            else
            {
                if (lavaTimer > 0)
                    lavaTimer -= Time.deltaTime;
                if (lavaTimer < 0)
                    lavaTimer = 0;
            }

            if (lavaTimer > 0 && !death)
                playerGUI.ShowLavaMeter(lavaTimer * 50);

            if (lavaTimer <= 0)
                playerGUI.HideLavaMeter();
        }
    }

    private void InitializePlayerController()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        playerGUI = GameObject.Find("PlayerGUI").GetComponent<GameGUI>();
        fadeScreen = GameObject.Find("FadeCanvas").GetComponent<FadeScreen>();
        cameraPitchClamp = new Vector2(-50f, 50f); // cameraArm X rotation clamp
        cameraPitch = 20f;
        mySpeed = runSpeed;
        playerCamera = Camera.main;
        aimTarget = new Vector3(0, 0, 0);
        spawnPoint = transform.position;

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
    private void CheckForGrounded()
    {
        // Ground for jumping animation
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

        // Reset player if under the map
        if (Vector3.Distance(transform.position, Vector3.zero) < 200f)
        {
            transform.position = transform.position + (transform.up * 50f);
        }
    }
    private void GetInput()
    {
        if (playerHasControl && !death)
        {
            // Crouching
            if (Input.GetKeyDown(KeyCode.LeftControl))
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

            // Flashlight
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashLight = !flashLight;
            }

            flashLightGO.SetActive(flashLight);

            // Rotate camera
            cameraPitch += Input.GetAxis("Mouse Y") * -GameManager.MouseSensitivity; // mouse Y input amount per frame (-1 to 1 in 0.05 steps)
            cameraYaw = Input.GetAxis("Mouse X") * GameManager.MouseSensitivity;

            // Movement
            inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rigidBody.AddForce(transform.up * 400f);
                animator.SetTrigger("jump");
            }
        }
        else
        {
            // For when the EscapeMenu gets opened
            inputDir = Vector3.zero;
            cameraYaw = 0;
        }
    }
    private void AnimationHandling()
    {
        if (playerHasControl && !death)
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
    }
    private void Shooting()
    {
        if (playerHasControl && !death)
        {
            // Very inefficient, CREATE SOMETING BETTER HERE
            if (Input.GetMouseButtonDown(0))
            {
                GameObject beam = Instantiate(laserBeamPrefab, gunEndPoint.position, Quaternion.identity);
                beam.transform.LookAt(aimTarget);
                shootTimer = 0;
            }

            if (Input.GetMouseButton(0))
            {
                shootTimer += Time.deltaTime;

                if (shootTimer > autoShootRate)
                {
                    GameObject beam = Instantiate(laserBeamPrefab, gunEndPoint.position, Quaternion.identity);
                    beam.transform.LookAt(aimTarget);
                    shootTimer = 0;
                }
            }
        }
    }
    private void CreateAimPoint()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        int ground = 1 << LayerMask.NameToLayer("PlanetGround") | LayerMask.NameToLayer("Lava");

        aimRotation = chest.transform.rotation * Quaternion.Euler(cameraPitch * 0.5f, 0, 0);
        chest.transform.rotation = aimRotation;

        // Lift / lower the rifle height based on aim height
        rifle.transform.localPosition = new Vector3(rifle.transform.localPosition.x, -cameraPitch / 400, rifle.transform.localPosition.z);

        if (Physics.Raycast(ray, out hit, maxDistance: 300f, ground))
        {
            aimTarget = hit.point;
        }
        else
        {
            aimTarget = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 150f));
        }
        rifle.transform.LookAt(aimTarget, transform.up);
    }
    private void Zoom()
    {
        if (playerHasControl && !death)
        {
            if (Input.GetMouseButton(1))
            {
                targetFOV = zoomFOV;
            }
            else
            {
                targetFOV = standardFOV;
            }

            float cameraFOV = Mathf.SmoothDamp(playerCamera.fieldOfView, targetFOV, ref currentFOV, 0.2f);
            playerCamera.fieldOfView = cameraFOV;
        }
    }

    #endregion
}
