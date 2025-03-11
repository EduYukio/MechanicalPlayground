using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadInitializer
{
    public static int selectedScene = -2;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitLoadingScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) return;

        selectedScene = sceneIndex;
        SceneManager.LoadScene(0);
    }
}