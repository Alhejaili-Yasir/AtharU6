using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSampleScene : MonoBehaviour
{
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
