using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour {
    private PlayerFSM player;

    private void Start() {
        player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    public void RestartLevel() {
        Checkpoint.ResetCheckPointState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel() {
        Checkpoint.ResetCheckPointState();
        Goal goal = GameObject.Find("Goal")?.GetComponent<Goal>();
        goal?.WhenPicked.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLevel() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex > 2) {
            Checkpoint.ResetCheckPointState();
            SceneManager.LoadScene(buildIndex - 1);
        }
        else {
            Debug.LogWarning("There is no previous level.");
        }
    }

    public void BeeLevel() {
        Checkpoint.ResetCheckPointState();
        player.mechanics.ResetMechanics();
        player.mechanics.EnableBasicMechanics();
        SceneManager.LoadScene(15);
    }
}