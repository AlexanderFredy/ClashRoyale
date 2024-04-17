using System.Collections.Generic;
using UnityEngine;

public class MapInfo : Singleton<MapInfo>
{
    [SerializeField] private List<Tower> _enemyTowers;
    [SerializeField] private List<Tower> _playersTowers;

    [SerializeField] private List<Unit> _enemyWalkingUnits;
    [SerializeField] private List<Unit> _playersWalkingUnits;
    [SerializeField] private List<Unit> _enemyFlyingUnits;
    [SerializeField] private List<Unit> _playersFlyingUnits;

    private void Start()
    {
        //_enemyTowers = new List<Tower>();
        //_playersTowers = new List<Tower>();
        //_enemyWalkingUnits = new List<Unit>();
        //_playersWalkingUnits = new List<Unit>();
        //_enemyFlyingUnits = new List<Unit>();
        //_playersFlyingUnits = new List<Unit>();

        SubscribeDestroy(_enemyTowers);
        SubscribeDestroy(_playersTowers);
        SubscribeDestroy(_enemyWalkingUnits);
        SubscribeDestroy(_playersWalkingUnits);

    }

    public void AddUnit(Unit unit)
    {
        List<Unit> list;
        if (unit.IsEnemy) list = unit.parameters.ifFlying ? _enemyFlyingUnits : _enemyWalkingUnits;
        else list = unit.parameters.ifFlying ? _playersFlyingUnits : _playersWalkingUnits;

        AddObjectToList(list, unit);
    }

    public bool TryGetNearestAnyUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        TryGetNearestWalkingUnit(currentPosition, enemy, out Unit walking, out float walkingDistance);
        TryGetNearestFlyingUnit(currentPosition, enemy, out Unit flying, out float flyingDistance);

        if (flyingDistance < walkingDistance)
        {
            unit = flying;
            distance = flyingDistance;
        } else
        {
            unit = walking;
            distance = walkingDistance;
        }

        return unit;
    }

    public bool TryGetNearestWalkingUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyWalkingUnits : _playersWalkingUnits;
        unit = GetNearest(currentPosition, units, out distance);
        return unit;
    }

    public bool TryGetNearestFlyingUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyFlyingUnits : _playersFlyingUnits;
        unit = GetNearest(currentPosition, units, out distance);
        return unit;
    }

    public Tower GetNearestTower(in Vector3 currentPosition, bool enemy)
    {
        List<Tower> towers = enemy ? _enemyTowers : _playersTowers;

        return GetNearest(currentPosition, towers, out float distance);
    }

    private T GetNearest<T>(in Vector3 currentPosition, List<T> objects, out float distance) where T : MonoBehaviour
    {
        distance = float.MaxValue;
        if (objects.Count <= 0) return null;

        distance = Vector3.Distance(currentPosition, objects[0].transform.position);
        T nearest = objects[0];

        for (int i = 1; i < objects.Count; i++)
        {
            float tempDistance = Vector3.Distance(currentPosition, objects[i].transform.position);
            if (tempDistance > distance) continue;

            nearest = objects[i];
            distance = tempDistance;
        }

        return nearest;
    }

    private void SubscribeDestroy<T>(List<T> objects) where T : IDestroyed
    {
        for (int i = 0; i < objects.Count; i++)
        {
            T obj = objects[i];
            objects[i].Destroyed += RemoveAndunsubscribe;

            void RemoveAndunsubscribe()
            {
                RemoveObjectFromList(objects, obj);
                obj.Destroyed -= RemoveAndunsubscribe;
            }
        }
    }

    public void AddObjectToList<T>(List<T> list, T obj) where T : IDestroyed
    { 
        list.Add(obj);
        obj.Destroyed += RemoveAndunsubscribe;

        void RemoveAndunsubscribe()
        {
            RemoveObjectFromList(list, obj);
            obj.Destroyed -= RemoveAndunsubscribe;
        }
    }

    public void RemoveObjectFromList<T>(List<T> list, T obj)
    {
        if (list.Contains(obj)) list.Remove(obj);
    }

    //public void RemoveUnitFromList(Unit unit)
    //{
    //    _enemyUnits.Remove(unit);
    //    _playersUnits.Remove(unit);
    //}

    //public void RemoveTowerFromList(Tower tower)
    //{
    //    _enemyTowers.Remove(tower);
    //    _playersTowers.Remove(tower);
    //}

    //private void CreateHealthIndicators()
    //{
    //    foreach (var item in _enemyTowers)
    //    {
    //        Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
    //    }

    //    foreach (var item in _playersTowers)
    //    {
    //        Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
    //    }

    //    foreach (var item in _enemyUnits)
    //    {
    //        Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
    //    }

    //    foreach (var item in _playersUnits)
    //    {
    //        Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
    //    }
    //}
}