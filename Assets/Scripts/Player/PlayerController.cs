using Input;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController:MonoBehaviour
{
    private Player _player;
    public PlayerController(Player player)
    {
        _player = player;
       
    }
    private PlayerController() { }

    private void Awake()
    {
        _player.Init();
    }
    private void Update()
    {
        OnUpdateController();
    }


    private void OnUpdateController()
    {
        bool isShot = InputManager.IsPressed(ActionType.Shot);
        bool isCharge = InputManager.IsPressed(ActionType.Blood);
        Vector2 input = InputManager.Instance.GetAxis();
        Controll(input, isShot, isCharge);
    }

    //毎フレーム呼び出しなのでインライン化
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Controll(Vector2 input, bool isShot, bool isCharge)
    {
        _player.Controll(input, isShot, isCharge);
    }
}