using System;
using UnityEngine;

    

public class Player:MonoBehaviour
{

    private PlayerStatus _status;
   
    public PlayerStatus PlayerStatus
    {
        get => _status;
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
public interface IWeaponCoolTimeHandle:IWeaponCoolTimeGetable, IWeaponCoolTimeSetable
{

}

public class PlayerStatus:EntityStatus,IWeaponCoolTimeHandle
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
