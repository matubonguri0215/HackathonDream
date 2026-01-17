using UnityEngine;

public class AssaultRifle : WeaponBase
{
    private protected override void DoShot(Vector2 shotDirection, float bulletDamage)
    {
        WeaponBulletBase bullet = Instantiate(_bulletBase, _InitBulletPos.position, Quaternion.identity);
        bullet.Initialize(shotDirection, bulletDamage, moveSpeed: _moveSpeed);
    }
}
