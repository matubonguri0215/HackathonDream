using System.Collections.Generic;

/// <summary>
/// 弾丸オブジェクトプールを管理するシングルトンクラス
/// 複数の弾丸タイプをサポートし、各タイプごとにプールを作成します
/// </summary>
public class BulletObjectPool
{
    private List<WeaponBulletBase> _useBullet;
    private List<WeaponBulletBase> _unUseBullet;

    public BulletObjectPool()
    {

    }

    public WeaponBulletBase GetBullet()
    {
        return null;
    }
}
