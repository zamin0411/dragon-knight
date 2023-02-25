using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                //Attack
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                SoundManager.instance.PlaySound(attackSound);
            }
        }

        //If the player is in sight, stop patrolling and attack
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    public override bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.
            BoxCast
            (
                boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, //Make the box change direction when player flip - also makes the box not too far away with colliderDistance
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), //Change the size of the box
                0,
                Vector2.left,
                0,
                playerLayer
            );

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.
            DrawWireCube
            (
                boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
            );
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            //Damage player health
            playerHealth.TakeDamage(damage);
        }
    }
}
