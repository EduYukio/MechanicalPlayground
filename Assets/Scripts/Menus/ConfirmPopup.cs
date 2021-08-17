using UnityEngine;

public class ConfirmPopup : MonoBehaviour {
    public MechanicsMenu mechanicsMenu;

    private void Update() {
        CheckMenuExitInput();
    }

    public void SaveButton() {
        mechanicsMenu.ReloadLevel();
    }

    public void DontSaveButton() {
        mechanicsMenu.RevertMechanics();
        gameObject.SetActive(false);
        mechanicsMenu.BackToPauseMenu();
    }

    private void CheckMenuExitInput() {
        if (Input.GetButtonDown("Esc") || Input.GetButtonDown("Circle")) {
            DontSaveButton();
        }
    }
}