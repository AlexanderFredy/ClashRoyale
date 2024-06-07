using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Spawner : NetworkBehaviour
{
    private CardManager _cardManager;
    private Queue<GameObject> _holograms = new Queue<GameObject>();

    private void SpawnEnemy(string json) => Spawn(json, true);

    private void SpawnPlayer(string json) => Spawn(json, false);

    public void Spawn(string jsonSpawnData, bool isEnemy)
    {
        SpawnData data = JsonUtility.FromJson<SpawnData>(jsonSpawnData);
        string id = data.cardID;
        Vector3 spawnPoint = new Vector3(data.x, data.y, data.z);

        Unit unitPrefab = _cardManager.GetUnityByID(id, isEnemy);
        Quaternion rotation = Quaternion.identity;
        if (isEnemy)
        {
            rotation = Quaternion.Euler(0,180,0);
            spawnPoint *= -1;
        }     

        if (isEnemy == false && _holograms.Count > 0)
        {
            var hologram = _holograms.Dequeue();
            Destroy(hologram);
        }

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }

    private void CancelSpawn()
    {
        if (_holograms.Count < 1) return;
     
            var hologram = _holograms.Dequeue();
            Destroy(hologram);
    }

    public void SendSpawn(string id, in Vector3 spawnPoint)
    {
        var hologram = Instantiate(_cardManager.GetHologramByID(id), spawnPoint, Quaternion.identity);
        _holograms.Enqueue(hologram);

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"json", JsonUtility.ToJson(new SpawnData(id,spawnPoint)) }
        };

        MultiplayerManager.Instance.SendMessage("Spawn", data);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(DelayStartClient());
    }

    private IEnumerator DelayStartClient()
    {
        var manager = MatchmakingManager.Instance;
        yield return new WaitUntil(() => manager.localSceneDependency != null);

        manager.localSceneDependency.cardManager.SetSpawner(this);
    }

    [System.Serializable]
    public class SpawnData
    {
        public SpawnData(string id, Vector3 spawnPoint)
        {
            cardID = id;
            x = spawnPoint.x;
            y = spawnPoint.y;
            z = spawnPoint.z;
        }
        
        public string cardID;
        public float x;
        public float y;
        public float z;
        public uint serverTime;
    }
}
