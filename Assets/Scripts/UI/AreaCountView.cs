using TMPro;
using UnityEngine;

public class AreaCountView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI areaCountTxt;

    public void SetAreaCount(int areaCount)
    {
        areaCountTxt.text = $"Šl“¾” {areaCount}/3" +
            $"";
    }
}
