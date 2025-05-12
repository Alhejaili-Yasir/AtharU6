using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // المحور (مثلاً Y = حول نفسه)
    public float rotationSpeed = 100f; // السرعة

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
