using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TimerManager;

public class StartTimer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _blockingImage;

    private void StartTick(string jsonTick)
    {
        Tick tick = JsonUtility.FromJson<Tick>(jsonTick);

        if (tick.tick < 10)
        {
            _text.text = (10 - tick.tick).ToString();
        } else
        {
            Destroy(_blockingImage);
        }
    }
}
