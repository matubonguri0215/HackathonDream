using UnityEngine;

public class AssaultRifle : WeaponBase
{    
    private protected override void DoShot(Vector2 shotDirection, float bulletDamage)
    {
        _bulletBase = Instantiate(_bulletBase, _InitBulletPos.position, Quaternion.identity);

        _bulletBase.Initialize(shotDirection, _weaponDamage);
    }
}
