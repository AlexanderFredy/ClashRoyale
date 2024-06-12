using UnityEngine;

[CreateAssetMenu(fileName = "MeleeChase", menuName = "UnitState/MeleeChase")]
public class MeleeChase : UnitStateChase
{
    protected override void FindTargetUnit(out Unit targetUnit)
    {
        _unit.mapInfo.TryGetNearestWalkingUnit(_unit.transform.position, _targetIsEnemy, out targetUnit, out float distance);
    }
}