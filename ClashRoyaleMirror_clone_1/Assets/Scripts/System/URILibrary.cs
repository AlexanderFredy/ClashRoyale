using UnityEngine;

public class URILibrary : MonoBehaviour
{
#if UNITY_SERVER
    public const string MAIN = "http://localhost/Projects/LessonDataBase/";
#else
    public const string MAIN = "http://localhost/Projects/LessonDataBase/";
#endif
    public const string AUTHORIZATION = "Authorization/authorization.php";
    public const string REGISTRATION = "Authorization/registration.php";

    public const string GETDECKINFO = "Game/getDeckInfo.php";
    public const string SAVEDECK = "Game/saveDeck.php";
    public const string GETRATING = "Game/getRating.php";

#if UNITY_SERVER
    public const string GETDECK = "Colyseus/getDeck.php";
#endif
}
