using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm walking!");

        Collider[] enemiesInRange = Physics.OverlapSphere(player.transform.position, 6f);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            EnemyAI enemy = enemyCollider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.SetTarget(player.gameObject);  
                enemy.SetState(EnemyAI.State.Follow);  
            }
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // What are we doing during this state?
        player.MovePlayer(player.default_speed);

        // On what conditions do we leave the state?
        if (player.movement.magnitude < 0.1f)
        {
            player.SwitchState(player.idleState);
        }
        else if (player.isSneaking)
        {
            player.SwitchState(player.sneakState);
        }
    }
}
