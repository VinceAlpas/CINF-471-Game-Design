using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);

    public abstract void UpdateState(PlayerStateManager player);

    public virtual void ExitState(PlayerStateManager player)
    {
        // Default behavior when exiting a state (optional)
    }
    
}
