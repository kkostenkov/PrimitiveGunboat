using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected Transform selfTransform;
    protected Vector3 movementDirection;
    protected float speed;
    
    void Awake()
    {
        selfTransform = GetComponent<Transform>();
    }

    internal virtual void Launch(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;
        selfTransform.position = from;
        var distance = to - from;
        movementDirection = new Vector3(distance.x, 0, distance.z).normalized;
    }

    protected virtual void Update()
    {
        selfTransform.position = selfTransform.position + movementDirection * speed * Time.deltaTime;
    }
}
