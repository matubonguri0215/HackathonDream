#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public static class SingleTonSOAssetBundleBuilder
{
    public const string BundleName = SingleTonSOBundleBuildData.BundleName;
    public const string OutputFolder = SingleTonSOBundleBuildData.OutputFolder;

    public static void Build()
    {
        // 1) プロジェクト内のすべての ScriptableObject のパスを取得
        string[] allAssetGUIDs = AssetDatabase.FindAssets("t:ScriptableObject");
        string[] singleTonSOAssets = allAssetGUIDs
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path =>
            {
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                return asset != null && IsDerivedFromSingleTonSO(asset);
            })
            .ToArray();

        if (singleTonSOAssets.Length == 0)
        {
            Debug.LogWarning("SingleTonSO を継承したアセットが見つかりませんでした。");
            return;
        }

        // 2) 一時的にアセットバンドル名を付与
        foreach (string path in singleTonSOAssets)
        {
            AssetImporter importer = AssetImporter.GetAtPath(path);
            Debug.Log($"Assigning bundle name to asset: {path}");
            importer.assetBundleName = BundleName;
        }

        // 3) 出力フォルダを準備
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }

        // 4) アセットバンドルビルド
        BuildPipeline.BuildAssetBundles(
            OutputFolder,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );

        Debug.Log("SingleTonSO アセットバンドルを出力しました: " + OutputFolder);

        // 5) バンドル名を元に戻す（不要ならコメントアウト）
        foreach (var path in singleTonSOAssets)
        {
            var importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = string.Empty;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static bool IsDerivedFromSingleTonSO(ScriptableObject obj)
    {
        var type = obj.GetType();
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SingleTonSO<>))
            {
                Debug.Log($"Found SingleTonSO derived type: {type.FullName}:{obj.name}");
                return true;
            }               
            type = type.BaseType;
        }
        return false;
    }
}
#endif
