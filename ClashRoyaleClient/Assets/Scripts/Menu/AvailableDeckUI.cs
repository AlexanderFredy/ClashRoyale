using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableDeckUI : MonoBehaviour
{
    [SerializeField] private CardSelecter _selecter;
    [SerializeField] private List<AvailableCardUI> _availableCardsUI;

    #region Editor
#if UNITY_EDITOR
    [SerializeField] private Transform _avalableCardParent;
    [SerializeField] private AvailableCardUI _availableCardUIPrefab;

    public void SetAllCardsCount(Card[] cards)
    {
        for (int i = 0; i < _availableCardsUI.Count; i++)
        {
            GameObject go = _availableCardsUI[i].gameObject;
            UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(go);};
        }
        _availableCardsUI.Clear();

        for (int i = 1; i < cards.Length; i++)
        {
            AvailableCardUI card = Instantiate(_availableCardUIPrefab, _avalableCardParent);
            card.Create(_selecter, cards[i], i);
            _availableCardsUI.Add(card);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    #endregion

    public void UpdateCardsList(IReadOnlyList<Card> available, IReadOnlyList<Card> selected)
    {
        for(int i = 0;i < _availableCardsUI.Count;i++) 
        {
            _availableCardsUI[i].SetState(AvailableCardUI.CardStateType.Locked);
        }

        for (int i = 0; i < available.Count; i++)
        {
            _availableCardsUI[available[i].id-1].SetState(AvailableCardUI.CardStateType.Available);
        }

        for (int i = 0; i < selected.Count; i++)
        {
            _availableCardsUI[selected[i].id-1].SetState(AvailableCardUI.CardStateType.Selected);
        }
    }
}
