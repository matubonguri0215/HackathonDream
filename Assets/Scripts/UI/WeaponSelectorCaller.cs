using Input;
using UnityEngine;

public class WeaponSelectorCaller : MonoBehaviour
{
    [SerializeField]
    private RadialMenu weaponSelectorMenu;

    private void Start()
    {
        if (weaponSelectorMenu == null)
        {
            Debug.LogError("WeaponSelectorMenu is not assigned in the inspector.");
        }
        InputManager.SubscribeInput(ActionType.WeaponSelector, (input) =>
        {
            weaponSelectorMenu.Show();
        });
    }
}