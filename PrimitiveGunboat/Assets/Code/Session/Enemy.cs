using UnityEngine;

public class Enemy : MovingObject
{
    [SerializeField]
    public string GroupId;
    
    [SerializeField]
    public float Speed;

    internal override void Launch(Vector3 from, Vector3 to)
    {
        base.speed = Speed;
        base.Launch(from, to);

        gameObject.SetActive(true);
    }
}
