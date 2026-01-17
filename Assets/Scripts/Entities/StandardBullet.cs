using UnityEngine;

public class StandardBullet : WeaponBulletBase
{
    protected override void Kill()
    {
        // Try to return to pool
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
