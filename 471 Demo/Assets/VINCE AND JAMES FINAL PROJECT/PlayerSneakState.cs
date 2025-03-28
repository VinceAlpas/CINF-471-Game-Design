using UnityEngine;

public class PlayerSneakState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering Sneak Mode");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.MovePlayer(player.default_speed / 2);


        // Exit to Idle if no movement
        if (player.movement.magnitude < 0.1f)
        {
            player.SwitchState(player.idleState);
        }
        // Exit to Walk if 'C' is released
        else if (!player.isSneaking)
        {
            player.SwitchState(player.walkState);
        }
    }
}
