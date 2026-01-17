using UnityEngine;

public class Shotgun : WeaponBase
{
    [SerializeField]
    private int _pelletCount = 5;

    [SerializeField]
    private float _spreadAngle = 30f;

    private protected override void DoShot(Vector2 shotDirection, float bulletDamage)
    {
        // If pellet count is 1 or less, just fire straight
        if (_pelletCount <= 1)
        {
            FireBullet(shotDirection, bulletDamage);
            return;
        }

        float angleStep = _spreadAngle / (_pelletCount - 1);
        float startAngle = -_spreadAngle / 2f;

        for (int i = 0; i < _pelletCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            // Rotate the direction vector
            Vector2 direction = RotateVector(shotDirection, currentAngle);
            FireBullet(direction, bulletDamage);
        }
    }

    private void FireBullet(Vector2 direction, float damage)
    {
        WeaponBulletBase bullet = BulletObjectPool.Instance.GetBullet(_bulletBase, _InitBulletPos.position, Quaternion.identity);
        if (bullet != null)
        {
            // Assuming default lifetime of 3f, or we could expose it in Shotgun if needed.
            // Using logic similar to WeaponBase if it had a default DoShot, but it's abstract.
            bullet.Initialize(direction, damage, 3f, _moveSpeed);
        }
    }

    private Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
