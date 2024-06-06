using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingManager : MonoBehaviour
{
    [SerializeField] private Text _retingtText;
    void Start()
    {
        InitRating();
    }

    void InitRating()
    {
        string uri = URILibrary.MAIN + URILibrary.GETRATING;
        var data = new Dictionary<string, string>()
        {
            {"userID", UserInfo.Instance.ID.ToString()}
        };

        WebRequestToMySQL.Instance.StartPost(uri, data, Success, Error);
    }

    private void Success(string obj)
    {
        string[] result = obj.Split('|');
        if (result.Length != 3)
        {
            Error("Length of array is not 3\n" + obj);
            return;
        }

        if (result[0] != "ok")
        {
            Error("Strange result/n" + obj);
            return;
        }

        _retingtText.text = $"<color=green>{result[1]}</color> : <color=red>{result[2]}</color>";
    }

    private void Error(string obj)
    {
        Debug.LogError(obj);
    }
}
