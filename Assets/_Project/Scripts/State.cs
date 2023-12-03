using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class State
{
    string stateName;
    Action onEnterState;
    Action onExitState;
    Action whileInState;

    public State(string sName, Action enter, Action whileIn, Action exit)
    {
        stateName = sName;
        onEnterState = enter;
        onExitState = exit;
        whileInState = whileIn;
    }

    public string GetStateName()
    {
        return stateName;
    }

    public void Enter()
    {
        onEnterState?.Invoke();
    }

    public Action GetWhileInState()
    {
        return whileInState;
    }

    public void Exit()
    {
        onExitState?.Invoke();
    }
}
