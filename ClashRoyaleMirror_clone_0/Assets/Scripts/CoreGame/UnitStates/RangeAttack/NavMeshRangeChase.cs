using UnityEngine;

[CreateAssetMenu(fileName = "NavMeshRangeChase", menuName = "UnitState/NavMeshRangeChase")]
public class NavMeshRangeChase : UnitStateChase
{
    protected override void FindTargetUnit(out Unit targetUnit)
    {
        _unit.mapInfo.TryGetNearestAnyUnit(_unit.transform.position, _targetIsEnemy, out targetUnit, out float distance);      
    }
}
