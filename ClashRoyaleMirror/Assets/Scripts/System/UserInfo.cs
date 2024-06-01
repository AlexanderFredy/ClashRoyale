using UnityEngine;

public class UserInfo : Singleton<UserInfo>
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int ID { get; private set; } = 9;
    public void SetID (int id) => ID = id;
}
