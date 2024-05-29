using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchmakingManager : MonoBehaviour
{
    [System.Serializable]
    public class Decks
    {
        public string player1ID;
        public string[] player1;
        public string[] player2;
    }

    [SerializeField] private string _gameSceneName = "GamePlayScene";
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _matchmakingCanvas;
    [SerializeField] private GameObject _cancelButton;

    public async void FindOpponent()
    {
        _cancelButton.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _matchmakingCanvas.SetActive(true);

        await MultiplayerManager.Instance.Connect();
        _cancelButton.SetActive(true);
    }

    public void CancelFind()
    {
        _mainMenuCanvas.SetActive(true);
        _matchmakingCanvas.SetActive(false);

        MultiplayerManager.Instance.Leave();
    }

    public void Subscribe()
    {
        MultiplayerManager.Instance.GetReady += GetReady;
        MultiplayerManager.Instance.StartGame += StartGame;
        MultiplayerManager.Instance.CancelStart += CancelStart;
    }

    private void GetReady()
    {
        _cancelButton.SetActive(false);
    }

    private void StartGame(string jsonDecks)
    {
        Decks decks = JsonUtility.FromJson<Decks>(jsonDecks);
        string[] playerDeck;
        string[] enemyDeck;
        print($"{MultiplayerManager.Instance.clientID} || {jsonDecks}");

        if (decks.player1ID == MultiplayerManager.Instance.clientID)
        {
            playerDeck = decks.player1;
            enemyDeck = decks.player2;
        } else
        {
            playerDeck = decks.player2;
            enemyDeck = decks.player1;
        }


        CardsInGame.Instance.SetDeck(playerDeck, enemyDeck);
        SceneManager.LoadScene(_gameSceneName);
    }

    private void CancelStart()
    {
        _cancelButton.SetActive(true);
    }

    public void Unsubscribe()
    {
        MultiplayerManager.Instance.GetReady -= GetReady;
        MultiplayerManager.Instance.StartGame -= StartGame;
        MultiplayerManager.Instance.CancelStart -= CancelStart;
    }
}
