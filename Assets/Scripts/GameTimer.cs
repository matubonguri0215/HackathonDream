using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private bool isRunning = true;
    [SerializeField]
    private float timeLimit = 60f;
    private float elapsedTime = 0f;

    private event Action<float> onTimeChanged;
    public event Action<float> OnTimeChanged
    {
        add { onTimeChanged += value; }
        remove { onTimeChanged -= value; }
    }

    private event Action onTimeUp;
    public event Action OnTimeUp
    {
        add { onTimeUp += value; }
        remove { onTimeUp -= value; }
    }
    private void Update()
    {
        if (!isRunning) return;
        elapsedTime += Time.deltaTime;
        onTimeChanged?.Invoke(elapsedTime);
        if (elapsedTime >= timeLimit)
        {
            isRunning = false;
            onTimeUp?.Invoke();
        }
    }
    /// <summary>
    /// 経過時間float単位で取得します。
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
    /// <summary>
    /// 経過時間を秒単位で取得します。
    /// </summary>
    /// <returns></returns>
    public int GetElapsedTimeInSeconds()
    {
        return Mathf.FloorToInt(elapsedTime);
    }
    /// <summary>
    /// 時間制限を取得します。
    /// </summary>
    /// <returns></returns>
    public float GetTimeLimit()
    {
        return timeLimit;
    }
    /// <summary>
    /// タイマーをリセットします。
    /// </summary>
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}
