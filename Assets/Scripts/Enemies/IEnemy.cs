using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Melee, Range, FlyingRange, Creature }
public enum EnemyStrength { Normal, Boss }
public enum AgentState { Idle, Attacking, Pathing, Warning, ReachPlayer }

public interface IEnemy
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }

    public void TakeDamage(float _damage);
    public void Die();
    public void SearchForPlayer();
    public void AttackPlayer();
    public void Flee();
}
