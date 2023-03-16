using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [SerializeField] private Transform heartPoint; //The position from which the heart drops
    private int hitsToDropHeart = 4;
    private int hits;
    [SerializeField] private GameObject[] hearts;
    private bool bossDead = false;

    public override void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        //So the health can't go below 0
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (hits >= hitsToDropHeart)
            {
                hearts[FindHeart()].transform.position = heartPoint.transform.position;
                hearts[FindHeart()].SetActive(true);
                hits = 0;    
            }
            hits++;
            if (currentHealth <= startingHealth / 2)
            {
                anim.SetBool("IsEnraged", true);
            }
        }
        else
        {
            if (!bossDead)
            {
                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }

                anim.SetTrigger("die");

                bossDead = true;
                SoundManager.instance.PlaySound(deathSound);
                Destroy(gameObject.GetComponent<Boss>().Gate);
                
            }

        }
        
    }

    private int FindHeart()
    {
        //If a fireball is not active, take it and use it
        for (int i = 0; i < hearts.Length; i++)
        {
            if (!hearts[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
