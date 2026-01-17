using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerView :MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

    [SerializeField]
    private Image timerImage;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private void Start()
    {
        if (gameTimer == null)
        {
            Debug.LogError("GameTimer is not assigned in the inspector.");
            return;
        }
        gameTimer.OnTimeChanged += UpdateTimerView;
        UpdateTimerView(0f);
    }
    private void UpdateTimerView(float elapsedTime)
    {
        float timeLimit = gameTimer.GetTimeLimit();
        float remainingTime = Mathf.Max(0f, timeLimit - elapsedTime);
        // Update timer text
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        // Update timer image fill amount
        timerImage.fillAmount = remainingTime / timeLimit;
    }

}
