using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{

    [SerializeField]
    private EnemyStatus _stataus;

    public EnemyStatus Stataus
    {
        get { return _stataus; }
    }

    void IDamageable.Damage(int damage)
    {
        _stataus.HP -= damage;
    }

    public abstract void OnCallAI();
}

[SerializeField]
public class EnemyStatus:EntityStatus
{

}