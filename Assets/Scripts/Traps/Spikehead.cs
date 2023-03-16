using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range; //How far the spike head will be able to see
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;

    private Vector3 destination; //When spike detects player, store the player's position

    private bool attacking; //To tell if the spike head is attacking the player or not

    private Vector3[] directions = new Vector3[4];

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private void OnEnable()
    {
        Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            transform.Translate(destination * Time.deltaTime * speed);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirection();

        //Check if spikehead sees the player in all 4 directions
        for (int i = 0; i < directions.Length; i++)
        {
            //Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirection()
    {
        directions[0] = transform.right * range; //The right direction
        directions[1] = -transform.right * range; //The left direction
        directions[2] = transform.up * range; //The up direction
        directions[3] = -transform.up * range; //The down direction
    }

    private void Stop()
    {
        destination = transform.position; //Set destination as the current position so it doesn't move
        attacking = false;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.gameObject.layer == 6 || collision.tag == "Door" || collision.tag == "Wall")
        {
            SoundManager.instance.PlaySound(impactSound);
            base.OnTriggerEnter2D(collision);
            //Stop spikehead once it hits something
            Stop();
        }
    }
}
