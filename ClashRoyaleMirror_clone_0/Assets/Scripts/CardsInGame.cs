using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CardsInGame : Singleton<CardsInGame>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public ReadOnlyDictionary<string, Card> _playerDeck { get; private set; }
    public ReadOnlyDictionary<string, Card> _enemyDeck { get; private set; }

    public void SetDeck(string[] playerCards, string[] enemyCards)
    {
        bool player = FindObjectOfType<DeckManager>().TryGetDeck(playerCards, out Dictionary<string, Card> playerDeck);
        bool enemy = FindObjectOfType<DeckManager>().TryGetDeck(enemyCards, out Dictionary<string, Card> enemyDeck);

        if (!player)
            Debug.LogError("Loading player deck fail!");
        if (!enemy)
            Debug.LogError("Loading enemy deck fail!");

        _playerDeck = new ReadOnlyDictionary<string, Card>(playerDeck);
        _enemyDeck = new ReadOnlyDictionary<string, Card>(enemyDeck);
    }

    public List<string> GetAllID() => _playerDeck.Keys.ToList();
}
