using UnityEngine;

public class PlayerBuilder:MonoBehaviour
{

    [SerializeField]
    private Player _playerPrefab;
    private PlayerController _playerController;
    [SerializeField]
    private Transform _playerSpawnPoint;
    private void Awake()
    {
        Vector3 spawnPoint = _playerSpawnPoint != null ? _playerSpawnPoint.position : Vector3.zero;
       
        Player playerInstance=Instantiate(_playerPrefab,spawnPoint,Quaternion.identity);
        _playerController =new PlayerController(playerInstance);
        MainLoopEntry.Instance.RegisterStartable(_playerController);
        MainLoopEntry.Instance.RegisterUpdatable(_playerController);

    }

}