using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;   // Player movement speed
    public float sprintSpeed = 8f;   // Player sprinting speed
    public float staminaMax = 100f;  // Maximum stamina
    public float staminaConsumptionRate = 10f;  // Rate at which stamina is consumed while sprinting
    public float staminaRecoveryRate = 5f;      // Rate at which stamina recovers when not sprinting
    public float staminaCooldownDuration = 2f;  // Cooldown duration before stamina starts regenerating

    public Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float currentStamina;   // Current stamina level
    private bool isSprinting;
    private bool isCooldownActive;      // Indicates whether the stamina cooldown is active
    private float cooldownTimer;        // Timer for the stamina cooldown


    public Transform attackPoint;   // Reference to the attack point game object
    public float attackRange = 0.5f;   // Range of the attack
    public LayerMask enemyLayer;   // Layer mask for the enemy
    bool attack = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentStamina = staminaMax;
        isSprinting = false;
        isCooldownActive = false;
        cooldownTimer = 0f;
    }
    void Update()
    {
        // Check for attack input
        if (Input.GetKey(KeyCode.Space))
        {
            attack = true;
            animator.SetBool("IsAttacking",true);
            //Attack();
        }

        
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        // Calculate the speed based on sprinting state
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        // Calculate the new position
        Vector2 newPosition = rb.position + new Vector2(moveHorizontal * currentSpeed * Time.fixedDeltaTime, 0f);

        // Move the player to the new position
        rb.MovePosition(newPosition);

        // Flip the sprite if moving in a different direction
        if (moveHorizontal < 0f)
        {
            spriteRenderer.flipX = true;    // Flip the sprite horizontally
        }
        else if (moveHorizontal > 0f)
        {
            spriteRenderer.flipX = false;   // Do not flip the sprite
        }

        // Fix the rotation to ensure the sprite stays upright
        transform.rotation = Quaternion.identity;

        // Manage sprinting and stamina
        HandleSprinting();

        // Stamina consumption while sprinting
        if (isSprinting && currentStamina > 0f)
        {
            currentStamina -= staminaConsumptionRate * Time.fixedDeltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, staminaMax);
        }
        else if (!isSprinting && currentStamina < staminaMax)
        {
            // Stamina recovery when not sprinting
            if (!isCooldownActive)
            {
                currentStamina += staminaRecoveryRate * Time.fixedDeltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, staminaMax);
            }
        }

        // Stamina cooldown
        if (isCooldownActive)
        {
            cooldownTimer += Time.fixedDeltaTime;

            if (cooldownTimer >= staminaCooldownDuration)
            {
                isCooldownActive = false;
            }
        }
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("currentStamina:" + cooldownTimer);

            if (currentStamina > 0f)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
                return;
                
            }
        }
        else
        {
            isSprinting = false;
            isCooldownActive = true;
            cooldownTimer = 0f;
        }
    }

    void Attack()
    {
        // Perform the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage or destroy the enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            // Add your attack logic here
            // For example, you can call a method on the enemy's script to damage or destroy it
            //enemy.GetComponent<EnemyScript>().TakeDamage();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}