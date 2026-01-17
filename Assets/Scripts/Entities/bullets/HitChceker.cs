using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HitChceker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _collisionLayerMask;

    private CircleCollider2D _circleCollider;
    private RaycastHit2D[] _collisionResults;

    public Action<IDamageable> OnHit;
   
    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// 衝突判定確認メソッド
    /// </summary>
    public void CheckCollision()
    {
        _collisionResults = Physics2D.CircleCastAll(transform.position, _circleCollider.radius, Vector2.down, 0f, _collisionLayerMask);

        for (int i = 0; i < _collisionResults.Length; i++)
        {
           GiveDamage(_collisionResults[i].collider);
        }
    }

    private void GiveDamage(Collider2D collisionCollider)
    {
        if (collisionCollider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            OnHit?.Invoke(component);
        }
    }
}
