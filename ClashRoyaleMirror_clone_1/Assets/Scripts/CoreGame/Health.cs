using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    public Action<float> UpdateHealth;

    [field: SerializeField] public float max { get; private set; } = 10f;
    
    [SyncVar(hook = nameof(HealthHook))]
    public float _current;

    private void HealthHook(float oldValue, float newValue)
    {
        UpdateHealth?.Invoke(newValue);
    }

    private void Start()
    {
        _current = max;
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
 
        if (_current < 0) _current = 0;

        UpdateHealth?.Invoke(_current);
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
