using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public GameObject gameOverUI;    // اسحب هنا الـ UI اللي يبغى يصير نشط عند الموت

    void Start()
    {
        currentHealth = maxHealth;
        if (gameOverUI != null)
            gameOverUI.SetActive(false);    // تأكد إنه مخفي في البداية
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
            Die();
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
        if (gameOverUI != null)
            gameOverUI.SetActive(true);      // فعل الـUI عند الموت
        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2f);   // انتظر 2 ثواني عشان يشوف اللاعب الـUI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
            healthText.text = $"Health: {currentHealth}";
    }
}
