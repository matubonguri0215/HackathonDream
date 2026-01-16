using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// シーンのロードを管理するScriptableObject
/// シングルトンとして機能し、エディタ上でシーンアセットを設定可能
/// </summary>
[CreateAssetMenu(fileName = "SceneLoaderSO", menuName = "SingleTonSO/Create SceneLoaderSO")]
public class SceneLoaderSO : SingleTonSO<SceneLoaderSO>
{
#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset _battleScene;
    [SerializeField]
    private SceneAsset _fieldScene;
    [SerializeField]
    private SceneAsset _titleScene;

#endif

    [SerializeField, HideInInspector]
    private string _battleSceneName;
    [SerializeField, HideInInspector]
    private string _fieldSceneName;
    [SerializeField, HideInInspector]
    private string _titleSceneName;
    public string BattleSceneName => _battleSceneName;
    public string FieldSceneName => _fieldSceneName;
    public string TitleSceneName => _titleSceneName;

    private string _additiveSceneName = default;

    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;
    public event Action<string> OnCompletedSceneLoadAdditive;
    public event Action<string> OnActiveSceneChanged;

    


    public async Task LoadScene(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName);
        OnSceneLoaded?.Invoke(sceneName);
    }

    public async void UnloadScene(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
        OnSceneUnloaded?.Invoke(sceneName);
    }

    public async Task LoadSceneAdditive(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _additiveSceneName = sceneName;
        OnCompletedSceneLoadAdditive?.Invoke(sceneName);
        Debug.Log($"Set Active Scene to {_additiveSceneName}");

    }

    public async void UnloadAdditiveScene(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
        OnSceneUnloaded.Invoke(sceneName);
    }

    public void SetActiveAdditiveScene()
    {
        
        ChangeActiveScene(_additiveSceneName);
        _additiveSceneName = default;
    }
    public void SetActiveFalseScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public void ChangeActiveScene(string sceneName)
    {
        Debug.Log($"Changing Active Scene to {sceneName}");
        Scene actScene = SceneManager.GetSceneByName(sceneName);
        if (actScene == null)
        {
            Debug.LogError($"Scene {sceneName} not found!");
            return;
        }

        bool isActive= SceneManager.SetActiveScene(actScene);
        if (isActive)
        {
            OnActiveSceneChanged?.Invoke(sceneName);
            Debug.Log($"Active Scene changed to {actScene.name}");
        }
        else
        {
            Debug.LogError($"Failed to change active scene to {actScene.name}");
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_battleScene != null)
        {
            _battleSceneName = _battleScene.name;
        }
        if (_fieldScene != null)
        {
            _fieldSceneName = _fieldScene.name;
        }
        if (_titleScene != null)
        {
            _titleSceneName = _titleScene.name;
        }
    }


#endif
}
