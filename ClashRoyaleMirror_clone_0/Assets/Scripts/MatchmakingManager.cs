using Mirror;
using System;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class MatchmakingManager : Singleton<MatchmakingManager>
    {
    [Server]
    public void OnJoint(PlayerPrefab playerPrefab, string sqlID)
    {
        Debug.Log("OnJoint (MatchmakingManager)");

        playerPrefab.NetworkMatch.matchId = GetRandomString(5).ToGuid();
    }

    private string GetRandomString(int length)
    {
        string s = string.Empty;
        for (int i = 0; i < length; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26) s += (char)(random + 65);
            else s += (random - 26).ToString();
        }

        return s;
    }

}

public static class MatchExtensions
{
    /// <summary>
    /// Translate ID to GUID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>GUID</returns>
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] input = Encoding.Default.GetBytes(id);
        byte[] hash = provider.ComputeHash(input);
        
        return new Guid(hash);
    }
}