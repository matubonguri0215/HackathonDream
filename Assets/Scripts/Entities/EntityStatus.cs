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
    private int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            OnHPChanged.Invoke(_hp);
        }
    }
    public event Action<int> OnHPChanged;

    [SerializeField]
    private float _moveForce;
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