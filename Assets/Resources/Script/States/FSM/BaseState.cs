using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    protected Animator objectAnimator;
    protected Object objectController;

    public BaseState(Object objectController)
    {
        this.objectController = objectController;

        if (objectAnimator == null)
            objectAnimator = objectController.GetComponent<Animator>();
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
