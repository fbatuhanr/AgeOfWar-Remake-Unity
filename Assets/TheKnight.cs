using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheKnight : MonoBehaviour
{
    [SerializeField] [Range(1,10)] private int characterHealth = 3;
    [SerializeField] [Range(1, 3)] private float characterSpeed = 1f;

    private Vector2 characterMovementDirection;
    
    private SpriteRenderer sprRender;
    [SerializeField] private Sprite[] idleSprites, walkSprites, attackSprites;

    private int spriteCounter = 0;
    private float spriteDelay = 0;
    
    private enum CharacterStates
    {
        idle=0,
        walk=1,
        attack=2
    }

    private CharacterStates characterState = CharacterStates.idle;
    
    private void Start()
    {
        characterMovementDirection = transform.CompareTag("friend") ? Vector2.right : Vector2.left;

        sprRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        RayResult(DrawRay());
    }

    private RaycastHit2D DrawRay()
    {
        
        Debug.DrawRay(transform.position, transform.TransformDirection(characterMovementDirection)*30f, Color.blue);

        RaycastHit2D hit = 
            Physics2D.Raycast(
                transform.position, 
                transform.TransformDirection(characterMovementDirection), 
                30f);
        
        return hit;
    }

    private void RayResult(RaycastHit2D hit)
    {
        if (hit.collider)
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
                CharacterAnimation(attackSprites);
                break;
            
            default: CharacterAnimation(idleSprites); break;
        }
    }


    private void CharacterMovement()
    {
        transform.Translate(characterMovementDirection * characterSpeed * Time.fixedDeltaTime);
    }
    
    private void CharacterAnimation(Sprite[] animSprites)
    {
        spriteDelay += Time.fixedDeltaTime;
        if (spriteDelay >= 0.05f)
        {
            sprRender.sprite = animSprites[spriteCounter++];
            if (spriteCounter == animSprites.Length) spriteCounter = 0;

            spriteDelay = 0;
        }
    }
    
    
}
