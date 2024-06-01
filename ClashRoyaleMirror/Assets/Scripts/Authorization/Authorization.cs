using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    private const string LOGIN = "login";
    private const string PASSWORD = "password";

    private string _login;
    private string _password;

    public event Action Error;
    public event Action Success;

    public void SetLogin(string login)
    {
        _login = login; 
    }

    public void SetPassword(string password)
    {
        _password = password;
    }

    public void SignIn()
    {
        if (string.IsNullOrEmpty(_login) || string.IsNullOrEmpty(_password))
        {
            ErrorMessage("Login and/or password is empty");
            return;
        }

        string uri = URILibrary.MAIN + URILibrary.AUTHORIZATION;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {LOGIN, _login},
            {PASSWORD, _password},
        };
        WebRequestToMySQL.Instance.StartPost(uri,data,SuccessMessage, ErrorMessage);
    }

    private void SuccessMessage(string data)
    {
        string[] result = data.Split('|');
        if (result.Length < 2 || result[0] != "ok")
        {
            ErrorMessage("Server answer: " + data);
            return;
        }

        if (int.TryParse(result[1], out int id))
        {
            UserInfo.Instance.SetID(id);
            Debug.Log("success for id:" + id);
            Success.Invoke();
        }
        else { 
            ErrorMessage($"Can't try parse \"{result[1]}\" in integer. Full answer is: {data}");
        }
    }

    private void ErrorMessage(string errror)
    {
        Debug.LogError(errror);
        Error?.Invoke();
    }
}
