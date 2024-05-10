using UnityEngine;

public class UserInfo : Singleton<UserInfo>
{
    public int ID { get; private set; } = 9;
    public void SetID (int id) => ID = id;
}
