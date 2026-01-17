using UnityEngine;

[RequireComponent(typeof(HitChceker))]
public abstract class WeaponBulletBase : MonoBehaviour
{
    protected float _lifeTime;
    protected float _moveSpeed;
    protected Vector2 _moveDirection;
    protected float _damage;
    protected float _elapsedTime;

    protected HitChceker _hitChceker;

    private void Start()
    {
        _hitChceker = GetComponent<HitChceker>();

        // イベントを購読（登録）
        _hitChceker.OnHit += HandleOnHit;
    }

    public virtual void Initialize(Vector2 moveDirection, float damage, float lifeTime = 3f, float moveSpeed = 3f)
    {
        _lifeTime = lifeTime;
        _moveSpeed = moveSpeed;
        _moveDirection = moveDirection.normalized;
        _damage = damage;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        Move();
        
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _lifeTime)
        {
            OnLifeTimeEnd();
        }
    }

    /// <summary>
    /// 弾の移動処理
    /// </summary>
    protected virtual void Move()
    {
        transform.position += (Vector3)(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 寿命が終了した時の処理
    /// </summary>
    protected virtual void OnLifeTimeEnd()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ヒット時の処理
    /// </summary>
    private void HandleOnHit(IDamageable damageable)
    {
        // ダメージを与える
        damageable.Damage((int)_damage);

        // 弾を非アクティブ化（オブジェクトプールに戻す）
        gameObject.SetActive(false);
    }
}