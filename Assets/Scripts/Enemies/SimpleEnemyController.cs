using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EnemyState { Idle, Pathing, Chase, Attack, Death, Hit }

public class SimpleEnemyController : MonoBehaviour
{
    private float currentHealth = 100f;
    private float maxHealth = 100f;

    private EnemyState enemyState;
    private Animator animator;

    private Rigidbody rigidBody;
    private Vector3 spawnPoint;
    private Vector3 nextWaypointDirection;
    private int wayPointCounter = 0;
    private float idleTimer = 0;
    private float deathTimer = 3f;
    private float pathTimer = 0;
    public float turnSpeed = 300f;

    public float runSpeed = 4;
    private float currentSpeed = 0;
    public Vector3 velocity;

    public bool hasSeenPlayer = true;

    private GameObject playerObject;

    private bool active = false;
    private bool dead = false;

    private SpawnHelper spawnHelper;
    void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        animator = GetComponent<Animator>();
        enemyState = EnemyState.Idle;
        nextWaypointDirection = Vector3.zero;
        spawnPoint = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        spawnHelper = GameObject.Find("QuestManager").GetComponent<SpawnHelper>();
    }

    void Update()
    {
        if (active)
        {
            StateSwitch();
        }
    }

    private void StateSwitch()
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
            nextWaypointDirection = (NextPathPoint() - transform.position).normalized;
        }
    }

    private void PathingState()
    {
        pathTimer += Time.deltaTime;

        if (pathTimer > 5f)
        {
            enemyState = EnemyState.Idle;
            animator.SetBool("Running", false);
            pathTimer = 0;
            currentSpeed = 2f;
        }
        else
        {
            animator.SetBool("Running", true);
            velocity = transform.forward * currentSpeed;

            Vector3 targetDirection = nextWaypointDirection;
            targetDirection.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * turnSpeed);
            currentSpeed = runSpeed;

            rigidBody.MovePosition(rigidBody.position + targetDirection * currentSpeed * Time.deltaTime);
        }
        
    }

    private void HitState()
    {
        animator.SetTrigger("Hit");
    }

    private void DeathState()
    {
        deathTimer -= Time.deltaTime;

        if (deathTimer < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void AttackState()
    {
        animator.SetTrigger("Attack");
    }

    private void ChaseState()
    {
        animator.SetBool("Running", true);
    }

    private Vector3 NextPathPoint()
    {
        Vector3 output = Vector3.zero;

        output = transform.position;

        int rnd = UnityEngine.Random.Range(0, 4);
        switch (rnd)
        {
            case 0:
                output += transform.right * 5;

                break;
            case 1:
                output += -transform.right * 5;
                break;
            case 2:
                output += transform.forward * 5;
                break;
            case 3:
                output += -transform.forward * 5;
                break;
        }
        wayPointCounter++;

        if (wayPointCounter >= 3)
        {
            wayPointCounter = 0;
            currentSpeed *= 1.5f;
            output = spawnPoint;
        }

        return output;
    }

    public void ActivateNPC(GameObject player)
    {
        playerObject = player;
        spawnPoint = transform.position;
        currentHealth = 100f;
        
        active = true;
    }

    public void TakeDamage(float damage)
    {
        if (dead == false)
        {
            animator.SetTrigger("Hit");

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                enemyState = EnemyState.Death;
                animator.SetTrigger("Death");
                dead = true;
                Debug.Log("Enemy died");
            }
        }
    }
}
