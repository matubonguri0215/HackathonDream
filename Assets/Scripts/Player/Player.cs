using System;
using System.Runtime.CompilerServices;
using UnityEngine;



public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
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
    public PlayerState PlayerState
    {
        get => _state;
    }

    public void Init()
    {


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
        Debug.Log("MoveForce " + _status.MoveForce);
        _moveComponent.Inject(_status);
        //武器生成を行うならここで行う

    }

    public void Controll(Vector2 inputL, Vector2 inputR, bool isShot, bool isCharge)
    {
        _moveComponent.OnCallMove(inputL, Time.deltaTime);
        Vector3 dir = inputR;
        float z = (Mathf.Atan2(inputR.y, inputR.x)-Mathf.PI/2)*Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(0,0,z);
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

[Serializable]
public class PlayerStatus : EntityStatus, IWeaponCoolTimeHandle
{
    [SerializeField]
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


    [SerializeField]
    private float _chargeMaxTime;
    private float _currentChargeTime;
    public float ChargeMaxTime
    {
        get => _chargeMaxTime;
    }
    public float CurrentChargeTime
    {
        get => _currentChargeTime;
        set
        {
            _currentChargeTime = Mathf.Clamp(value, 0, _chargeMaxTime);
            OnChargeTimeChanged?.Invoke(_currentChargeTime);
        }
    }
    public event Action<float> OnChargeTimeChanged;

}

public class PlayerState : EntityState
{

}
