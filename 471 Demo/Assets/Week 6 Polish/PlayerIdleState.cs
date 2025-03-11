using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm idling!");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // What are we doing during this state? Nothing!

        // On what conditions do we leave the state?
        if (player.movement.magnitude > 0.1f)
        {
            if (player.isSneaking)
            {
                player.SwitchState(player.sneakState);
            }
            else
            {
                player.SwitchState(player.walkState);
            }
        }
    }
}
