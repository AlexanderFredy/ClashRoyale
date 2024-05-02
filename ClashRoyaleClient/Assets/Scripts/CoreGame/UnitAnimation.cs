using UnityEngine;
using UnityEngine.VFX;

public class UnitAnimation : MonoBehaviour
{
    private const string State = "State";
    private const string AttackSpeed = "AttackSpeed";
    [SerializeField] private Animator _animator;

    [SerializeField] protected Transform _attackEffectGO;
    public VisualEffect AttackEffect { get; private set; }

    public void Init(Unit unit)
    {
        float damageDelay = unit.parameters.damageDalay;
        _animator.SetFloat(AttackSpeed, 1/damageDelay);

        if (_attackEffectGO)
        {
            AttackEffect = _attackEffectGO.GetComponent<VisualEffect>();
            AttackEffect.Stop();

            if (!AttackEffect)
                print($"For attack effect {_attackEffectGO} don't setted visual effect");
        }

    }
    public void SetState(UnitStateType stateType)
    {
        _animator.SetInteger(State, (int)stateType);
    }
}
