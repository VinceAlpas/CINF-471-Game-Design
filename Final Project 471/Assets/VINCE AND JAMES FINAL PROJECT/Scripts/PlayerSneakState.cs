using UnityEngine;

public class PlayerSneakState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering Sneak Mode");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // Move the player at half speed when sneaking
        player.MovePlayer(player.sneak_speed);

        // Exit to Idle if no movement
        if (player.movement.magnitude < 0.1f)
        {
            player.SwitchState(player.idleState);
        }
        // Exit to Walk if 'CONTROL' is released and not sneaking
        else if (!player.isSneaking)
        {
            player.SwitchState(player.walkState);
        }
    }
}
