
using UnityEngine;

public class BattleEntry : MonoBehaviour
{
    [SerializeField]
    private PlayerBuilder _playerBuilder;
    [SerializeField]
    private EnemyBuilder _enemyBuilder;

    private void Start()
    {
        InitBuilder();
        BuildEntities();
    }

    private void InitBuilder()
    {
        if (_playerBuilder == null)
        {
            _playerBuilder = FindAnyObjectByType<PlayerBuilder>();
            if (_playerBuilder == null)
            {
                MyDebugLogger.LogError("PlayerBuilder not found in the scene", MyDebugLogger.LogCategory.Initialization);
                return;
            }
        }

       

        if (_enemyBuilder == null)
        {
            _enemyBuilder = FindAnyObjectByType<EnemyBuilder>();
            if (_enemyBuilder == null)
            {
                MyDebugLogger.LogError("EnemyBuilder not found in the scene", MyDebugLogger.LogCategory.Initialization);
                return;
            }
        }

    }

    private void BuildEntities()
    {
        _playerBuilder.BuildPlayer();
        _enemyBuilder.BuildEnemy();
    }

}