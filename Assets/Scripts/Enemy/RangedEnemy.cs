using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(attackSound);
        cooldownTimer = 0;

        //Shoot projectile
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                //Attack
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }

        //If the player is in sight, stop patrolling and attack
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
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

        return hit.collider != null;
    }
}
