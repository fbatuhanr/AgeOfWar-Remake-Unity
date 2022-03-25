using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TheKnight : MonoBehaviour
{
    // Character Specialties
    [SerializeField] [Range(1,10)] private int health = 3;
    private int currentHealth;
    
    [SerializeField] [Range(1, 5)] private float movementSpeed = 1f, attackSpeed = 1f;
    private float defaultAnimSpeed, movementAnimSpeed, attackAnimSpeed;

    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 0.5f;

    [SerializeField] private float rayDistance = 45f;
    [SerializeField] private LayerMask[] baseMasks;
    private LayerMask selectedRayIgnoreMask;
    
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

        if (transform.CompareTag("friend")) {
            movementDirection = Vector2.right;
            selectedRayIgnoreMask = baseMasks[0];
        }
        else {
            movementDirection = Vector2.left;
            selectedRayIgnoreMask = baseMasks[1];
        }


        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultAnimSpeed = 0.07f; // 0.01 faster, 0.1 slower animation
        movementAnimSpeed = 1/movementSpeed * defaultAnimSpeed;
        attackAnimSpeed = 1/attackSpeed * defaultAnimSpeed;
        
        attackSpeed = 1/attackSpeed * 0.5f;
    }

    private void Update()
    {
        RayResult(DrawRay());
    }

    private RaycastHit2D DrawRay()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(movementDirection)*rayDistance, Color.blue);

        RaycastHit2D hit = 
            Physics2D.Raycast(
                transform.position, 
                transform.TransformDirection(movementDirection), 
                rayDistance,
                ~selectedRayIgnoreMask);
        
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
                CharacterAnimation(idleSprites, defaultAnimSpeed); 
                break;
            case CharacterStates.walk:
                CharacterMovement();
                CharacterAnimation(walkSprites, movementAnimSpeed);
                break;
            case CharacterStates.attack:
                HitOverlap();
                CharacterAnimation(attackSprites, attackAnimSpeed);
                break;
            case CharacterStates.dead:
                Invoke("Die",1f);
                CharacterAnimation(deadSprites, defaultAnimSpeed, false);
                break;
            
            default: CharacterAnimation(idleSprites, defaultAnimSpeed); break;
        }
    }

    private float hitDelay=0;
    private void HitOverlap()
    {
        hitDelay += Time.fixedDeltaTime;
        if (hitDelay >= attackSpeed)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (!enemy.CompareTag(transform.tag))
                {
                    // Debug.Log("We hit enemy! " + enemy.name);

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

    private void CharacterAnimation(Sprite[] animSprites, float animSpeed, bool animLoop=true)
    {
        spriteDelay += Time.fixedDeltaTime;
        if (spriteDelay >= animSpeed)
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
        healthBar.fillAmount = (float) currentHealth / (float) health;
        
        if(currentHealth <= 0) {
            
            GetComponent<Collider2D>().enabled = false;
            spriteRenderer.sortingOrder = -1;
            characterState = CharacterStates.dead;
            
            if (transform.CompareTag("friend"))
                GameObject.FindWithTag("enemyBase").GetComponent<EnemyBase>().gold += 50;
            else
                GameObject.FindWithTag("friendBase").GetComponent<PlayerBase>().gold += 50;
        }
    }

    private float deathOpacity = 1f;
    private void Die()
    {
        deathOpacity -= Time.fixedDeltaTime*0.5f;
        if (deathOpacity > 0)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, deathOpacity);
            if (deathOpacity < 0.5f)
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, -3f), Time.fixedDeltaTime*1.5f);
        }
        else {
            Destroy(gameObject);
        }
    }
}