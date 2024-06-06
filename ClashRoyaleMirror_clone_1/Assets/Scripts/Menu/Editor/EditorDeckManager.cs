using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeckManager))]
public class EditorDeckManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DeckManager deckManager = (DeckManager)target;

        if (GUILayout.Button("Update card list in \"AvailableDeckUI\""))
        {
            deckManager.availableDeckUI.SetAllCardsCount(deckManager.CardsLibrary.cards);

            Debug.Log("Update card list in \"AvailableDeckUI\"");
        }
    }
}
