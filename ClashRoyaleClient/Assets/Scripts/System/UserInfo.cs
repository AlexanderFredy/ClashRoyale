using UnityEngine;

public class UserInfo : Singleton<UserInfo>
{
    public int ID {  get; private set; }
    public void SetID (int id) => ID = id;
}
