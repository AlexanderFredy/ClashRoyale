using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CardsInGame : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private CardsLibrary _cardsLibrary;
    public ReadOnlyDictionary<string, Card> _playerDeck { get; private set; }
    public ReadOnlyDictionary<string, Card> _enemyDeck { get; private set; }

    public void SetDecks(string[] playerCards, string[] enemyCards)
    {
        bool player = _cardsLibrary.TryGetDeck(playerCards, out Dictionary<string, Card> playerDeck);
        bool enemy = _cardsLibrary.TryGetDeck(enemyCards, out Dictionary<string, Card> enemyDeck);

        if (!player)
            Debug.LogError("Loading player deck fail!");
        if (!enemy)
            Debug.LogError("Loading enemy deck fail!");

        _playerDeck = new ReadOnlyDictionary<string, Card>(playerDeck);
        _enemyDeck = new ReadOnlyDictionary<string, Card>(enemyDeck);
    }

    public List<string> GetAllID() => _playerDeck.Keys.ToList();
}
