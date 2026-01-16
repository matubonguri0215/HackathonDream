
using UnityEngine;

public class PlayerMoveComponent:MonoBehaviour,IInjectable<IMoveForceGetable>
{

    private IMoveForceGetable _moveForceGetable;
    public void Inject(IMoveForceGetable moveForce)
    {
        _moveForceGetable = moveForce;
    }

    public void OnCallMove(Vector2 input, float delta)
    {
        transform.position += (Vector3)input * (_moveForceGetable.MoveForce * delta);
    }



}