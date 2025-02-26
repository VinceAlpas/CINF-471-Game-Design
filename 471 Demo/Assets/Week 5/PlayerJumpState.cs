using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm Jumping!"); //  Added Jump Message
        player.verticalVelocity = player.jumpForce; //  Apply jump force
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Apply gravity
        player.verticalVelocity += player.gravity * Time.deltaTime;

        // Move player upwards
        Vector3 move = new Vector3(player.movement.x, player.verticalVelocity, player.movement.y);
        player.controller.Move(move * Time.deltaTime);

        // Transition: If touching ground, switch to walking
        if (player.controller.isGrounded)
        {
            player.SwitchState(player.walkState);
        }
    }
}
