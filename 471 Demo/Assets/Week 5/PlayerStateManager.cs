using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public PlayerBaseState currentState;

    [HideInInspector] public PlayerIdleState idleState = new PlayerIdleState();
    [HideInInspector] public PlayerWalkState walkState = new PlayerWalkState();
    [HideInInspector] public PlayerSneakState sneakState = new PlayerSneakState();
    [HideInInspector] public PlayerSprintState sprintState = new PlayerSprintState();
    [HideInInspector] public PlayerJumpState jumpState = new PlayerJumpState();
    [HideInInspector] public PlayerAttackState attackState = new PlayerAttackState();

    [HideInInspector] public Vector2 movement;

    public float default_speed = 2f;
    public float sprint_speed = 4f;
    public float sneak_speed = 1f;

    public bool isSneaking = false;
    public bool isSprinting = false;
    public bool isGrounded;
    public bool isAttacking = false;

    public CharacterController controller;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float verticalVelocity;

    // 🔥 Sword & Animator References
    public GameObject sword;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Ensure Animator is assigned
        SwitchState(idleState);
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Keeps character grounded
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 move = new Vector3(movement.x, verticalVelocity, movement.y);
        controller.Move(move * Time.deltaTime);

        currentState.UpdateState(this);
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

    // Handle Sneak Input (C key)
    void OnSneak(InputValue sneakVal)
    {
        isSneaking = sneakVal.isPressed;

        if (isSneaking)
        {
            Debug.Log("Sneaking Activated");
            SwitchState(sneakState);
        }
        else
        {
            Debug.Log("Sneaking Deactivated");
            SwitchState(walkState);
        }
    }

    // Handle Sprint Input (Left Shift)
    void OnSprint(InputValue sprintVal)
    {
        isSprinting = sprintVal.isPressed;

        if (isSprinting)
        {
            SwitchState(sprintState);
        }
        else if (movement.magnitude > 0.1f)
        {
            SwitchState(walkState);
        }
    }

    // Handle Jump Input (Spacebar)
    void OnJump()
    {
        if (isGrounded)
        {
            Debug.Log("I'm Jumping!");
            SwitchState(jumpState);
        }
    }

    // Handle Attack Input (Left Click)
    void OnAttack()
    {
        if (!isAttacking) // Prevent spam attacks
        {
            Debug.Log("Sword Attack!");
            isAttacking = true;

            // 🔥 **Play Attack Animation**
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

    // ✅ Helper Function to Move the Player
    public void MovePlayer(float speed)
    {
        if (isAttacking) return; // Prevent movement during attack

        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = transform.right * move.x + transform.forward * move.z; // Ensure movement is relative to camera

        controller.Move(move * Time.deltaTime * speed);
    }

    // ✅ Switching Between States
    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
}
