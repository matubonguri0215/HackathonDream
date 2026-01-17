using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾丸オブジェクトプールを管理するシングルトンクラス
/// 複数の弾丸タイプをサポートし、各タイプごとにプールを作成します
/// </summary>
public class BulletObjectPool : MonoSingleton<BulletObjectPool>
{
    private Dictionary<WeaponBulletBase, ObjectPool<WeaponBulletBase>> _pools = new Dictionary<WeaponBulletBase, ObjectPool<WeaponBulletBase>>();
    private Dictionary<int, ObjectPool<WeaponBulletBase>> _instanceToPoolMap = new Dictionary<int, ObjectPool<WeaponBulletBase>>();

    public WeaponBulletBase GetBullet(WeaponBulletBase prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("BulletObjectPool: Prefab is null!");
            return null;
        }

        if (!_pools.ContainsKey(prefab))
        {
            var newPool = new ObjectPool<WeaponBulletBase>();
            // Create a parent transform for organization
            GameObject poolParent = new GameObject($"Pool_{prefab.name}");
            poolParent.transform.SetParent(this.transform);
            
            newPool.Initialize(prefab, poolParent.transform);
            _pools.Add(prefab, newPool);
        }

        ObjectPool<WeaponBulletBase> pool = _pools[prefab];
        WeaponBulletBase bullet = pool.Get();
        
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;

        // Map the instance to the pool for easy return
        int ids = bullet.GetInstanceID();
        if (!_instanceToPoolMap.ContainsKey(ids))
        {
            _instanceToPoolMap[ids] = pool;
        }

        return bullet;
    }

    public void ReturnBullet(WeaponBulletBase bullet)
    {
        if (bullet == null) return;

        int ids = bullet.GetInstanceID();
        if (_instanceToPoolMap.TryGetValue(ids, out var pool))
        {
            pool.Return(bullet);
        }
        else
        {
            Debug.LogWarning($"BulletObjectPool: Trying to return a bullet that wasn't spawned by this pool? {bullet.name}");
            Destroy(bullet.gameObject);
        }
    }
}
