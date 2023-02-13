using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private CharacterController characterController;   
    private Animator animator;
    private Vector3 targetWalkPosition;
    private Vector3 playerPosition;

    private float idleTimer = 0;
    private float walkTimer = 0;


    public float Health { get; set; }
    public float MaxHealth { get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();
        Health = 100f;
    }


    void Update()
    {

    }

    private void IdleState()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= 3f)
        {

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
        animator.SetTrigger("Die");
        Debug.Log("Creature has died");
        Destroy(transform.GetComponent<Creature>());
    }

    public void Flee()
    {
        
    }

    public void SearchForPlayer()
    {
        
    }

    public void TakeDamage(float _damage)
    {
        
    }
}
