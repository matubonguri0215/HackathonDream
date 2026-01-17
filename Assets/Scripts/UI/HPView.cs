using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HPView : MonoBehaviour
{
    [SerializeField]
    private Image hpSlider;
    [SerializeField]
    private TextMeshProUGUI hpTxt;
    [SerializeField]
    private UnityEvent onHeal;
    [SerializeField]
    private UnityEvent onDamage;

    [SerializeField]
    private float damageColorDuration = 0.5f;
    [SerializeField]
    private Color onDamageColor;
    private Color originalColor;

    private int maxHP = 100;

    private void Start()
    {
        originalColor = hpTxt.color;
        onDamage.AddListener(PlayFlashDamage);
        PlayerStatus status = PlayerDataProvider.GetPlayer().PlayerStatus;
        status.OnHPChanged += SetHP;
        maxHP = status.MaxHP;
    }
    private void PlayFlashDamage()
    {
        StartCoroutine(FlashDamageColor());
    }
    public void SetHP(int currentHP)
    {
        hpSlider.fillAmount = (float)currentHP / maxHP;
        hpTxt.text = $"{currentHP}";
        onHeal?.Invoke();
        onDamage?.Invoke();
    }

    private IEnumerator FlashDamageColor()
    {
        hpTxt.color = Color.red;
        yield return new WaitForSeconds(damageColorDuration);
        hpTxt.color = originalColor;
    }
}
