using System;
using UnityEngine;

public class Torpedo : MovingObject
{
    public event Action<Torpedo> Died;  
    private float lifetime;

    public bool IsAlive { get {return lifetime <= Settings.TorpedoLifetime; } }

    protected override void Update()
    {
        if (!IsAlive)
        {
            gameObject.SetActive(false);
            Died?.Invoke(this);
        }
        lifetime += Time.deltaTime;
        base.Update();
    }

    internal override void Launch(Vector3 from, Vector3 to)
    {
        speed = Settings.TorpedoSpeed;
        base.Launch(from, to);
        selfTransform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        gameObject.SetActive(true);
    }

    internal override void Release()
    {
        Died = null;
        lifetime = 0;
        base.Release();
    }
}
