using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public void ReloadScene()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }
}
