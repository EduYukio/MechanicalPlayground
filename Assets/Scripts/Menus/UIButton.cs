using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler {
    public bool playDefaultHover = true;
    public bool playDefaultConfirm = true;

    private Button button;

    private void Start() {
        button = GetComponent<Button>();
    }

    public void PlayCustomSound(string soundName) {
        Manager.audio.Play("UI_" + soundName);
    }

    private void PlayHoverSound() {
        if (playDefaultHover) {
            Manager.audio.Play("UI_Hover");
        }
    }

    private void PlayConfirmSound() {
        if (playDefaultConfirm) {
            Manager.audio.Play("UI_Confirm");
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData) {
        PlayConfirmSound();
    }

    public void OnSelect(BaseEventData eventData) {
        PlayHoverSound();
    }

    public void OnSubmit(BaseEventData eventData) {
        PlayConfirmSound();
    }
}