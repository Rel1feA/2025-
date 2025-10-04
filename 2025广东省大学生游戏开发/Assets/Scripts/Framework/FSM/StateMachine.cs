using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public State nextState;

    public void Init(State state)
    {
        currentState = state;
        currentState.EnterState();
    }

    public void Changestate(State newState)
    {
       this.nextState=newState;
    }


    /// <summary>
    /// 用于检测状态的切换，必须放在lateupdate执行，防止旧的状态的每帧方法会在转换状态后还继续执行
    /// </summary>
    public void CheckChangeState()
    {
        if(nextState!= null)
        {
            currentState.ExitState();
            currentState = nextState;
            currentState.EnterState();
            nextState= null;
        }
    }
}
