

using UnityEngine;

public class TestEnemy:EnemyBase
{

    public override void OnCallAI()
    {

        Vector2 dirction = _playerTransform.position - transform.position;
        Move(dirction);
    }

    private void Move(Vector2 dirction)
    {
        transform.position += (Vector3)dirction.normalized * Stataus.MoveForce * Time.deltaTime;
    }
}