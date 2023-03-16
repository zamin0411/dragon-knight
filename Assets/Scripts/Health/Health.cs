using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float startingHealth;
    public float currentHealth { get; protected set; }
    protected Animator anim;
    protected bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] protected Behaviour[] components;
    public bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;



    protected void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        //Invulnerable after respawn
        StartCoroutine(Invulnerability());

        //Activate all attached component classes
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }

    public virtual void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        //So the health can't go below 0
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            
            
            //Player hurt
            if (gameObject.layer == 8)
            {
                anim.SetTrigger("hurt");
                SoundManager.instance.PlaySound(hurtSound);
                StartCoroutine(Invulnerability());      
            }
            //iframes
            
            
        }
        else
        {
            if (!dead)
            {
                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }
                foreach (AnimatorControllerParameter param in anim.parameters)
                {
                    if (param.name == "grounded")
                        anim.SetBool("grounded", true);
                }

                anim.SetTrigger("die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }

        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    protected IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        //Flash red when hurt and ignore damage for a bit of time
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
