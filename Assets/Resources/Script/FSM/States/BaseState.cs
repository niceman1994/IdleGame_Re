using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    protected Animator playerAnimator;
    protected Player playerController;

    public BaseState(Player controller)
    {
        playerController = controller;

        if (playerAnimator == null)
            playerAnimator = playerController.GetComponent<Animator>();
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
