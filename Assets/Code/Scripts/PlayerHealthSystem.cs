using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider; 
    public TextMeshProUGUI healthText; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Fireball":
                TakeDamage(20);
                break;
            case "SaluwaAttack":
                TakeDamage(10);
                break;
            case "BigSaluwaAttack":
                TakeDamage(20);
                break;
            case "Damage":
                TakeDamage(15);
                break;
            case "Heal":
                Heal(30);
                Destroy(other.gameObject);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"Damage: {amount} | Health: {currentHealth}");

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Healed: {amount} | Health: {currentHealth}");

        UpdateUI();
    }

    void Die()
    {
        Debug.Log("اللاعب مات!");
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
    }
}
