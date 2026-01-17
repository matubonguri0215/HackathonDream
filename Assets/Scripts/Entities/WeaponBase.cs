using UnityEngine;

public abstract class WeaponBase:MonoBehaviour
{
    [SerializeField]
    private protected float _coolDownTime;
    [SerializeField]
    private protected float _moveSpeed;
    [SerializeField]
    private protected float _weaponDamage;
    [SerializeField]
    private protected WeaponBulletBase _bulletBase;

    [SerializeField]
    private protected Transform _InitBulletPos;

    private protected float _nowCoolDownTime = 0f;

    public void OnCallShot(float healthRatio, Vector2 shotDirection)
    {
        if (ChackCoolDownTime())
        {
            DoShot(shotDirection, GetDamage(healthRatio));
        }
    }

    private protected abstract void DoShot(Vector2 shotDirection, float bulletDamage);
   
    private bool ChackCoolDownTime()
    {
        _nowCoolDownTime += Time.deltaTime;

        if (_nowCoolDownTime > _coolDownTime)
        {
            _nowCoolDownTime = 0f;
            return true;
        }
        return false;
    }

    private float GetDamage(float healthRatio)
    {
        float damageUpRatio = 1 - healthRatio;
        return damageUpRatio + 1;
    }
}