using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum EnemyState { Idle, Pathing, Chase, Attack, Death, Hit }

public class SimpleEnemyController : MonoBehaviour
{
    [SerializeField] private int healthPoints = 100;

    private EnemyState enemyState;
    private Animator animator;

    private Rigidbody rigidBody;
    private Vector3 spawnPoint;
    private Vector3 nextWaypoint;
    private int wayPointCounter = 0;
    private float idleTimer = 0;
    private float moveSpeed = 4f;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyState = EnemyState.Idle;
        nextWaypoint = NextPathPoint();
        spawnPoint = transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Pathing:
                PathingState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Death:
                DeathState();
                break;
            case EnemyState.Hit:
                HitState();
                break;
        }
    }

    private void IdleState()
    {
        animator.SetBool("Running", false);
        idleTimer += Time.deltaTime;

        if (idleTimer >= 3f)
        {
            enemyState = EnemyState.Pathing;
            idleTimer = 0f;
        }
    }

    private void PathingState()
    {
        animator.SetBool("Running", true);
        nextWaypoint = NextPathPoint();

    }

    private void HitState()
    {
        animator.SetTrigger("Hit");
    }

    private void DeathState()
    {
        animator.SetTrigger("Death");
    }

    private void AttackState()
    {
        animator.SetTrigger("Attack");
    }

    private void ChaseState()
    {
        animator.SetBool("Running", true);
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * 1 + transform.right * 1;
        moveDirection.Normalize();
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private Vector3 NextPathPoint()
    {
        Vector3 output = Vector3.zero;

        output = transform.position;

        int rnd = UnityEngine.Random.Range(0, 4);
        switch (rnd)
        {
            case 0:
                output += new Vector3(0, 0, 5f);
                break;
            case 1:
                output += new Vector3(-5f, 0, 0);
                break;
            case 2:
                output += new Vector3(0, 0, -5f);
                break;
            case 3:
                output += new Vector3(5f, 0, 0);
                break;
        }

        output = GetGroundPoint(output);


        wayPointCounter++;
        return output;
    }

    private Vector3 GetGroundPoint(Vector3 input)
    {
        Vector3 output = input;

        LayerMask checkLayer = (1 << LayerMask.NameToLayer("PlanetGround") | 1 << LayerMask.NameToLayer("Lava"));
        Ray floorcheckRay = new Ray();
        RaycastHit floorcheckHit;
        Vector3 awayfromcenter = (transform.position - Vector3.zero).normalized;
        floorcheckRay.origin = transform.position + awayfromcenter * 5;
        floorcheckRay.direction = -awayfromcenter;

        if (Physics.Raycast(floorcheckRay, out floorcheckHit, 7f, checkLayer))
        {
            if (floorcheckHit.transform.gameObject.tag == "PlanetGround")
            {
                output = floorcheckHit.point;
            }
            else
            {
                Debug.Log("No Ground found for waypoint");
                output = NextPathPoint();
            }
        }

        return output;
    }
}
