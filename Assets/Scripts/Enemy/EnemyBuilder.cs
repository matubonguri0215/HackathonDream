using UnityEngine;

public class EnemyBuilder:MonoBehaviour
{
    [SerializeField]
    private EnemyBase[] enemiePrefabs;
    [SerializeField]
    private float _spawmRange;
    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }

    public void BuildEnemy()
    {
        int rnd = Random.Range(0, enemiePrefabs.Length);
        EnemyBase enemy = Instantiate<EnemyBase>(enemiePrefabs[rnd], Vector3.zero, Quaternion.identity);
        EnemyAICaller.Register(enemy);
        enemy.Inject(PlayerDataProvider.GetPlayer());
    }
}