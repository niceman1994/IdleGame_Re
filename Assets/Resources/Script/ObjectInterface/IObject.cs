/// <summary>
/// 공격과 연관된 IAttack 인터페이스를 상속받고 체력과 관련된 함수가 들어있는 인터페이스
/// </summary>
public interface IObject : IAttack, IBuffable
{
    public float CurrentHp();
    public void CurrentHpChange(float hp);
    public void HpUp(float addhp);
}
