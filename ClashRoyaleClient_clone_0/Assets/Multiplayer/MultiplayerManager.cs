using System.Threading.Tasks;
using Colyseus;
using System.Collections.Generic;
using System;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string RoomName = "state_handler";

    private const string GetReadyName = "GetReady";
    private const string StartGameName = "Start";
    private const string CancelStartName = "CancelStart";
    private const string StartTickName = "StartTick";
    private const string SpawnPlayerName = "SpawnPlayer";
    private const string SpawnEnenmyName = "SpawnEnemy";
    private const string CheatName = "Cheat";

    private ColyseusRoom<State> _room;

    public event Action GetReady;
    public event Action<string> StartGame;
    public event Action CancelStart;
    public event Action<string> StartTick;
    public event Action<string> SpawnPlayer;
    public event Action<string> SpawnEnemy;
    public event Action Cheat;

    public string clientID { get { return _room == null ? "": _room.SessionId; } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
    }

    public async Task Connect()
    {
        var data = new Dictionary<string, object>
        {
            {"id", UserInfo.Instance.ID }
        };

        _room = await Instance.client.JoinOrCreate<State>(RoomName, data);
        _room.OnMessage<object>(GetReadyName, (empty) => GetReady?.Invoke());
        _room.OnMessage<string>(StartGameName, (jsonDecks) => StartGame?.Invoke(jsonDecks));
        _room.OnMessage<object>(CancelStartName, (empty) => CancelStart?.Invoke());
        _room.OnMessage<string>(StartTickName, (tick) => StartTick?.Invoke(tick));
        _room.OnMessage<string>(SpawnPlayerName, (spawnData) => SpawnPlayer?.Invoke(spawnData));
        _room.OnMessage<string>(SpawnEnenmyName, (spawnData) => SpawnEnemy?.Invoke(spawnData));
        _room.OnMessage<object>(CheatName, (empty) => Cheat?.Invoke());
    }

    public void Leave()
    {
        _room.Leave();
        _room = null;
    }

    public void SendMessage(string key,Dictionary<string, string> data)
    {
        _room.Send(key,data);
    }
}
