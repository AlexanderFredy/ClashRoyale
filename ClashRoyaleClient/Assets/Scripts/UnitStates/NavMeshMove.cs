using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NavMeshMove", menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    [SerializeField] private float _moveOffset = 0.5f;
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;

    public override void Init()
    {
        _agent = _unit.GetComponent<NavMeshAgent>();
        _targetPosition = Vector3.forward;
        _agent.SetDestination(_targetPosition);
    }
    public override void Run()
    {
        float distanceToTarget = Vector3.Distance(_unit.transform.position, _targetPosition);
        if (distanceToTarget <= _moveOffset) { Debug.Log("Done"); }
        
    }

    public override void Finish()
    {
        _agent.isStopped = true;
    }

}