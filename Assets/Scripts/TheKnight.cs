using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheKnight : MonoBehaviour
{
    // Character Specialties
    [SerializeField] [Range(1,10)] private int health = 3;
    [SerializeField] [Range(1, 3)] private float movementSpeed = 1f;

    private int currentHealth;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 0.5f;
    
    private Transform attackPoint; // Character attack point reference object, it assigns from child when the game has started.
    
    // Character Health UI
    private Image healthBar;
    
    // Character Physic2D Ray & Transform System
    private Vector2 movementDirection;
    
    // Character Animation
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] idleSprites, walkSprites, attackSprites, deadSprites;
    private int spriteCounter = 0;
    private float spriteDelay = 0;
    
    private enum CharacterStates { idle, walk, attack, dead }

    private CharacterStates characterState = CharacterStates.idle;
    
    private void Start()
    {
        currentHealth = health;

        healthBar = transform.Find("Canvas/HealthBar").GetComponent<Image>();
        
        
        attackPoint = transform.Find("AttackPoint");
        movementDirection = transform.CompareTag("friend") ? Vector2.right : Vector2.left;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        RayResult(DrawRay());
    }

    private RaycastHit2D DrawRay()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(movementDirection)*45f, Color.blue);

        RaycastHit2D hit = 
            Physics2D.Raycast(
                transform.position, 
                transform.TransformDirection(movementDirection), 
                45f);
        
        return hit;
    }

    private void RayResult(RaycastHit2D hit)
    {
        if (hit.collider && GetComponent<Collider2D>().enabled)
        {
            // Debug.Log("Hit something!: " + hit.transform.tag + " Range: " + hit.distance);

            if (hit.distance >= 0.75f) // 0.75 is a distance between friend & enemy
            {
                characterState = CharacterStates.walk;
            }
            else
            {
                characterState = 
                    hit.transform.CompareTag( transform.tag )  // used 'transform.tag' instead of "friend" because current character can be 'friend' or 'enemy'!
                        ? CharacterStates.idle 
                        : CharacterStates.attack;
            }
        }
    }
    private void FixedUpdate()
    { 
        switch (characterState)
        {
            case CharacterStates.idle:
                CharacterAnimation(idleSprites); 
                break;
            case CharacterStates.walk:
                CharacterMovement();
                CharacterAnimation(walkSprites);
                break;
            case CharacterStates.attack:
                HitOverlap();
                CharacterAnimation(attackSprites);
                break;
            case CharacterStates.dead:
                Die();
                CharacterAnimation(deadSprites, false);
                break;
            
            default: CharacterAnimation(idleSprites); break;
        }
    }

    private float hitDelay=0;
    private void HitOverlap()
    {
        hitDelay += Time.fixedDeltaTime;
        if (hitDelay >= 0.5f)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (!enemy.CompareTag(transform.tag))
                {
                    Debug.Log("We hit enemy! " + enemy.name);

                    //MonoBehaviour ff = enemy.GetComponent<MonoBehaviour>();
                    //enemy.GetComponent<ff.GetType().Name>().TakeDamage(attackDamage);
                    
                    enemy.SendMessage("TakeDamage", attackDamage);

                    break;
                }
            }

            hitDelay = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void CharacterMovement()
    {
        transform.Translate(movementDirection * movementSpeed * Time.fixedDeltaTime);
    }
    
    private void CharacterAnimation(Sprite[] animSprites, bool animLoop=true)
    {
        spriteDelay += Time.fixedDeltaTime;
        if (spriteDelay >= 0.05f)
        {
            spriteRenderer.sprite = animSprites[spriteCounter];
            if (animSprites.Length > spriteCounter+1)
                spriteCounter++;
            else if(animLoop)
                spriteCounter = 0;

            spriteDelay = 0;
        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / (float)health;

        if (currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            characterState = CharacterStates.dead;
        }
    }

    private float opacity = 1f;
    private void Die()
    {
        opacity -= Time.fixedDeltaTime;
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        
        
        transform.position = 
            Vector2.Lerp(
                transform.position, 
                new Vector2(transform.position.x, -3f), 
                Time.fixedDeltaTime);
    }
}
