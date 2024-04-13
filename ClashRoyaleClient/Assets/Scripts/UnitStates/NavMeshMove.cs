using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;
    private bool _targetIsEnemy;
    private Tower _nearestTower;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = _unit.IsEnemy == false;

        _agent = _unit.GetComponent<NavMeshAgent>();
        if (_agent == null ) Debug.LogError($"Unit {unit.name} hasn't NavMeshAgent");

        _agent.speed = _unit.parameters.speed;
        _agent.radius = _unit.parameters.modelRadius;
        _agent.stoppingDistance = _unit.parameters.startAttackDistance;
    }

    public override void Init()
    {     
        Vector3 unitPosition = _unit.transform.position;
        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _targetIsEnemy);
        _targetPosition = _nearestTower.transform.position;

        _agent.SetDestination(_targetPosition);
}
    public override void Run()
    {
        TryAttackTower();
    }

    public void TryAttackTower()
    {
        float distanceToTarget = _nearestTower.GetDistance(_unit.transform.position);
        if (distanceToTarget <= _unit.parameters.startAttackDistance)
        {
            Debug.Log("Done");
        }
    }

    public override void Finish()
    {
        _agent.isStopped = true;
    }

}