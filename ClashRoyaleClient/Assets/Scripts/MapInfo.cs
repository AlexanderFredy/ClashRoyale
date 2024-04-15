using System;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : Singleton<MapInfo>
{
    [SerializeField] private List<Tower> _enemyTowers = new List<Tower>();
    [SerializeField] private List<Tower> _playersTowers = new List<Tower>();

    [SerializeField] private List<Unit> _enemyUnits = new List<Unit>();
    [SerializeField] private List<Unit> _playersUnits = new List<Unit>();

    [SerializeField] private HealthIndicator _healthIndicatorPrefab;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        CreateHealthIndicators();
    }

    public bool TryGetNearestUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyUnits : _playersUnits;
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

    private void CreateHealthIndicators()
    {
        foreach (var item in _enemyTowers)
        {
            Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
        }

        foreach (var item in _playersTowers)
        {
            Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
        }

        foreach (var item in _enemyUnits)
        {
            Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
        }

        foreach (var item in _playersUnits)
        {
            Instantiate(_healthIndicatorPrefab, _canvas.transform).Init(item.transform);
        }
    }

    public void RemoveUnitFromList(Unit unit)
    {
        _enemyUnits.Remove(unit);
        _playersUnits.Remove(unit);
    }

    public void RemoveTowerFromList(Tower tower)
    {
        _enemyTowers.Remove(tower);
        _playersTowers.Remove(tower);
    }
}