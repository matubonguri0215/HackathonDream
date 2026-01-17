using System.Collections.Generic;
using UnityEngine;

public class EnemyAICaller : IStartable, IUpdatable
{
    private static EnemyAICaller _instance;
    private static EnemyAICaller Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyAICaller();
                _instance.RegisterEntry();
            }
            return _instance;
        }
    }

    private void RegisterEntry()
    {
        MainLoopEntry.Instance.RegisterStartable(this);
        MainLoopEntry.Instance.RegisterUpdatable(this);
    }

    private EnemyAICaller()
    {



    }
    ~EnemyAICaller()
    {
        MainLoopEntry.Instance.UnregisterStartable(this);
        MainLoopEntry.Instance.UnregisterUpdatable(this);
    }

    private List<IEnemyAI> _enemyAIs = new ();
    void IStartable.OnStart()
    {

    }

    void IUpdatable.OnUpdate()
    {
       foreach(EnemyBase enemy in _enemyAIs)
        {
            enemy.OnCallAI();
        }
    }

    public static void Register(IEnemyAI enemy)
    {
        Instance._enemyAIs.Add(enemy);
    }

}