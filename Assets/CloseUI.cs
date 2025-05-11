using UnityEngine;

public class CloseButtonHandler : MonoBehaviour
{
    [Tooltip("Objects to deactivate when Close is pressed")]
    public GameObject[] objectsToDisable;

    public void Close()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
