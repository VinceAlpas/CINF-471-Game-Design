using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm Sprinting!");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.MovePlayer(player.sprint_speed);

        // If player stops moving, switch to Idle
        if (player.movement.magnitude < 0.1f)
        {
            player.SwitchState(player.idleState);
        }
        // If Sprint key (Left Shift) is released, switch back to Walk
        else if (!player.isSprinting)
        {
            player.SwitchState(player.walkState);
        }
    }
}
