using TMPro;
using UnityEngine;

public class AreaCountView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI areaCountTxt;
    [SerializeField]
    private AreaController areaController;

    private void Start()
    {
        areaController.OnChangeOccupiedArea += SetAreaCount;
    }
    public void SetAreaCount(int areaCount)
    {
        areaCountTxt.text = $"älìæêî {areaCount}/3";
    }
}
