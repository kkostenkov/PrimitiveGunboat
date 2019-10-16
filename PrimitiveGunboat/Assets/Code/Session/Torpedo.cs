using System;
using UnityEngine;

public class Torpedo : MovingObject
{
    public event Action<Torpedo> Exploded;  

    internal override void Launch(Vector3 from, Vector3 to)
    {
        speed = Settings.TorpedoSpeed;
        base.Launch(from, to);
        selfTransform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        gameObject.SetActive(true);
    }

    internal override void Release()
    {
        Exploded = null;
        base.Release();
    }

    public int collisionLayersMask = Constants.ENEMY_LAYER;

    private void OnTriggerStay(Collider other)
    {
        if (!CheckCollisionTrigger(other.gameObject.layer))
        {
            return;       
        }
        var damageTaker = other.GetComponent<IDamageTaker>();

        if (damageTaker == null)
        {
            return;
        }

        damageTaker.TakeDamage(Constants.TORPEDO_DAMAGE);

        Exploded?.Invoke(this);
    }

    private bool CheckCollisionTrigger(int otherLayer)
    {
        return (collisionLayersMask & otherLayer) == otherLayer;
    }
}
