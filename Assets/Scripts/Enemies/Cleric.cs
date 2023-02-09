using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyType EnemyType;
    [SerializeField] private EnemyStrength EnemyStrength;

    public float Health { get; set; }
    public float MaxHealth { get; set; }


    //private float MovementSpeed;
    //private AgentState AgentState;
    //private float AggroRange;
    private Vector3 playerPos;
    private float attackRange = 10f;

    void Start()
    {
        InitializeEnemy();
    }

    void Update()
    {
        
    }

    private void InitializeEnemy()
    {
        // Set health
        switch (EnemyStrength)
        {
            case EnemyStrength.Normal:
                Health = 100f;
                MaxHealth = 100f;
                break;
            case EnemyStrength.Boss:
                Health = 300f;
                MaxHealth = 300f;
                break;
        }

        //AggroRange = 20f;

        // Set enemy based on tape
        switch (EnemyType)
        {
            case EnemyType.Melee:
                //MovementSpeed = 3f;
                break;
            case EnemyType.Range:
                //MovementSpeed = 5f;
                break;
            case EnemyType.FlyingRange:
                //MovementSpeed = 5f;
                break;
            case EnemyType.Creature:
                //MovementSpeed = 3f;
                break;
        }

        // Check for player pos
        SearchForPlayer();

        //AgentState = AgentState.Idle;
    }

    public void TakeDamage(float _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Die();
        }

        // To Do Implement hit animation
    }

    public void SearchForPlayer()
    {
        GameObject player;

        if (player = GameObject.FindGameObjectWithTag("Player"))
        {
            playerPos = player.transform.position;
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(playerPos, transform.position) < attackRange)
        { 
            
        }
    }

    public void Flee()
    { 
    
    }

    public void Die()
    {
        //Rip
        //Destroy(gameObject);
    }
}
