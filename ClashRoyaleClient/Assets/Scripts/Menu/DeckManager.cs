using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public event Action<IReadOnlyList<Card>, IReadOnlyList<Card>> UpdateAvailable;
    public event Action<IReadOnlyList<Card>> UpdateSelected;
    public event Action<IReadOnlyList<Card>> DeckUploadToDataBase;

    [SerializeField] private GameObject _lockScreenCanvas;
    [SerializeField] private Card[] _cards;
    [SerializeField] private List<Card> _availableCards;// = new List<Card>();
    [SerializeField] private List<Card> _selectedCards;// = new List<Card>();
    
    public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }
    public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }

    #region Editor
#if UNITY_EDITOR
    [SerializeField] private AvailableDeckUI _availableDeckUI;
    private void OnValidate()
    {
        _availableDeckUI.SetAllCardsCount(_cards);
    }
#endif
    #endregion

    public void Init(List<int> availableCardIndexes, int[] selectedCardIndexes)
    {
        for (int i = 0; i < availableCardIndexes.Count; i++)
        {
            _availableCards.Add(_cards[availableCardIndexes[i]]);
        }
        for (int i = 0; i < selectedCardIndexes.Length; i++)
        {
            _selectedCards.Add(_cards[selectedCardIndexes[i]]);
        }

        UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
        UpdateSelected?.Invoke(SelectedCards);

        _lockScreenCanvas.SetActive(false);
    }

    public void SaveDeckToDataBase(IReadOnlyList<Card> deck) 
    {
        _selectedCards.Clear();
        for (int i = 0; i < deck.Count; i++)
        {
            _selectedCards.Add(deck[i]);
        }
        UpdateSelected?.Invoke(SelectedCards);

        DeckUploadToDataBase?.Invoke(deck);

        _lockScreenCanvas.SetActive(true);
    }

    public void LockScreenSetActive(bool enable)
    {
        _lockScreenCanvas.SetActive(enable);
    }

    public bool TryGetDeck(string[] CardsIDs, out Dictionary<string, Card> deck)
    {
        deck = new Dictionary<string, Card>();
        for (int i = 0; i < CardsIDs.Length; i++)
        {
            if (int.TryParse(CardsIDs[i], out int id) == false || id == 0) return false;
            Card card = _cards.FirstOrDefault(c => c.id == id);
            if (card == null) return false;

            deck.Add(CardsIDs[i], card);
        }

        return true;
    }
}

[System.Serializable]
public class Card
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public int id { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; } 
}
