using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalBar;
    [SerializeField] private Image currentBar;

    void Start()
    {
        UpdateHealthBar();
    }

    void Update()
    {
        currentBar.fillAmount = playerHealth.currentHealth / 10;
    }

    public void UpdateHealthBar()
    {
        totalBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
