using System;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public event Action<Torpedo> Died;
    private Transform selfTransform;
    private Vector3 movementDirection;
    private float lifetime;

    public bool IsAlive { get {return lifetime <= Settings.TorpedoLifetime; } }

    void Awake()
    {
        selfTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!IsAlive)
        {
            gameObject.SetActive(false);
            Died?.Invoke(this);
        }
        lifetime += Time.deltaTime;
        selfTransform.position = selfTransform.position + movementDirection * Settings.TorpedoSpeed * Time.deltaTime;
    }

    internal void Launch(Vector3 from, Vector3 to)
    {
        selfTransform.position = from;
        var distance = to - from;
        movementDirection = new Vector3(distance.x, 0, distance.z).normalized;

        selfTransform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        gameObject.SetActive(true);
    }

    internal void Defuse()
    {
        Died = null;
        gameObject.SetActive(false);
        lifetime = 0;
    }
}
