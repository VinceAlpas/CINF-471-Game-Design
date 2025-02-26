using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float attackDuration = 0.5f; // Duration of the attack animation
    private float attackTimer = 0f;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm Attacking!");

        // Play Attack Animation
        if (player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
        else
        {
            Debug.LogWarning("No Animator found on Player!");
        }

        // Enable the sword collider for the attack
        if (player.sword != null)
        {
            Collider swordCollider = player.sword.GetComponent<Collider>();
            if (swordCollider != null)
            {
                swordCollider.enabled = true; // Activate collider only during attack
            }
            else
            {
                Debug.LogWarning("Sword does not have a Collider!");
            }
        }
        else
        {
            Debug.LogWarning("Sword is not assigned in PlayerStateManager!");
        }

        attackTimer = 0f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)
        {
            player.SwitchState(player.idleState);
        }
    }
}
