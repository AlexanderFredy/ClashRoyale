using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsLibrary : MonoBehaviour
{
    [field: SerializeField] public Card[] cards;

    public bool TryGetDeck(string[] CardsIDs, out Dictionary<string, Card> deck)
    {
        deck = new Dictionary<string, Card>();
        for (int i = 0; i < CardsIDs.Length; i++)
        {
            if (int.TryParse(CardsIDs[i], out int id) == false || id == 0) return false;
            Card card = GetCardByID(id);
            if (card == null) return false;

            deck.Add(CardsIDs[i], card);
        }

        return true;
    }

    public Card GetCardByID(int id) => cards.FirstOrDefault(c => c.id == id);
}

[System.Serializable]
public class Card
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public int id { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public Unit unit { get; private set; }
    [field: SerializeField] public GameObject hologram { get; private set; }
}

