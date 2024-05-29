using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectOfType<GameRecorder>().isPlay)
            FindObjectOfType<GameRecorder>().ContinueLoadGame();
    }

    public void SaveGame()
    {
        FindObjectOfType<GameRecorder>().SaveGame();
    }
}
