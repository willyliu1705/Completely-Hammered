using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenEdge : MonoBehaviour
{
    [SerializeField] private string goScreen;

    // Add Desired Scenes into Build Settings for this to function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Loader.Load(goScreen);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > PlayerPrefs.GetInt("maxSceneIndex"))
        {
            PlayerPrefs.SetInt("maxSceneIndex", currentSceneIndex);
        }
        PlayerPrefs.Save();
    }

    public string GoScreen()
    {
        return goScreen;
    }
}
