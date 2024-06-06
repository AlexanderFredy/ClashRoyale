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
    [field: SerializeField] public CardsLibrary CardsLibrary { get; private set; }
    [SerializeField] private List<Card> _availableCards;// = new List<Card>();
    [SerializeField] private List<Card> _selectedCards;// = new List<Card>();
    
    public IReadOnlyList<Card> AvailableCards { get { return _availableCards; } }
    public IReadOnlyList<Card> SelectedCards { get { return _selectedCards; } }

    #region Editor
#if UNITY_EDITOR
    [field: SerializeField] public AvailableDeckUI availableDeckUI { get; private set; }
#endif
    #endregion

    public void Init(List<int> availableCardIndexes, int[] selectedCardIndexes)
    {
        for (int i = 0; i < availableCardIndexes.Count; i++)
        {
            _availableCards.Add(CardsLibrary.cards[availableCardIndexes[i]]);
        }
        for (int i = 0; i < selectedCardIndexes.Length; i++)
        {
            _selectedCards.Add(CardsLibrary.cards[selectedCardIndexes[i]]);
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
}
