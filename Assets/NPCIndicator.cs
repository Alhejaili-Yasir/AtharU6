using UnityEngine;

public class NPCIndicator : MonoBehaviour
{
    public GameObject indicatorObject;

    public void Toggle(bool state)
    {
        if (indicatorObject != null)
            indicatorObject.SetActive(state);
    }
}
