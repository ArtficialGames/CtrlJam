using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    List<State> states = new List<State>();
    State currentState;

    public void Init(State[] initialStates)
    {
        states = initialStates.ToList();
        GoToState(states[0].GetStateName());
    }

    public void GoToState(string stateName)
    {
        State stateToGo = null;

        foreach (var state in states)
        {
            if(state.GetStateName() == stateName)
            {
                stateToGo = state;
                break;
            }
        }

        if (stateToGo == null)
            return;

        if (currentState != null)
            currentState.Exit();

        stateToGo.Enter();
        currentState = stateToGo;
    }

    public string GetCurrentStateName()
    {
        return currentState.GetStateName();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.GetWhileInState()?.Invoke();
    }
}
