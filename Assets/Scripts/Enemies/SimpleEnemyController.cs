using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EnemyState { Idle, Pathing, BackToSpawn, Chase, Attack, Death, Hit }

public class SimpleEnemyController : MonoBehaviour
{
    private float currentHealth = 100f;

    private EnemyState enemyState;
    private Animator animator;

    private Rigidbody rigidBody;
    private Vector3 spawnPoint;
    private Vector3 nextWaypointDirection;
    private Vector3 nextWaypointPosition;
    private int wayPointCounter = 0;
    private float idleTimer = 0;
    private float deathTimer = 3f;
    private float pathTimer = 0;
    public float turnSpeed = 300f;

    private float runSpeed = 3;
    private float currentSpeed = 0;
    private Vector3 velocity;
    private float aggroRange = 10f;
    
    public bool hasSeenPlayer = true;

    private GameObject playerObject;

    private bool active = false;
    private bool dead = false;

    private float detectionTimer = 0.5f;
    private float attackTimer = 1f;
    private float distanceToPlayer;

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
        spawnPoint = transform.position + new Vector3(0, 1, 0);
        rigidBody = GetComponent<Rigidbody>();
        spawnHelper = GameObject.Find("QuestManager").GetComponent<SpawnHelper>();
    }

    void Update()
    {
        if (active)
        {
            PlayerDetection();
            StateSwitch();
        }
    }

    private void PlayerDetection()
    {
        detectionTimer -= Time.deltaTime;

        if (detectionTimer < 0f && !dead)
        {
            distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

            // If player is near enemy, chase
            if (distanceToPlayer < aggroRange && enemyState != EnemyState.Attack)
            {
                enemyState = EnemyState.Chase;
            }

            // Go back to pathing if player is far enough
            if (enemyState == EnemyState.Chase && distanceToPlayer > aggroRange * 2)
            {
                enemyState = EnemyState.Pathing;
            }

            if (enemyState == EnemyState.Attack && distanceToPlayer > 1f)
            {
                enemyState = EnemyState.Chase;
            }

            detectionTimer = 0.5f;
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
                Pathing();
                break;
            case EnemyState.BackToSpawn:
                BackToSpawn();
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

    private void BackToSpawn()
    {
        if (!dead)
        {
            currentSpeed = runSpeed * 1.5f;
            Vector3 targetDirection = (spawnPoint - transform.position).normalized;
            velocity = transform.forward * currentSpeed;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * turnSpeed);

            rigidBody.MovePosition(rigidBody.position + targetDirection * currentSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, spawnPoint) < 1f)
            {
                enemyState = EnemyState.Pathing;
                currentSpeed = runSpeed;
            }
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

    private void Pathing()
    {
        pathTimer += Time.deltaTime;
        Vector3 targetDirection = nextWaypointDirection;

        if (Vector3.Distance(transform.position, spawnPoint) > 40f)
        {
            enemyState = EnemyState.BackToSpawn;
        }
        else
        {
            if (nextWaypointDirection != null)
            {
                if (pathTimer > 5f || Vector3.Distance(transform.position, nextWaypointPosition) < 1f)
                {
                    enemyState = EnemyState.Idle;
                    animator.SetBool("Running", false);
                    pathTimer = UnityEngine.Random.Range(-3f, 1);
                    currentSpeed = 0;
                }
                else
                {
                    animator.SetBool("Running", true);
                    velocity = transform.forward * currentSpeed;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * turnSpeed);
                    currentSpeed = runSpeed;
                }

                
            }
        }

        rigidBody.MovePosition(rigidBody.position + targetDirection * currentSpeed * Time.deltaTime);
    }

    private void ChaseState()
    {
        if (!dead)
        {
            if (Vector3.Distance(transform.position, spawnPoint) > 40f)
            {
                enemyState = EnemyState.BackToSpawn;
            }
            else
            {
                animator.SetBool("Running", true);
                currentSpeed = runSpeed;
                Vector3 targetDirection = ((playerObject.transform.position + new Vector3(0, 1f, 0)) - transform.position).normalized;
                velocity = transform.forward * currentSpeed;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * turnSpeed);
                currentSpeed = runSpeed;

                if (Vector3.Distance(transform.position, playerObject.transform.position) < 1f)
                {
                    enemyState = EnemyState.Attack;
                    attackTimer = 0.1f;

                    animator.SetBool("Running", false);
                }

                rigidBody.MovePosition(rigidBody.position + targetDirection * currentSpeed * Time.deltaTime);
            }
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
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            animator.SetTrigger("Attack");
            attackTimer = 2f;
        }
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

        if (wayPointCounter >= 3)
        {
            wayPointCounter = 0;
            currentSpeed *= 1.5f;
            output = spawnPoint;
            pathTimer -= 3f;
        }

        nextWaypointPosition = transform.position + (output * 4f);
        wayPointCounter++;

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
