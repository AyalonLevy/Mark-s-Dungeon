using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, InputReader inputReader, PlayerData playerData) : base(player, playerStateMachine, inputReader, playerData)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Entered Move State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (inputReader.IsAttacking)
        {
            playerStateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector2 rawDirection = inputReader.movement;
        bool isSprinting = inputReader.IsSprinting && player.CurrentStamina > 0.0f;

        float speed = isSprinting ? playerData.SprintSpeed : playerData.WalkSpeed;

        Vector3 moveDirection = new(rawDirection.x, 0.0f, rawDirection.y);

        player.Move(moveDirection * speed);

        if (isSprinting && moveDirection.sqrMagnitude > playerData.MovementThreshold)
        {
            player.CurrentStamina = Mathf.Max(0, player.CurrentStamina - (playerData.StaminaDrainRate * Time.fixedDeltaTime));
        }

        if (inputReader.movement.sqrMagnitude < playerData.MovementThreshold)
        {
            playerStateMachine.ChangeState(player.IdleState);
        }
    }
}
