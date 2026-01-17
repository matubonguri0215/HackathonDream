using UnityEngine;

public class SniperRifle : WeaponBase
{
    private protected override void DoShot(Vector2 shotDirection, float bulletDamage)
    {
        // Get bullet from pool (expecting PiercingBullet prefab to be assigned to _bulletBase)
        WeaponBulletBase bullet = BulletObjectPool.Instance.GetBullet(_bulletBase, _InitBulletPos.position, Quaternion.identity);
        
        if (bullet != null)
        {
            // Sniper bullets are typically faster and live longer or shorter depending on design.
            // Using base parameters. _moveSpeed should be high for Sniper.
            bullet.Initialize(shotDirection, bulletDamage, 5f, _moveSpeed);
        }
    }
}
