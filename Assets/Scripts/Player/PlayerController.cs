using Input;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController:IStartable, IUpdatable
{
    private Player _player;
    public PlayerController(Player player)
    {
        _player = player;
       
    }
    private PlayerController() { }

    ~PlayerController()
    {
        MainLoopEntry.Instance.UnregisterStartable(this);
        MainLoopEntry.Instance.UnregisterUpdatable(this);
    }
    void IStartable.OnStart()
    {
        _player.Init();
    }
   

    void IUpdatable.OnUpdate()
    {
        OnUpdateController();
    }


    private void OnUpdateController()
    {
        bool isShot = InputManager.IsPressed(ActionType.Shot);
        bool isCharge = InputManager.IsPressed(ActionType.Blood);
        Vector2 leftInput = InputManager.GetAxisLeft();
        Vector2 rightInput = InputManager.GetAxisRight();
        Controll(leftInput,rightInput,isShot, isCharge);
    }

    //毎フレーム呼び出しなのでインライン化
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Controll(Vector2 inputL,Vector2 inputR, bool isShot, bool isCharge)
    {
        _player.Controll(inputL, inputR,isShot, isCharge);
    }
}