using System;
using UnityEngine;

public class MovingObject : VisibleObject
{
    protected Transform selfTransform;
    protected Vector3 movementDirection;
    protected float speed;
    public event Action<MovingObject> BoundsBroken;
    protected IScreenBoundsSchecker boundsChecker;
    
    void Awake()
    {
        selfTransform = GetComponent<Transform>();
    }

    public void SetBounds(IScreenBoundsSchecker boundsChecker)
    {
        this.boundsChecker = boundsChecker;
    }

    internal virtual void Launch(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;
        selfTransform.position = from;
        var distance = to - from;
        movementDirection = new Vector3(distance.x, 0, distance.z).normalized;
    }

    internal virtual void Release()
    {
        BoundsBroken = null;
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        selfTransform.position = selfTransform.position + movementDirection * speed * Time.deltaTime;
        if (BoundsBroken == null)
        {
            return;
        }

        var isInBounds = boundsChecker.ValidateBounds(selfTransform.position);
        if (!isInBounds)
        {
            BoundsBroken?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        BoundsBroken = null;
        boundsChecker = null;
    }
}
