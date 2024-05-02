using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<float> UpdateHealth;

    [field: SerializeField] public float max { get; private set; } = 10f;
    public float Current { get; private set; }

    private void Start()
    {
        Current = max;
    }

    public void ApplyDamage(float value)
    {
        Current -= value;
 
        if (Current < 0) Current = 0;

        UpdateHealth?.Invoke(Current);
    }

    public void ApplyDelayDamage(float delay, float damage)
    {
        StartCoroutine(DelayDamage(delay, damage));
    }

    private IEnumerator DelayDamage(float delay, float damage)
    {
        yield return new WaitForSeconds(delay);
        ApplyDamage(damage);
    }
}

public interface IHealth
{
    Health health { get;}
}
