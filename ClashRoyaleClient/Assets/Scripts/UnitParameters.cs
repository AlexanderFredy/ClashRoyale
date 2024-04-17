using UnityEngine;

public class UnitParametrs : MonoBehaviour
{
    [field: SerializeField] public bool ifFlying { get; private set; } = false;
    [field: SerializeField] public float speed { get; private set; } = 2.5f;
    [field:SerializeField] public float modelRadius { get; private set; } = 0.5f;
    [field:SerializeField] public float startChaseDistance { get; private set; } = 2f;
    [field:SerializeField] public float stopChaseDistance { get; private set; } = 3.5f;
    public float startAttackDistance { get { return modelRadius + _startAttackDistance; } }
    public float stopAttackDistance { get { return modelRadius + _stopAttackDistance; } }

    [SerializeField] public float _startAttackDistance = 1f;
    [SerializeField] public float _stopAttackDistance = 1.5f;
}
