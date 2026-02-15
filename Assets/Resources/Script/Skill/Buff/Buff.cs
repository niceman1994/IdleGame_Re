using System;
using UnityEngine;

public abstract class Buff
{
    public bool IsActive { get; private set; }
    public float duration;

    protected float elapsed;
    protected IBuffEffect buffEffect;
    protected IObject targetObject;

    public virtual void Apply(IObject targetObject)
    {
        IsActive = true;
        this.targetObject = targetObject;
        buffEffect.Apply(targetObject);
    }

    public virtual void Tick(float deltaTime)
    {
        elapsed += deltaTime;
        
        // 버프 지속 시간이 지났거나 버프 대상의 체력이 0 이하일 때 버프를 종료함
        if (elapsed >= duration || targetObject.CurrentHp() <= 0)
        {
            ExpireBuff();
        }
    }

    public virtual void ExpireBuff()
    {
        IsActive = false;
        buffEffect.Expire(targetObject);
    }
}
