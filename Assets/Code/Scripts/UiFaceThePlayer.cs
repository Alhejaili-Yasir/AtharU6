using UnityEngine;

public class FaceToPlayer : MonoBehaviour
{
    private Transform playerCam;

    void Start()
    {
        if (Camera.main != null)
        {
            playerCam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (playerCam == null)
            return;

        transform.LookAt(playerCam);
        transform.rotation = Quaternion.LookRotation(playerCam.forward);
    }
}
