using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Spawner : NetworkBehaviour
{
    private CardManager _cardManager;
    private MapInfo _mapInfo;
    
    #region CLIENT
    private Queue<GameObject> _holograms = new Queue<GameObject>();
    private PlayerPrefab _playerPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(DelayStartClient());
    }

    private IEnumerator DelayStartClient()
    {
        var manager = MatchmakingManager.Instance;
        yield return new WaitUntil(() => manager.localSceneDependency != null);

        manager.localSceneDependency.SetSpawner(this);

        _cardManager = manager.localSceneDependency.cardManager;
        _cardManager.SetSpawner(this);

        _playerPrefab = NetworkClient.localPlayer.GetComponent<PlayerPrefab>();
    }

    public void SendSpawn(string id, in Vector3 spawnPoint)
    {
        print(spawnPoint);
        var hologram = Instantiate(_cardManager.GetHologramByID(id), spawnPoint, Quaternion.identity);
        _holograms.Enqueue(hologram);

        _playerPrefab.CmdSpawn(id, spawnPoint);
    }

    public void DestroyHologram()
    {
        if (_holograms.Count < 1) return;

        var hologram = _holograms.Dequeue();
        Destroy(hologram);
    }
    #endregion

    #region SERVER

    private float _height;
    public void SetHeight(float spawnHeight)
    {
        _height = spawnHeight;
    }
    public void InitServer(CardManager cardManager, MapInfo mapInfo)
    {
        _cardManager = cardManager;
        _mapInfo = mapInfo;
    }

    [Server]
    public void Spawn(string id, Vector3 spawnPoint, bool isEnemy)
    {
        spawnPoint.y = _height;

        Unit unitPrefab = _cardManager.GetUnityByID(id, isEnemy);
        Quaternion rotation = isEnemy ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation, transform);
        unit.Init(isEnemy, _mapInfo);
        _mapInfo.AddUnit(unit);

        NetworkServer.Spawn(unit.gameObject);
    }

    #endregion
}
