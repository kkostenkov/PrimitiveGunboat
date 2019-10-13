using System;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
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
        lifetime += Time.deltaTime;
        selfTransform.Translate(movementDirection * Settings.TorpedoSpeed * Time.deltaTime);
    }

    internal void Launch(Vector3 from, Vector3 to)
    {
        selfTransform.position = from;
        movementDirection = (to - selfTransform.position).normalized;
        gameObject.SetActive(true);
    }

    internal void Defuse()
    {
        gameObject.SetActive(false);
        lifetime = 0;
    }
}
