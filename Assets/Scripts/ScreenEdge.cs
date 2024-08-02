using UnityEngine;

public class ScreenEdge : MonoBehaviour
{
    [SerializeField] private string goScreen;

    // Add Desired Scenes into Build Settings for this to function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Loader.Load(goScreen);
    }

    public string GoScreen()
    {
        return goScreen;
    }
}
