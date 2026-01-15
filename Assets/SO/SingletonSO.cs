using UnityEditor;
using UnityEngine;

public abstract class SingleTonSO<T> : ScriptableObject where T : SingleTonSO<T>
{
    private static T _instance;


    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR

                // 既存を探す（アセット検索）
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                }
                Debug.Log($"instance isNull?{_instance==null} Type={typeof(T).ToString()}");
#else
                // ビルド後はアセットバンドルからロード
                _instance = SingleTonSOLoader.Load<T>();
                Debug.LogWarning("ビルド後はSingleTonSOLoaderを使用してアセットバンドルからロードしてください");
                Debug.Log($"instance isNull?{_instance==null} Type={typeof(T).ToString()}");

#endif
            }
            return _instance;
        }
        set
        {
            if (_instance == null)
            {
                _instance = value;
            }
            else
            {
                Debug.LogError("SOシングルトンインスタンスが既に存在します");
            }
        }
    }

    public SingleTonSO()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
    }
}
