using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _blockingImage;

    public void StartTick(int tick)
    {
        _text.text = tick.ToString();
    }

    public void Destroy()
    {
        Destroy(_blockingImage);
    }
}
