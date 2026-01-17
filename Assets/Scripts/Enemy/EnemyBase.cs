using UnityEngine;


public interface IEnemyAI
{
    void OnCallAI();
}
public abstract class EnemyBase : MonoBehaviour, IDamageable,IEnemyAI
{

    [SerializeField]
    private EnemyStatus _stataus;

    public EnemyStatus Stataus
    {
        get { return _stataus; }
    }

    void IDamageable.Damage(int damage)
    {
        _stataus.HP -= damage;
    }

    public abstract void OnCallAI();
}

[SerializeField]
public class EnemyStatus:EntityStatus
{

}