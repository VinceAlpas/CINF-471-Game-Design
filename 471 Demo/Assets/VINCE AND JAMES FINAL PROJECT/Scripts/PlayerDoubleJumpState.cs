using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    private bool hasDoubleJumped = false;
    private float lastJumpTime = -100f;  // Timestamp for the last jump
    public float doubleJumpWindow = 0.3f; // Time window for double jump (0.3 seconds)

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering Double Jump State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Handle first jump or second jump (double jump)
        if (!player.isGrounded)
        {
            // Double jump condition: Check if we're in the air and if the time window is still valid
            if (Time.time - lastJumpTime <= doubleJumpWindow && Input.GetButtonDown("Jump") && !hasDoubleJumped)
            {
                // Perform the double jump by applying more force to simulate jumping higher
                player.velocity.y = Mathf.Sqrt(player.jumpForce * -2f * player.gravity) * 2f; // Double the force for double jump
                hasDoubleJumped = true; // Mark that the player has double jumped
                Debug.Log("Double Jumped!");
                RecordJumpTime();  // Update the jump time to prevent multiple double jumps within the window
            }
        }

        // Reset the double jump when the player is grounded
        if (player.isGrounded)
        {
            hasDoubleJumped = false; // Reset double jump flag
            player.SwitchState(player.walkState); // Switch to walking or any other state after landing
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exiting Double Jump State");
    }

    public void RecordJumpTime()
    {
        lastJumpTime = Time.time; // Store the time of the last jump
    }
}