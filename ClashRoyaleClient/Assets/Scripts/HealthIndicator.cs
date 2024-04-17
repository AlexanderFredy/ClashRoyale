using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private RectTransform _rectTrans;
    [SerializeField] private Image _filledImage;
    [SerializeField] private Transform _owner;
    private Health _ownerHealth;
    private float _maxHealth;

    private void Start()
    {
        _ownerHealth = _owner.GetComponent<Health>();
        _maxHealth = _ownerHealth.max;
        _tmp.text = _maxHealth.ToString("0.0");

        _ownerHealth.UpdateHealth += UpdateHealth;
    }

    void UpdateHealth(float current)
    {
        if (_owner == null)
        {
            Destroy(gameObject);
            return;
        }

        _filledImage.fillAmount = current / _maxHealth;
        _tmp.text = current.ToString("0.0");
    }

    private void OnDestroy()
    {
        _ownerHealth.UpdateHealth -= UpdateHealth;
    }
}
