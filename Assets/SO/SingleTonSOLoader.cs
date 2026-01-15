using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SingleTonSOLoader
{
    private const string BundleName = SingleTonSOBundleBuildData.BundleName;
    private static AssetBundle _bundle;
#if UNITY_EDITOR
    [MenuItem("Tools/TestLoad")]
    public static void TestLoad()
    {
        ScriptableObject[] assets = LoadAll();
        foreach (var asset in assets)
        {
            Debug.Log($"Loaded Asset: {asset.name} of Type: {asset.GetType().Name}");
        }
    }
#endif

    /// <summary>
    /// SingleTonSO をロード
    /// </summary>
    public static T Load<T>() where T : ScriptableObject
    {
        if (_bundle == null)
        {
            string path = GetBundlePath();
            Debug.Log($"Loading AssetBundle from: {path}");
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android は StreamingAssets 内が圧縮APKなので UnityWebRequest を使う
            _bundle = LoadBundleAndroid(path);
#else
            _bundle = AssetBundle.LoadFromFile(path);
#endif
            if (_bundle == null)
            {
                Debug.LogError($"AssetBundle がロードできません: {path}");
                return null;
            }
        }
        T res=_bundle.LoadAsset<T>(typeof(T).Name);
        if(res==null)
        {
            res=_bundle.LoadAllAssets<T>()[0];
        }
        // 型名とアセット名が一致している場合
        return res;
    }

    public static ScriptableObject[] LoadAll()
    {
        if (_bundle == null)
        {
            string path = GetBundlePath();
            Debug.Log($"Loading AssetBundle from: {path}");
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android は StreamingAssets 内が圧縮APKなので UnityWebRequest を使う
            _bundle = LoadBundleAndroid(path);
#else
            _bundle = AssetBundle.LoadFromFile(path);
#endif
            if (_bundle == null)
            {
                Debug.LogError($"AssetBundle がロードできません: {path}");
                return null;
            }
        }
        return _bundle.LoadAllAssets<ScriptableObject>();
    }

    /// <summary>
    /// アセットバンドルのパスをプラットフォームに応じて取得
    /// </summary>
    private static string GetBundlePath()
    {
#if UNITY_EDITOR
        return Path.Combine(SingleTonSOAssetBundleBuilder.OutputFolder, BundleName);
#elif UNITY_STANDALONE
        return Path.Combine(Application.dataPath, $"../{SingleTonSOBundleBuildData.BuildOutputFolderName}", BundleName);
#elif UNITY_IOS
        return Path.Combine(Application.streamingAssetsPath, BundleName);
#elif UNITY_ANDROID
        return Path.Combine(Application.streamingAssetsPath, BundleName);
#else
        return Path.Combine(Application.streamingAssetsPath, BundleName);
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AssetBundle LoadBundleAndroid(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(path))
        {
            var operation = uwr.SendWebRequest();
            while (!operation.isDone) { } // 簡易待機、コルーチン推奨
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Android AssetBundle ロード失敗: {uwr.error}");
                return null;
            }
            return DownloadHandlerAssetBundle.GetContent(uwr);
        }
    }
#endif

    /// <summary>
    /// バンドルアンロード
    /// </summary>
    public static void UnloadBundle(bool unloadAllLoadedObjects = false)
    {
        if (_bundle != null)
        {
            _bundle.Unload(unloadAllLoadedObjects);
            _bundle = null;
        }
    }
}
