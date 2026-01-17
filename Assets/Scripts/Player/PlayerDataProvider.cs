

using UnityEngine;

public class PlayerDataProvider
{
    private Player _player;

    private bool _isInit = false;

    private static PlayerDataProvider _instance = null;
    private static PlayerDataProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerDataProvider();
            }
            return _instance;
        }
    }

    

    private PlayerDataProvider()
    {
        SceneLoaderSO.Instance.OnActiveSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(string _)
    {
        _instance = null;
    }
    public static void InitProvider(Player player)
    {
        if (Instance._isInit) return;
        Instance._player=player;
        MyDebugLogger.Log("PlayerDataProvider Init",MyDebugLogger.LogCategory.Initialization);
        Instance._isInit = true;
    }

    public static Player GetPlayer()
    {
        return Instance._player;
    }
    

}