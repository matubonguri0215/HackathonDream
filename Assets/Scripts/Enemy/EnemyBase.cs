using UnityEngine;


public interface IEnemyAI
{
    void OnCallAI();
}
public abstract class EnemyBase : MonoBehaviour, IDamageable,IEnemyAI,IInjectable<IEntityTransformGetable>
{

    [SerializeField]
    private EnemyStatus _stataus;
    private Transform _playerTransform;

    public EnemyStatus Stataus
    {
        get { return _stataus; }
    }

    void IDamageable.Damage(int damage)
    {
        _stataus.HP -= damage;
    }

    public abstract void OnCallAI();

    public void Inject(IEntityTransformGetable instance)
    {
        _playerTransform = instance.transform;
    }
}

[SerializeField]
public class EnemyStatus:EntityStatus
{

}