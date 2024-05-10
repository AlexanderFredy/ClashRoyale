using UnityEngine;

public class RotateImage : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _speed*Time.deltaTime);
    }
}
