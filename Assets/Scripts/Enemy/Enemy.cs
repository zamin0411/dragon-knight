using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Attack Parameters")]
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float range;
    [SerializeField] protected int damage;

    [Header("Collider Parameters")]
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected float colliderDistance;


    [Header("Player Layer")]
    [SerializeField] protected LayerMask playerLayer;
    protected float cooldownTimer = Mathf.Infinity; //Gives the enemy the ability to attack right away

    [Header("Attack Sound")]
    [SerializeField] protected AudioClip attackSound;

    //References
    protected Animator anim;
    protected Health playerHealth;
    protected EnemyPatrol enemyPatrol;

    public abstract bool PlayerInSight();
    public abstract void OnDrawGizmos();
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
}
