using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Tower : MonoBehaviour, IHealth, IDestroyed
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
