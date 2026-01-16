using System;
using System.Runtime.CompilerServices;
using UnityEngine;



public class Player : MonoBehaviour, IDamageable
{

    private PlayerStatus _status;
    private PlayerMoveComponent _moveComponent;
    [SerializeField]
    private WeaponBase[] _weapons;
    private WeaponBase _currentWeapon;
    private PlayerState _state;

    public PlayerStatus PlayerStatus
    {
        get => _status;
    }

    public void Init()
    {
        _status = new PlayerStatus();
        
        _state = new PlayerState();

        InitComponent();
    }


    
    private void InitComponent()
    {
        _moveComponent = GetComponent<PlayerMoveComponent>();
        if (_moveComponent == null)
        {
            _moveComponent = this.gameObject.AddComponent<PlayerMoveComponent>();
        }

        //武器生成を行うならここで行う

    }

    public void Controll(Vector2 input, bool isShot, bool isCharge)
    {
        _moveComponent.OnCallMove(input, Time.deltaTime);
    }

    void IDamageable.Damage(int damage)
    {
        _status.HP -= damage;
    }
}

public interface IWeaponCoolTimeGetable
{
    public float WeaponChangeCoolTime { get; }
    event Action<float> OnWeaponCoolTimeChanged;
}

public interface IWeaponCoolTimeSetable
{
    public float WeaponChangeCoolTime { set; }
}
public interface IWeaponCoolTimeHandle : IWeaponCoolTimeGetable, IWeaponCoolTimeSetable
{

}

public class PlayerStatus : EntityStatus, IWeaponCoolTimeHandle
{
    private float _weaponChangeCoolTime;
    public float WeaponChangeCoolTime
    {
        get
        {
            return _weaponChangeCoolTime;
        }
        set
        {
            _weaponChangeCoolTime = value;
            OnWeaponCoolTimeChanged.Invoke(_weaponChangeCoolTime);
        }

    }
    public event Action<float> OnWeaponCoolTimeChanged;


}

public class PlayerState : EntityState
{

}
