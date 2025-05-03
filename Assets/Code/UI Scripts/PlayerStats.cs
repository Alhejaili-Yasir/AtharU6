using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int money = 0;
    public AudioSource moneySound; 

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddGold(int amount)
    {
        money += amount;

        if (moneySound != null)
            moneySound.Play();

        Debug.Log("You have Money " + money);
    }
}
