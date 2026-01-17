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
                MainLoopEntry.Instance.RegisterStartable(_instance);
                MainLoopEntry.Instance.RegisterUpdatable(_instance);
            }
            return _instance;
        }
    }


    private EnemyAICaller()
    {



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