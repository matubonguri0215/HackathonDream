using UnityEngine;

public abstract class WeaponBulletBase:MonoBehaviour
{

    protected float _lifeTime;
    protected float _moveSpeed;
    protected Vector2 _moveDirection;
    protected float _damage;

    public virtual void Initialize(float lifeTime,float moveSpeed,Vector2 moveDirection,float damage)
    {
        _lifeTime = lifeTime;
        _moveSpeed = moveSpeed;
        _moveDirection = moveDirection.normalized;
        _damage = damage;
    }

}