using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {
    public void CreditsBackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
