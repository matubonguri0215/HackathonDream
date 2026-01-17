using UnityEngine;

public class AssaultRifle : WeaponBase
{
    private protected override void DoShot(Vector2 shotDirection, float bulletDamage)
    {
        WeaponBulletBase bullet = BulletObjectPool.Instance.GetBullet(_bulletBase, _InitBulletPos.position, Quaternion.identity);
        bullet.Initialize(shotDirection, bulletDamage, moveSpeed: _moveSpeed);
    }
}
