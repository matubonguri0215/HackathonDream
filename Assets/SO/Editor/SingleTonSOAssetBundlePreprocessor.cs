using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public class SingleTonSOAssetBundleProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string TempBundleName = SingleTonSOAssetBundleBuilder.BundleName;
    private static string OutputPath = SingleTonSOAssetBundleBuilder.OutputFolder;
    public const string OutputFolderName = SingleTonSOBundleBuildData.BuildOutputFolderName;

    public int callbackOrder => 250;

    public void OnPreprocessBuild(BuildReport report)
    {
        EditorApplication.delayCall += ()=>
        {
           BuildAssetBundle(report);
        };
    }


    private void BuildAssetBundle(BuildReport report)
    {
        // ① 既存バンドル削除
        DeleteExistingBundles();

        // ② ターゲットSOを検索して一時的にアセットバンドル名を設定
        AssignBundleNames();

       SingleTonSOAssetBundleBuilder.Build();

        // ④ 名前を元に戻す
        ResetBundleNames();

        // ⑤ ビルド出力先にコピー
        CopyToBuildFolder(report);
    }
    public void OnPostprocessBuild(BuildReport report)
    {

       
    }


    private void DeleteExistingBundles()
    {
        if (Directory.Exists(OutputPath))
        {
            Directory.Delete(OutputPath, true);
        }
        Directory.CreateDirectory(OutputPath);
    }

    private void AssignBundleNames()
    {
        var guids = AssetDatabase.FindAssets("t:SingleTonSO");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = TempBundleName;
        }
        AssetDatabase.SaveAssets();
    }

    private void BuildBundle()
    {
        BuildPipeline.BuildAssetBundles(
            OutputPath,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );
    }

    private void ResetBundleNames()
    {
        var guids = AssetDatabase.FindAssets("t:SingleTonSO");
        foreach (var guid in guids)
        {
            var importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
            importer.assetBundleName = string.Empty;
        }
        AssetDatabase.SaveAssets();
    }

    private void CopyToBuildFolder(BuildReport report)
    {
        string dst = Path.Combine(Path.GetDirectoryName(report.summary.outputPath), OutputFolderName);
        if (!Directory.Exists(dst))
        {
            Directory.CreateDirectory(dst);
        }
           

        var files = Directory.GetFiles(OutputPath);
        
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            string path = Path.Combine(dst, name);
            Debug.Log(path);
            File.Copy(file, path, true);
        }
    }

    
}


