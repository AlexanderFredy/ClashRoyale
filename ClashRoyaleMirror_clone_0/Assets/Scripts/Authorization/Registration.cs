using System;
using System.Collections.Generic;
using UnityEngine;

public class Registration : MonoBehaviour
{
    private const string LOGIN = "login";
    private const string PASSWORD = "password";

    private string _login;
    private string _password;
    private string _confirmPassword;

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

    public void SetConfirmPassword(string confirmPassword)
    {
        _confirmPassword = confirmPassword;
    }

    public void SignUp()
    {
        if (string.IsNullOrEmpty(_login) || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_confirmPassword))
        {
            ErrorMessage("Login and/or password is empty");
            return;
        }

        if (_password != _confirmPassword)
        {
            ErrorMessage($"passwords are not eaquel {_password} != {_confirmPassword}");
            return;
        }

        string uri = URILibrary.MAIN + URILibrary.REGISTRATION;
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {LOGIN, _login},
            {PASSWORD, _password},
        };
        WebRequestToMySQL.Instance.StartPost(uri, data, SuccessMessage, ErrorMessage);
    }

    private void SuccessMessage(string data)
    {
        if (data != "ok")
        {
            ErrorMessage("Server answer: " + data);
            return;
        }

        Debug.Log("Successfully registration!");
        Success?.Invoke();
    }

    private void ErrorMessage(string errror)
    {
        Debug.LogError(errror);
        Error?.Invoke();
    }
}
