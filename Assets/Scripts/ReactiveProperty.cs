using System;
using System.Collections.Generic;

/// <summary>
/// SubscribeすることでReactivePropertyのValueが変更されたときにコールバックが呼ばれる。
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReactiveProperty<T>
{
    #region Field
    private T _value;
    private List<Action<T>> _observers = new();
    #endregion


    #region Property
    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                NortifyObservers();
            }
        }
    }
    #endregion
    

    #region Methods
    public ReactiveProperty(T initialValue = default)
    {
        _value = initialValue;
    }
    public void Subscribe(Action<T> obserber)
    {
        _observers.Add(obserber);
        obserber(_value);
    }
    public void UnSubscribe(Action<T> obserber)
    {
        _observers.Remove(obserber);
    }
    private void NortifyObservers()
    {
        foreach (var obserber in _observers)
        {
            obserber(_value);
        }
    }
    #endregion
}