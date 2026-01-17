

public interface IDamageable
{
    void Damage(int damage);
}

public interface IHealable
{
    void Heal(int healAmount);
}

public class EntityState
{
    public bool IsDead { get; set; } = false;
    public bool IsInvincible { get; set; } = false;
    public bool IsStunned { get; set; } = false;

}