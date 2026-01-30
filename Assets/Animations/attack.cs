using UnityEngine;
using System.Collections;

public class PlayerDualAttack : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public MonoBehaviour movementScript;

    [Header("General Settings")]
    [Tooltip("If true, you can't start any other action while one attack is running.")]
    public bool lockActionsDuringAttack = true;

    [Tooltip("Disable movement during attack.")]
    public bool disableMovementDuringAttack = true;

    [Header("Q Attack Settings")]
    public GameObject qActiveObject;
    public float qAttackDuration = 0.6f;
    public float qCooldown = 0.8f; // <-- COOLDOWN (Inspector)
    public AudioSource qSound;

    [Header("R Attack Settings")]
    public GameObject rActiveObject;
    public float rAttackDuration = 0.6f;
    public float rCooldown = 0.8f; // <-- COOLDOWN (Inspector)
    public AudioSource rSound;

    // Internal state
    bool isAttacking = false;
    float nextQTime = 0f;
    float nextRTime = 0f;

    Coroutine currentRoutine;

    void Reset()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        bool isInAir = animator.GetBool("air");
        bool isCrouching = animator.GetBool("crouch");

        // Only allow when grounded and not crouching
        if (isInAir || isCrouching) return;

        // Optional: lock all actions while attacking
        if (lockActionsDuringAttack && isAttacking) return;

        // Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time < nextQTime) return;     // cooldown gate
            StartAttack(
                qActiveObject,
                qAttackDuration,
                qCooldown,
                ref nextQTime,
                qSound
            );
        }

        // R
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Time.time < nextRTime) return;     // cooldown gate
            StartAttack(
                rActiveObject,
                rAttackDuration,
                rCooldown,
                ref nextRTime,
                rSound
            );
        }
    }

    void StartAttack(GameObject obj, float duration, float cooldown, ref float nextTime, AudioSource sound)
    {
        // If an attack is running and you still want to allow overriding, stop it.
        // But default behavior: don't stack.
        if (isAttacking) return;

        nextTime = Time.time + cooldown;

        if (sound != null) sound.Play();

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(PerformAttack(obj, duration));
    }

    IEnumerator PerformAttack(GameObject obj, float duration)
    {
        isAttacking = true;

        // Set animator state
        animator.SetBool("attack", true);

        // Disable movement (optional)
        if (disableMovementDuringAttack && movementScript != null)
            movementScript.enabled = false;

        // Enable hitbox / shield / etc
        if (obj != null) obj.SetActive(true);

        // Wait for attack duration
        yield return new WaitForSeconds(duration);

        // End attack
        animator.SetBool("attack", false);

        if (obj != null) obj.SetActive(false);

        if (disableMovementDuringAttack && movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
        currentRoutine = null;
    }
}
