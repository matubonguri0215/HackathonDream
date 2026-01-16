using UnityEngine;

public abstract class WeaponBase:MonoBehaviour
{

    protected abstract float ChargeTime { get; }

    public virtual void OnCallShot()
    {

    }


}