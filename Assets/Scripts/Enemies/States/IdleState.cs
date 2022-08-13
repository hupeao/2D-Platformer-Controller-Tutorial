using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 待机状态
/// </summary>
public class IdleState : State
{
    /// <summary>
    /// 状态数据
    /// </summary>
    protected D_IdleState stateData;

    /// <summary>
    /// 待机后翻转
    /// </summary>
    protected bool flipAfterIdle;
    /// <summary>
    /// 是否待机时间结束
    /// </summary>
    protected bool isIdleTimeOver;
    /// <summary>
    /// 玩家是否在最小仇恨范围内
    /// </summary>
    protected bool isPlayerInMinAggroRange;

    /// <summary>
    /// 待机时间
    /// </summary>
    protected float idleTime;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0);
        isIdleTimeOver = false;
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    /// <summary>
    /// 设置待机后翻转
    /// </summary>
    /// <param name="flip"></param>
    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    /// <summary>
    /// 设置随机待机时间
    /// </summary>
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
