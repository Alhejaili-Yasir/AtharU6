using UnityEngine;

/*
    Improved ThirdPersonController:
    - Uses a reliable grounded check (SphereCast) instead of cc.isGrounded
    - Adds LayerMasks for Ground and HeadHit
    - Prevents "stuck in jump animation" caused by bad CharacterController sizing/center
*/

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed at which the character moves (not affected by gravity or jumping).")]
    public float velocity = 5f;

    [Tooltip("This value is added to speed while sprinting.")]
    public float sprintAdittion = 3.5f;

    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;

    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;

    [Space]
    [Tooltip("Force that pulls the player down. Changing this affects all movement, jumping and falling.")]
    public float gravity = 9.8f;

    [Header("Ground Check (Fix for stuck jump)")]
    [Tooltip("Which layers count as ground.")]
    public LayerMask groundMask = ~0;

    [Tooltip("Extra distance below the CharacterController to check for ground.")]
    public float groundCheckDistance = 0.15f;

    [Tooltip("Sphere radius used for ground check (usually close to controller radius).")]
    public float groundCheckRadius = 0.25f;

    [Tooltip("Small downward force when grounded to keep character snapped to ground.")]
    public float groundedStickForce = 2.0f;

    [Header("Head Hit Detect")]
    [Tooltip("Layers that can stop the jump when hitting the head.")]
    public LayerMask headHitMask = ~0;

    [Tooltip("Extra multiplier for head hit distance.")]
    public float headHitDistanceMultiplier = 1.1f;

    [Header("Debug")]
    public bool showGroundCheckGizmos = true;

    float jumpElapsedTime = 0f;

    // Player states
    bool isJumping = false;
    bool isSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    // Components
    Animator animator;
    CharacterController cc;

    // Cached grounded state
    bool isGrounded;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cc == null)
            Debug.LogError("CharacterController missing on player object.");

        if (animator == null)
            Debug.LogWarning("Animator missing on player object. Animations won't work.");
    }

    void Update()
    {
        // Reliable grounded check (do this early each frame)
        isGrounded = CheckGrounded();

        // Input
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        if (inputCrouch)
            isCrouching = !isCrouching;

        // Animations (only if animator exists)
        if (animator != null)
        {
            // Crouch
            animator.SetBool("crouch", isCrouching);

            // Run / Sprint based on controller velocity magnitude
            float minimumSpeed = 0.9f;
            bool running = cc.velocity.magnitude > minimumSpeed;
            animator.SetBool("run", running);

            isSprinting = running && inputSprint;
            animator.SetBool("sprint", isSprinting);

            // Air (jump/fall) state
            animator.SetBool("air", !isGrounded);
        }

        // Jump input
        if (inputJump && isGrounded)
        {
            isJumping = true;
            // If you want crouch canceled on jump, uncomment:
            // isCrouching = false;
        }

        HeadHittingDetect();
    }

    // CharacterController movement should be in Update (not FixedUpdate) for best results
    void UpdateMovement()
    {
        // Sprinting boost / crouching slowdown
        float velocityAdittion = 0f;
        if (isSprinting) velocityAdittion = sprintAdittion;
        if (isCrouching) velocityAdittion = -(velocity * 0.50f);

        float moveSpeed = (velocity + velocityAdittion);

        // Horizontal input movement (local camera-based)
        float directionX = inputHorizontal * moveSpeed;
        float directionZ = inputVertical * moveSpeed;

        // Vertical (Y)
        float directionY = 0f;

        // Jump handler
        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime);

            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0f;
            }
        }

        // Gravity
        directionY -= gravity;

        // If grounded and not actively jumping, add a small downward force to stay grounded
        if (isGrounded && !isJumping)
        {
            directionY = -groundedStickForce;
        }

        // Camera relative movement
        Vector3 camForward = Camera.main ? Camera.main.transform.forward : Vector3.forward;
        Vector3 camRight = Camera.main ? Camera.main.transform.right : Vector3.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 horizontalMove = (camForward * directionZ + camRight * directionX);

        // Rotation towards movement direction
        if (horizontalMove.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(horizontalMove.x, horizontalMove.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // Final movement vector (CharacterController.Move uses meters per second -> multiply by deltaTime)
        Vector3 movement = (horizontalMove + Vector3.up * directionY) * Time.deltaTime;
        cc.Move(movement);
    }

    void LateUpdate()
    {
        // Do movement here after Update reads inputs and states
        if (cc != null)
            UpdateMovement();
    }

    bool CheckGrounded()
    {
        if (cc == null) return false;

        // Start from controller center in world
        Vector3 center = transform.TransformPoint(cc.center);

        // We cast from a little above the bottom of the capsule
        float castStartOffset = (cc.height * 0.5f) - cc.radius;
        Vector3 castOrigin = center + Vector3.up * 0.02f; // small lift to avoid inside-ground starts

        // Total cast distance to reach ground
        float castDistance = castStartOffset + groundCheckDistance;

        // SphereCast down
        bool hit = Physics.SphereCast(
            castOrigin,
            Mathf.Max(0.01f, groundCheckRadius),
            Vector3.down,
            out RaycastHit hitInfo,
            castDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        return hit;
    }

    // Ends jump if head hits something (with mask and ignoring self)
    void HeadHittingDetect()
    {
        if (cc == null) return;

        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = (cc.height / 2f) * headHitDistanceMultiplier;

        // Raycast up and ignore triggers
        if (Physics.Raycast(ccCenter, Vector3.up, out RaycastHit hit, hitCalc, headHitMask, QueryTriggerInteraction.Ignore))
        {
            // If we hit our own colliders (rare but possible if you have child colliders),
            // ignore it by checking transform root
            if (hit.transform.root == transform.root) return;

            jumpElapsedTime = 0f;
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!showGroundCheckGizmos) return;
        if (cc == null) cc = GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 center = transform.TransformPoint(cc.center);
        Vector3 origin = center + Vector3.up * 0.02f;

        Gizmos.DrawWireSphere(origin + Vector3.down * ((cc.height * 0.5f) - cc.radius + groundCheckDistance), groundCheckRadius);
    }
}
