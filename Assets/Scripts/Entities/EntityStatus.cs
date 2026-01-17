using System;
using UnityEngine;

public interface IHPGetable
{
    int HP { get; }
    event Action<int> OnHPChanged;
}
public interface IHPSetable
{ 
    int HP { set; }
}
public interface IHPHandle : IHPGetable, IHPSetable { }
public interface IMoveForceGetable
{
    float MoveForce { get; }
    event Action<float> OnMoveForceChanged;
}
public interface IMoveForceSetable
{
    float MoveForce { set; }
}

public interface IMoveForceHandle:IMoveForceGetable,IMoveForceSetable{}

[Serializable]
public class EntityStatus:IHPHandle,IMoveForceHandle
{
    [SerializeField]
    private int _maxHP;
    public int MaxHP
    {
        get => _maxHP;
        set
        {
            _maxHP = value;
            OnMaxHPChanged.Invoke(_maxHP);
        }
    }
    public event Action<int> OnMaxHPChanged;

    [SerializeField]
    protected int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (value > 0)
            {
                OnHPIncreased.Invoke(_hp);
            }
            else
            {
                OnHPdecreased.Invoke(_hp);
                OnHPChanged.Invoke(_hp);
            }
        }
    }
    public event Action<int> OnHPChanged;
    public event Action<int> OnHPdecreased;
    public event Action<int> OnHPIncreased;

    [SerializeField]
    protected float _moveForce;
    public float MoveForce
    {
        get => _moveForce;
        set
        {
            _moveForce = value;
            OnMoveForceChanged.Invoke(_moveForce);
        }
    }
    public event Action<float> OnMoveForceChanged;

}