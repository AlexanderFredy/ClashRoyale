using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private float upOffset = 50f;
    [SerializeField] private RectTransform _rectTrans;
    [SerializeField] private Image _filledImage;
    private Transform _owner;
    private Health _ownerHealth;
    private float _maxHealth;

    public void Init(Transform owner)
    {
        _owner = owner;
        _ownerHealth = _owner.GetComponent<Health>();
        _maxHealth = _ownerHealth.max;
    }

    void Update()
    {
        if (_owner == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 pos = Camera.main.WorldToScreenPoint(_owner.position) + new Vector3(0f, upOffset);
        _rectTrans.position = pos;

        _filledImage.fillAmount = _ownerHealth.Current / _maxHealth;
        _tmp.text = _ownerHealth.Current.ToString("0.0");
    }
}
