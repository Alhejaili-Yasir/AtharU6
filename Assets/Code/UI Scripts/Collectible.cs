using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string itemName = "Meat"; 
    public float pickupRange = 2f;
    private Transform player;
    public GameObject promptUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            if (promptUI != null)
                promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                InventorySystem.Instance.AddItem(itemName);
                Destroy(gameObject);
            }
        }
        else
        {
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
