public interface IBuffEffect
{
    public void Apply(IObject targetObject);
    public void Expire(IObject targetObject);
}

public class AttackBuff : IBuffEffect
{
    private float buffAmount;
    public AttackBuff(float buffAmount)
    {
        this.buffAmount = buffAmount;
    }

    public void Apply(IObject targetObject)
    {
        targetObject.CurrentAtk(buffAmount);
    }

    public void Expire(IObject targetObject)
    {
        targetObject.CurrentAtk(-buffAmount);
    }
}

public class HealBuff : IBuffEffect
{
    private float buffAmount;
    public HealBuff(float buffAmount)
    {
        this.buffAmount = buffAmount;
    }

    public void Apply(IObject targetObject)
    {
        targetObject.CurrentHpChange(buffAmount);
    }

    public void Expire(IObject targetObject) 
    {
        
    }
}

public class GoldBuff : IBuffEffect
{
    private int buffAmount;
    public GoldBuff(int buffAmount)
    {
        this.buffAmount = buffAmount;
    }

    public void Apply(IObject targetObject)
    {
        GameManager.Instance.gameGold.curGold[0] += buffAmount;
    }

    public void Expire(IObject targetObject)
    {
        
    }
}
