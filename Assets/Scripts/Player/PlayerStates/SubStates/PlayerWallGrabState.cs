using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家抓墙状态
/// </summary>
public class PlayerWallGrabState : PlayerToucingWallState
{
    /// <summary>
    /// 进入抓取状态的玩家位置
    /// </summary>
    private Vector2 holdPosition;

    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        holdPosition = player.transform.position;

        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            HoldPosition();

            if (yInput > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            else if (yInput < 0 || !grabInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }

    /// <summary>
    /// 保持原位
    /// </summary>
    private void HoldPosition()
    {
        player.transform.position = holdPosition;

        core.Movement.SetVelocityX(0f);
        core.Movement.SetVelocityY(0f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
