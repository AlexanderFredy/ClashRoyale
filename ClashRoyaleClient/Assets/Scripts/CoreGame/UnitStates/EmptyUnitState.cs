using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "UnitState/Empty")]
public class EmptyUnitState : UnitState
{
    public override void Init()
    {
        _unit.SetState(UnitStateType.Default);
    }

    public override void Run()
    {
    }
    public override void Finish()
    {
        Debug.LogWarning($"Unit {_unit.name} had empty state, it has default state now");
    }

}
