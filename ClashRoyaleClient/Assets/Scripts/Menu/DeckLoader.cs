using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private DeckManager _manager;
    [SerializeField] private List<int> _availableCards = new List<int>();
    [SerializeField] private int[] _selectedCards;// = new int[5];

    string stringIDs = string.Empty;

    private void Start()
    {
        StartLoad();
    }

    private void StartLoad()
    {
        WebRequestToMySQL.Instance.StartPost(URILibrary.MAIN + URILibrary.GETDECKINFO,
            new Dictionary<string, string>
            {
                { "userID",UserInfo.Instance.ID.ToString() }
            },
            SuccessLoad,
            ErrorLoad
            );
    }

    private void ErrorLoad(string error)
    {
        Debug.LogError(error);
        StartLoad();
    }

    private void SuccessLoad(string data)
    {
        DeckData deckData = JsonUtility.FromJson<DeckData>(data);

        _selectedCards = new int[deckData.selectedIDs.Length];
        for (int i = 0; i < _selectedCards.Length; i++)
        {
            int.TryParse(deckData.selectedIDs[i], out _selectedCards[i]);
        }

        for (int i = 0; i < deckData.availableCards.Length; i++)
        {
            int.TryParse(deckData.availableCards[i].id, out int id);
            _availableCards.Add(id);
        }

        _manager.Init(_availableCards, _selectedCards);
        _manager.DeckUploadToDataBase += SaveDeckToDataBase;
    }

    private void SaveDeckToDataBase(IReadOnlyList<Card> list)
    {
        foreach (Card card in list)
        {
            stringIDs += card.id.ToString() + ",";
        }

        StartUpload(stringIDs);
        print("upload..");
    }

    private void StartUpload(string strIDs)
    {       
        WebRequestToMySQL.Instance.StartPost(URILibrary.MAIN + URILibrary.SAVEDECK,
            new Dictionary<string, string>
            {
                { "userID",UserInfo.Instance.ID.ToString() },
                { "cardIDs",strIDs }
            },
            SuccessUpload,
            ErrorUpload
            );
    }

    private void ErrorUpload(string error)
    {
        Debug.LogError(error);
        StartUpload(stringIDs);
    }

    private void SuccessUpload(string data)
    {
        stringIDs = string.Empty;
        print("upload deck " + (data == "done" ? "succes" : "but something wrong/n" + data));
        _manager.LockScreenSetActive(false);
    }

    private void OnDestroy()
    {
        _manager.DeckUploadToDataBase -= SaveDeckToDataBase;
    }
}


[System.Serializable]
public class DeckData
{
    public AvailableCard[] availableCards;
    public string[] selectedIDs;
}

[System.Serializable]
public class AvailableCard
{
    public string name;
    public string id;  
}




