using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    protected Object objectController;

    public BaseState(Object controller)
    {
        objectController = controller;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
