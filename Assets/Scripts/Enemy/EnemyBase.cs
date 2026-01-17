using System;
using UnityEngine;


public interface IEnemyAI
{
    void OnCallAI();
}
public abstract class EnemyBase : MonoBehaviour, IDamageable,IEnemyAI,IInjectable<IEntityTransformGetable>
{

    [SerializeField]
    protected EnemyStatus _stataus;
    protected Transform _playerTransform;

    public event Action OnEnemyDead;
    public void BaseInit()
    {
        _stataus.OnHPChanged += OnHPChanged;
    }
    private void OnHPChanged(int hp)
    {
        if(hp<=0)
        {
            OnEnemyDead?.Invoke();
        }
    }

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

[Serializable]
public class EnemyStatus:EntityStatus
{

}