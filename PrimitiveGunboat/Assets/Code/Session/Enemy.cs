using System;
using UnityEngine;

public class Enemy : MovingObject, IDamageTaker
{
    [SerializeField]
    private string groupId;
    public string GroupId { get { return groupId; } }
    
    [SerializeField]
    private float Speed;

    [SerializeField]
    private int MaxHp;

    [SerializeField]
    private int Damage;
    [SerializeField]
    private int pointsValue;
    public int PointsValue { get {return pointsValue;} }

    public event Action<IDamageTaker> Killed;
    public event Action<MovingObject> Crashed;

    private int currentHp;

    internal override void Launch(Vector3 from, Vector3 to)
    {
        currentHp = MaxHp;
        base.speed = Speed;
        base.Launch(from, to);

        gameObject.SetActive(true);
    }

    internal override void Release()
    {
        Killed = null;
        Crashed = null;
        base.Release();
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Killed?.Invoke(this);
        }
    }

    private int collisionLayersMask = Constants.SPACE_STATION_LAYER;

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

        damageTaker.TakeDamage(Damage);

        Crashed?.Invoke(this);
    }

    private bool CheckCollisionTrigger(int otherLayer)
    {
        return (collisionLayersMask & otherLayer) == otherLayer;
    }
}

