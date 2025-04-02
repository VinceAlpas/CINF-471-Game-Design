using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public PlayerBaseState currentState;
    [HideInInspector] public PlayerIdleState idleState = new PlayerIdleState();
    [HideInInspector] public PlayerWalkState walkState = new PlayerWalkState();
    [HideInInspector] public PlayerSneakState sneakState = new PlayerSneakState();
    [HideInInspector] public PlayerSprintState sprintState = new PlayerSprintState();
    [HideInInspector] public PlayerJumpState jumpState = new PlayerJumpState();
    [HideInInspector] public PlayerAttackState attackState = new PlayerAttackState();
    [HideInInspector] public PlayerDoubleJumpState doubleJumpState = new PlayerDoubleJumpState();

    [HideInInspector] public Vector2 movement;
     public Vector2 lookInput;
     public Transform playerCamera;
     public float mouseSensitivity = 30f;
     public float xRotation = 0f;
     
     public float default_speed = 2f;
     public float sprint_speed = 3f;
     public float sneak_speed = 0.5f;
     public float jumpForce = 0.3f;
     public bool isSneaking = false;
     public bool isSprinting = false;
     public bool IsJumping = false;
     public bool isAttacking = false;
     public CharacterController controller;
     public Vector3 velocity;
     public float gravity = -20f;
     public float verticalVelocity;
     public bool isGrounded;
     
     private bool hasDoubleJumped = false;
     private float lastJumpTime = -100f;

     public float doubleJumpWindow = 0.3f;

    // Sword & Animator References
    public GameObject sword;
    public Animator animator;

    // Bullet & Bullet Spawner References
    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    private bool isFiring = false;
    private float fireRate = 0.1f;
    private float lastFireTime = 0f;

    // Fall damage
    public float fallThreshold = -10f;
    // Player Health
    public int playerHealth = 100;

    // To track the previous state
    private PlayerBaseState previousState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>(); // Ensure Animator is assigned
        SwitchState(idleState);
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps character grounded

            if(movement.magnitude > 0.1f)
            {
                if (isSprinting)
                    SwitchState(sprintState);
                else
                    SwitchState(walkState);
            }
            else
            {
            SwitchState(idleState);
            }
        }

        currentState.UpdateState(this);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleMouseLook();
        HandleShooting();

        if (transform.position.y < fallThreshold)
        {
            Die();
        }
    }

    // Handle Movement Input
    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();

        if (isAttacking) return; // Prevent movement while attacking

        if (movement.magnitude > 0.1f)
        {
            if (isSprinting)
            {
                SwitchState(sprintState);
            }
            else if (isSneaking)
            {
                SwitchState(sneakState);
            }
            else
            {
                SwitchState(walkState);
            }
        }
        else
        {
            SwitchState(idleState);
        }
    }

    void OnLook(InputValue lookVal)
    {
        lookInput = lookVal.Get<Vector2>();
    }

    // Handle Sneak Input (C key)
    void OnSneak(InputValue sneakVal)
    {
        isSneaking = sneakVal.isPressed;

        if (isSneaking && !isSprinting)
        {
            Debug.Log("Sneaking Activated");
            SwitchState(sneakState);
        }
        else if (!isSneaking && !isSprinting)
        {
            Debug.Log("Sneaking Deactivated");
            SwitchState(walkState);
        }
    }

    // Handle Sprint Input (Left Shift)
    void OnSprint(InputValue sprintVal)
    {
        isSprinting = sprintVal.isPressed;
        if (isSprinting && !isSneaking)
        {
            SwitchState(sprintState);
        }
        else if (isSprinting && !isSneaking)
        {
            SwitchState(walkState);
        }
    }

    // Handle Jump Input (Spacebar)
    void OnJump()
    {
        if (isGrounded)
        {
            // normal jump
            JumpPlayer();
            lastJumpTime = Time.time;
            hasDoubleJumped = false;
        }
        else
        {
            // double jump
            if (Time.time - lastJumpTime <= doubleJumpWindow && !hasDoubleJumped)
            {
                SwitchState(doubleJumpState);
                doubleJumpState.RecordJumpTime();
            }
        }
    }

    // Handle Attack Input (Left Click)
    void OnAttack()
    {
        if (!isAttacking) // Prevent spam attacks
        {
            Debug.Log("Sword Attack!");
            isAttacking = true;

            // **Play Attack Animation**
            animator.SetTrigger("Attack");

            // **Switch to Attack State**
            SwitchState(attackState);

            // **Return to Previous State After Attack Finishes**
            Invoke("EndAttack", 0.5f); // Adjust time based on animation length
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        if (movement.magnitude > 0.1f)
        {
            SwitchState(walkState);
        }
        else
        {
            SwitchState(idleState);
        }
    }

    // Helper Function to Move the Player
    public void MovePlayer(float speed)
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        if (isAttacking) return; // Prevent movement during attack

        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = transform.right * move.x + transform.forward * move.z; // Ensure movement is relative to camera

        Vector3 moveDirection = forward * movement.y + right * movement.x;
        controller.Move(move * Time.deltaTime * speed);
    }

    // Switching Between States
    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    void HandleShooting()
    {
        if (Mouse.current.rightButton.isPressed && Time.time > lastFireTime + fireRate)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    void FireBullet()
    {
        if (bulletPrefab == null || bulletSpawner == null)
        {
            Debug.LogError("❌ Bullet Prefab or Spawner is missing.");
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletSpawner.transform.forward * 20f;
        }

        Destroy(newBullet, 5f);
    }

    private void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void JumpPlayer()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void Die()
    {
        Debug.Log("Player has fallen off the platform!");
    }
}
