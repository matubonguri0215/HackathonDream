using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = MyDebugLogger;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Audio/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField]
    private List<BGMData> BGMList = new List<BGMData>();
    [SerializeField]
    private List<SEData> SEList = new List<SEData>();

    private Dictionary<BGM_ID, BGMData> _bgmDict;
    private Dictionary<SE_ID, SEData> _seDict;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _bgmDict = BGMList.ToDictionary(bgm => bgm.ID, bgm => bgm);
        _seDict = SEList.ToDictionary(se => se.ID, se => se);

        Debug.Log("サウンドデータベース初期化完了", Debug.LogCategory.Initialization);
    }

    /// <summary>
    /// BGMデータ取得
    /// </summary>
    /// <param name="id">BGM種類識別ID</param>
    /// <returns></returns>
    public BGMData GetBGMData(BGM_ID id)
    {
        if (_bgmDict == null) Initialize();
        if (_bgmDict.TryGetValue(id, out var bgmData))
        {
            return bgmData;
        }
        Debug.LogError($"BGMData with ID {id} not found.");
        return null;
    }

    /// <summary>
    /// SEデータ取得
    /// </summary>
    /// <param name="id">SE種類識別ID</param>
    /// <returns></returns>
    public SEData GetSEData(SE_ID id)
    {
        if (_seDict == null) Initialize();
        if (_seDict.TryGetValue(id, out var seData))
        {
            return seData;
        }
        Debug.LogError($"SEData with ID {id} not found.");
        return null;
    }
}
