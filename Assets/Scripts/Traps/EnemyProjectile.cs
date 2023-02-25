using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage //Will damage the player everytime they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return; //Stop the projectile from moving any further
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent
        boxCollider.enabled = false; //Cannot hurt the player after exploding

        //Because both enemy arrow and fireball both have this script attached
        if (anim != null)
        {
            anim.SetTrigger("explode"); //When the object is a fireball, explode
        }
        else
        {
            gameObject.SetActive(false); //When this hit any object, deactivate arrow
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
