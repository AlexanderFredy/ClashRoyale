using System;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Health), typeof(NetworkIdentity), typeof(NetworkTransformUnreliable))]
public class Tower : NetworkBehaviour, IHealth, IDestroyed
{
    public event Action Destroyed;

    [field: SerializeField] public Health health { get; private set; }
    [field: SerializeField] public float radius { get; private set; } = 2f;


    private void Start()
    {
        health.UpdateHealth += CheckDestroy;
    }

    private void CheckDestroy(float currentHP)
    {
        if (currentHP > 0) return;

        Destroy(gameObject);
        health.UpdateHealth -= CheckDestroy;
        Destroyed?.Invoke();
    }

    public float GetDistance(in Vector3 point) => Vector3.Distance(transform.position, point) - radius;
}
