#if UNITY_EDITOR
using UnityEditor;

public class SingleTonSOAssetBundleMenu
{
    [MenuItem("Tools/Build SingleTonSO AssetBundle")]
    public static void BuildFromMenu()
    {
        SingleTonSOAssetBundleBuilder.Build();
    }
}
#endif
