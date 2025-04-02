using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm Jumping!"); 
        player.JumpPlayer(); //  Apply jump force
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Transition: If touching ground, switch to walking
        if (player.isGrounded)
        {
            player.IsJumping = false;
            player.SwitchState(player.idleState);
        }
    }
}
