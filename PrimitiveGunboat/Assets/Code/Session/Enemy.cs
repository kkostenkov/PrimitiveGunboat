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

    public event Action<IDamageTaker> Died;

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
        Died = null;
        base.Release();
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Died?.Invoke(this);
        }
    }
}

