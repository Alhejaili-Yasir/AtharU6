using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 45f; // Degrees per second

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
