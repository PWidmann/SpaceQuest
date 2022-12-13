using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IEnemy
{
    private CharacterController characterController;   
    private Animator animator;
    private AgentState agentState;
    private Vector3 targetWalkPosition;
    private Vector3 playerPosition;

    private float idleTimer = 0;
    private float walkTimer = 0;


    public float Health { get; set; }
    public float MaxHealth { get; set; }

    void Start()
    {
        agentState = AgentState.Idle;
    }


    void Update()
    {
        switch (agentState)
        {
            case AgentState.Idle:
                IdleState();
                break;
            case AgentState.Pathing:
                Pathing();
                break;
            case AgentState.Warning:
                break;
            case AgentState.ReachPlayer:
                break;
            case AgentState.Attacking:
                break;
        }
    }

    private void IdleState()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= 3f)
        {
            agentState = AgentState.Pathing;
            idleTimer = 0f;
        }
    }

    private void Pathing()
    {
        walkTimer += Time.deltaTime;

        if (targetWalkPosition == null)
        {
            SearchWayPoint();
        }
    }

    private void SearchWayPoint()
    {

    }


    public void AttackPlayer()
    {
        
    }

    public void Die()
    {
        Debug.Log("Creature has died");
    }

    public void Flee()
    {
        
    }

    public void SearchForPlayer()
    {
        
    }

    public void TakeDamage(float _damage)
    {
        Health -= _damage;
        Debug.Log("Creature has taken " + _damage + " and is now at " + Health + " health");
        if (Health <= 0)
        {
            Die();
        }
    }
}
