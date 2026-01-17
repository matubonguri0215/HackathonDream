using UnityEngine;

public class EnemyBuilder:MonoBehaviour
{
    [SerializeField]
    private EnemyBase[] enemiePrefabs;
    [SerializeField]
    private float _spawmRange;
    private void Awake()
    {
        int rnd = Random.Range(0, enemiePrefabs.Length);
        Instantiate(enemiePrefabs[rnd], Vector3.zero, Quaternion.identity);
    }
}