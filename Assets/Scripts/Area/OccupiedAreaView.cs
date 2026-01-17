using System;
using Unity.VisualScripting;
using UnityEngine;

public class OccupiedAreaView : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer = default;
    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }

}

[System.Serializable]
public class AreaInfo
{
    [SerializeField]
    private float _occupationTime = default;

    private float _currentOccupationTime = default;

    [SerializeField]
    private int _healLife = default;

    [SerializeField]
    private float _healTiming = default;

    [SerializeField]
    private Color _occupiedColor = default;

    [SerializeField]
    private LayerMask _playerLayer = default;

    /// <summary>
    /// エリアが変わったときに呼ばれるイベント
    /// </summary>
    public event Action OnChangeArea;

    /// <summary>
    /// 占有までの時間更新イベント
    /// <br/>
    /// 一個目は変更している時間、二個目は最大時間だよ
    /// </summary>
    public event Action<float, float> OnChangeOccupationTime;

    public Color OccupiedColor => _occupiedColor;

    public LayerMask PlayerLayer => _playerLayer;

    public void CheckTime()
    {
        _currentOccupationTime -= Time.deltaTime;
        if (_currentOccupationTime <= 0)
        {
            OnChangeArea?.Invoke();
        }
        OnChangeOccupationTime?.Invoke(_currentOccupationTime, _occupationTime);
    }

}

public class PlayerOccupationChecker
{
    public bool CheckPlayer(LayerMask playerLayer, Vector2 origin, float areaRadius)
    {
        if (Physics2D.OverlapCircle(origin, areaRadius, playerLayer))
        {
            return true;
        }
        return false;
    }
}

public class AreaController : MonoBehaviour
{
    [SerializeField]
    private OccupiedAreaView[] _occupiedAreaViews = default;

    [SerializeField]
    private AreaInfo _areaInfo = default;

    private int _currentAreaIndex = default;

    private int _currentOccupiedArea = default;

    private PlayerOccupationChecker _playerChecker = default;

    /// <summary>
    /// エリアが変わったときに呼ばれるイベント
    /// </summary>
    public event Action<Transform> OnChangeNextArea;

    /// <summary>
    /// 占有までの時間更新イベント
    /// <br/>
    /// 一個目は変更している時間、二個目は最大時間だよ
    /// </summary>
    public event Action<float, float> OnChangeOccupationTime
    {
        add { _areaInfo.OnChangeOccupationTime += value; }
        remove { _areaInfo.OnChangeOccupationTime -= value; }
    }

    public event Action<int> OnChangeOccupiedArea;

    private void Awake()
    {
        _areaInfo.OnChangeArea += ChangeArea;
    }

    private void Update()
    {
        OccupiedAreaView _currentView = _occupiedAreaViews[_currentAreaIndex];
        if (_playerChecker.CheckPlayer(_areaInfo.PlayerLayer, _currentView.transform.position, _currentView.transform.lossyScale.x / 2))
        {
            _areaInfo.CheckTime();
        }
    }

    private void ChangeArea()
    {
        _currentOccupiedArea++;
        _occupiedAreaViews[_currentAreaIndex].ChangeColor(_areaInfo.OccupiedColor);
        OnChangeOccupiedArea?.Invoke(_currentOccupiedArea);
        OnChangeNextArea?.Invoke(_occupiedAreaViews[_currentAreaIndex + 1].transform);
    }

}