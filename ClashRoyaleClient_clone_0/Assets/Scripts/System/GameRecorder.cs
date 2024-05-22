using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MatchmakingManager;
using static TimerManager;

public class GameRecorder : MonoBehaviour
{
    [System.Serializable]
    public class Game
    {
        public string jsonDeck;
        public string startTick;
        public List<SpawnLog> spawnLogs = new List<SpawnLog>();
    }


    [System.Serializable]
    public class SpawnLog
    {
        public string json;
        public bool isEnemy;

        public SpawnLog(string json, bool isEnemy)
        {
            this.json = json;
            this.isEnemy = isEnemy;
        }
    }

    private Game _game = new Game();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        MultiplayerManager.Instance.StartGame += StartGame;
        MultiplayerManager.Instance.StartTick += StartTick;
        MultiplayerManager.Instance.SpawnPlayer += SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy += SpawnEnemy;
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.StartGame -= StartGame;
        MultiplayerManager.Instance.StartTick -= StartTick;
        MultiplayerManager.Instance.SpawnPlayer -= SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy -= SpawnEnemy;
    }

    private void SpawnEnemy(string obj)
    {
        _game.spawnLogs.Add(new SpawnLog(obj, true));
    }

    private void SpawnPlayer(string obj)
    {
        _game.spawnLogs.Add(new SpawnLog(obj, false));
    }

    private void StartGame(string jsonDeck)
    {
        _game.jsonDeck = jsonDeck;
    }

    private void StartTick(string jsonTick)
    {
        Tick tick = JsonUtility.FromJson<Tick>(jsonTick);
        if (tick.tick < 10) return;
        _game.startTick = jsonTick;
    }

    public void SaveGame()
    {
        string path = Path.Combine(Application.dataPath, "../Files", "GameLog.txt");
        string json = JsonUtility.ToJson(_game);
        using (StreamWriter sw = new StreamWriter(path,true))
            sw.Write(json);
    }

    public bool isPlay = false;

    public void LoadGame()
    {
        isPlay = true;
        string path = Path.Combine(Application.dataPath, "../Files", "GameLog.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();
            _game = JsonUtility.FromJson<Game>(json);
        }

        Decks decks = JsonUtility.FromJson<Decks>(_game.jsonDeck);
        string[] playerDeck = decks.player1;
        string[] enemyDeck = decks.player2;
        CardsInGame.Instance.SetDeck(playerDeck, enemyDeck);
        SceneManager.LoadScene("GamePlayScene");
    }

    public void ContinueLoadGame()
    {
        FindObjectOfType<TimerManager>().StartTick(_game.startTick);
        var spawner = FindObjectOfType<Spawner>();
        for (int i = 0; i < _game.spawnLogs.Count; i++)
        {
            var log = _game.spawnLogs[i];
            spawner.StartCoroutine(spawner.Spawn(log.json, log.isEnemy));
        }
    }
}
