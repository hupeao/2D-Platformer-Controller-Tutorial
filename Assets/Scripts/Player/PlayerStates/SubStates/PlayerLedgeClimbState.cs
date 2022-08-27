using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家平台攀爬状态
/// </summary>
public class PlayerLedgeClimbState : PlayerState
{
    /// <summary>
    /// 检测位置
    /// </summary>
    private Vector2 detectedPos;
    /// <summary>
    /// 内角位置
    /// </summary>
    private Vector2 cornerPos;
    /// <summary>
    /// 起始位置
    /// </summary>
    private Vector2 startPos;
    /// <summary>
    /// 停下位置
    /// </summary>
    private Vector2 stopPos;

    /// <summary>
    /// 是否在挂着
    /// </summary>
    private bool isHanging;
    /// <summary>
    /// 是否在攀爬
    /// </summary>
    private bool isClimbing;
    /// <summary>
    /// 跳跃输入
    /// </summary>
    private bool jumpInput;
    /// <summary>
    /// 是否接触天花板
    /// </summary>
    private bool isTouchingCeiling;

    /// <summary>
    /// X轴输入
    /// </summary>
    private int xInput;
    /// <summary>
    /// Y轴输入
    /// </summary>
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = startPos;

            if (xInput == player.FacingDirection && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    /// <summary>
    /// 设置检测位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    /// <summary>
    /// 检查空间
    /// </summary>
    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);
        player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }
}
