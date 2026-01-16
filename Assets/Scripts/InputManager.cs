using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Input
{
    public class InputManager : MonoSingleton<InputManager>
    {

        private static ReactiveProperty<bool> isShotInput = new ReactiveProperty<bool>(false);
        private static ReactiveProperty<bool> isBloodInput = new ReactiveProperty<bool>(false);
        private static ReactiveProperty<bool> isAimLockInput = new ReactiveProperty<bool>(false);
        private static ReactiveProperty<bool> isWeaponSelectorInput = new ReactiveProperty<bool>(false);

        public static Vector2 GetAxisLeft()
        {
            Vector2 keyboardInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) keyboardInput.y += 1;
            if (Keyboard.current.sKey.isPressed) keyboardInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) keyboardInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) keyboardInput.x += 1;
            Vector2 gamepadInput = Gamepad.current != null ? Gamepad.current.leftStick.ReadValue() : Vector2.zero;
            return keyboardInput.normalized + gamepadInput;
        }
        public static Vector2 GetAxisRight()
        {
            if (Mouse.current != null)
            {
                // 画面中央のワールド座標
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, Camera.main.nearClipPlane));

                // マウスのワールド座標
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

                // 画面中央からマウスへの方向ベクトル
                Vector2 mouseInput = ((Vector2)mouseWorld - (Vector2)worldCenter).normalized;

                Vector2 gamepadInput = Gamepad.current != null ? Gamepad.current.rightStick.ReadValue() : Vector2.zero;

                // どちらか入力されている方を優先
                return mouseInput.magnitude > 0.01f ? mouseInput : gamepadInput;
            }

            return Gamepad.current != null ? Gamepad.current.rightStick.ReadValue() : Vector2.zero;
        }


        public static void SubscribeInput(ActionType action, System.Action<bool> onChanged)
        {
            switch (action)
            {
                case ActionType.Shot:
                    isShotInput.Subscribe(onChanged);
                    break;
                case ActionType.Blood:
                    isBloodInput.Subscribe(onChanged);
                    break;
                case ActionType.AimLock:
                    isAimLockInput.Subscribe(onChanged);
                    break;
                case ActionType.WeaponSelector:
                    isWeaponSelectorInput.Subscribe(onChanged);
                    break;
            }
        }
        private void Update()
        {
            isShotInput.Value = IsPressed(ActionType.Shot);
            isBloodInput.Value = IsPressed(ActionType.Blood);
            isAimLockInput.Value = IsPressed(ActionType.AimLock);
            isWeaponSelectorInput.Value = IsPressed(ActionType.WeaponSelector);
        }

        /// <summary>
        /// 瞬間入力返す。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool WasPressed(ActionType action)
        {
            return action switch
            {
                ActionType.Shot =>
                    Mouse.current.leftButton.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame,
                ActionType.Blood =>
                    Mouse.current.rightButton.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame,
                ActionType.AimLock =>
                    Mouse.current.middleButton.wasPressedThisFrame || Gamepad.current.rightStickButton.wasPressedThisFrame,
                ActionType.WeaponSelector =>
                    Keyboard.current.spaceKey.wasPressedThisFrame,
                ActionType.Weapon1 =>
                    Keyboard.current.digit1Key.wasPressedThisFrame,
                ActionType.Weapon2 =>
                    Keyboard.current.digit2Key.wasPressedThisFrame,
                ActionType.Weapon3 =>
                    Keyboard.current.digit3Key.wasPressedThisFrame,
                ActionType.Weapon4 =>
                    Keyboard.current.digit4Key.wasPressedThisFrame,

                _ => false,
            };
        }
        public static bool IsPressed(ActionType action)
        {
            return action switch
            {
                ActionType.Shot =>
                    Mouse.current.leftButton.isPressed || Gamepad.current.rightTrigger.isPressed,
                ActionType.Blood =>
                    Mouse.current.rightButton.isPressed || Gamepad.current.leftTrigger.isPressed,
                ActionType.AimLock =>
                    Mouse.current.middleButton.isPressed || Gamepad.current.rightStickButton.isPressed,
                ActionType.WeaponSelector =>
                    Keyboard.current.spaceKey.isPressed,
                ActionType.Weapon1 =>
                    Keyboard.current.digit1Key.isPressed,
                ActionType.Weapon2 =>
                    Keyboard.current.digit2Key.isPressed,
                ActionType.Weapon3 =>
                    Keyboard.current.digit3Key.isPressed,
                ActionType.Weapon4 =>
                    Keyboard.current.digit4Key.isPressed,
                _ => false,
            };
        }
        public static bool WasReleased(ActionType action)
        {
            return action switch
            {
                ActionType.Shot =>
                    Mouse.current.leftButton.wasReleasedThisFrame || Gamepad.current.rightTrigger.wasReleasedThisFrame,
                ActionType.Blood =>
                    Mouse.current.rightButton.wasReleasedThisFrame || Gamepad.current.leftTrigger.wasReleasedThisFrame,
                ActionType.AimLock =>
                    Mouse.current.middleButton.wasReleasedThisFrame || Gamepad.current.rightStickButton.wasReleasedThisFrame,
                ActionType.WeaponSelector =>
                    Keyboard.current.spaceKey.wasReleasedThisFrame,
                ActionType.Weapon1 =>
                    Keyboard.current.digit1Key.wasReleasedThisFrame,
                ActionType.Weapon2 =>
                    Keyboard.current.digit2Key.wasReleasedThisFrame,
                ActionType.Weapon3 =>
                    Keyboard.current.digit3Key.wasReleasedThisFrame,
                ActionType.Weapon4 =>
                    Keyboard.current.digit4Key.wasReleasedThisFrame,
                _ => false,
            };
        }
    }


}


public enum ActionType
{
    Shot,
    Blood,
    AimLock,
    WeaponSelector,
    Weapon1,
    Weapon2,
    Weapon3,
    Weapon4,
}

