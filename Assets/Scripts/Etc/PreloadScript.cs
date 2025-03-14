using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScript : MonoBehaviour
{
    public Sound currentBGM;
    private int currentBGMIndex = 0;
    private int maxBGMIndex = 2;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadNextScene();
    }

    private void Update()
    {
        CheckIfBGMEnded();
    }

    public void InitializeFirstBGM()
    {
        Manager.audio.Play("BGM0");
        currentBGM = Manager.audio.FindSound("BGM0");
        currentBGMIndex = 0;
    }

    private void LoadNextScene()
    {
        if (PreloadInitializer.selectedScene > 0)
        {
            SceneManager.LoadScene(PreloadInitializer.selectedScene);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private void CheckIfBGMEnded()
    {
        if (currentBGM.source == null) return;

        if (!currentBGM.source.isPlaying)
        {
            if (currentBGMIndex == maxBGMIndex)
            {
                currentBGMIndex = 0;
            }
            else
            {
                currentBGMIndex++;
            }

            string clipName = "BGM" + currentBGMIndex.ToString();
            currentBGM = Manager.audio.FindSound(clipName);
            Manager.audio.Play(clipName);
        }
    }
}
