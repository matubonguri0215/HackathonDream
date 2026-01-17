using UnityEngine;
using System.Collections.Generic;

public class PiercingBullet : WeaponBulletBase
{
    private HashSet<int> _hitTargets = new HashSet<int>();

    public override void Initialize(Vector2 moveDirection, float damage, float lifeTime = 3f, float moveSpeed = 3f)
    {
        base.Initialize(moveDirection, damage, lifeTime, moveSpeed);
        _hitTargets.Clear();
    }

    protected override void HandleOnHit(IDamageable damageable)
    {
        // Try to identify the target to avoid multi-hits on the same target
        var component = damageable as Component;
        if (component != null)
        {
            int id = component.gameObject.GetInstanceID();
            if (_hitTargets.Contains(id))
            {
                return;
            }
            _hitTargets.Add(id);
        }

        // Deal damage
        damageable.Damage((int)_damage);
        
        // Intentionally do NOT call Kill() to allow piercing
    }

    protected override void Kill()
    {
        // Return to pool
        if (Application.isPlaying) 
        {
            BulletObjectPool.Instance.ReturnBullet(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
