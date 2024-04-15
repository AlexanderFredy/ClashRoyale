using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float max { get; private set; } = 10f;
    public float Current { get; private set; }

    private void Start()
    {
        Current = max;
    }

    public void ApplyDamage(float value)
    {
        Current -= value;
        //print($"Object {name}: was {Current + value}, is {Current}");

        if (Current < 0)
        {
            Current = 0;

            if (TryGetComponent(out Unit unit))
                MapInfo.Instance.RemoveUnitFromList(unit);
            else if (TryGetComponent(out Tower tower))
                MapInfo.Instance.RemoveTowerFromList(tower);

            Destroy(gameObject);
        }
    }
}

interface IHealth
{
    Health health { get;}
}
