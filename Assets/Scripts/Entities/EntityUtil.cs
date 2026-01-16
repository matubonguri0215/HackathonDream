

public interface IDamageable
{
    void Damage(int damage);
}

public class EntityState
{
    public bool IsDead { get; set; } = false;
    public bool IsInvincible { get; set; } = false;
    public bool IsStunned { get; set; } = false;

}