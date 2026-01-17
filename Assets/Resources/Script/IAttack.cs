/// <summary>
/// 공격, 현재 공격력, 받은 데미지에 대한 함수가 들어있는 인터페이스
/// </summary>
public interface IAttack
{
    public float CurrentAtk();
    public float CurrentAtk(float addAtk);
    public void GetAttackDamage(float dmg);
}
