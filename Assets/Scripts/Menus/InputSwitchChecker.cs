﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputSwitchChecker : MonoBehaviour {
    public GameObject firstButton;
    Button[] buttons;
    Vector3 initialMousePosition;
    bool gamepadMode = false;
    bool mouseMode = false;

    private void OnEnable() {
        buttons = GetComponentsInChildren<Button>();
        initialMousePosition = Input.mousePosition;
        ChangeToGamePadMode();
        StartCoroutine(nameof(DeselectAfterOneFrame));
    }

    private void Update() {
        CheckMouseInput();
        CheckGamePadInput();
    }

    private void OnDestroy() {
        Cursor.visible = true;
    }

    public List<RaycastResult> ThrowRayCastAtMousePosition() {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        return raycastResults;
    }

    public Button CheckIfMouseRayHitButton(List<RaycastResult> hitList) {
        if (hitList.Count <= 0) return null;

        foreach (RaycastResult hit in hitList) {
            GameObject hoveredObj = hit.gameObject;
            Button hoveredButton = hoveredObj.GetComponent<Button>();
            if (hoveredButton != null) {
                return hoveredButton;
            }
        }
        return null;
    }

    public void SetButtonHighlight() {
        List<RaycastResult> hitList = ThrowRayCastAtMousePosition();
        Button hoveredButton = CheckIfMouseRayHitButton(hitList);
        if (hoveredButton != null) {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            hoveredButton.OnPointerEnter(pointer);
        }
    }

    void DeselectAllButtons() {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        foreach (var button in buttons) {
            button.OnPointerExit(pointer);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator DeselectAfterOneFrame() {
        yield return null;
        DeselectAllButtons();
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void CheckGamePadInput() {
        if (gamepadMode) return;

        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) {
            ChangeToGamePadMode();
        }
    }

    void CheckMouseInput() {
        if (mouseMode) return;

        if (initialMousePosition != Input.mousePosition) {
            ChangeToMouseMode();
        }
    }

    private void ChangeToGamePadMode() {
        gamepadMode = true;
        mouseMode = false;
        Cursor.visible = false;
        DeselectAllButtons();
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void ChangeToMouseMode() {
        gamepadMode = false;
        mouseMode = true;
        Cursor.visible = true;
        initialMousePosition = Input.mousePosition;
        DeselectAllButtons();
        SetButtonHighlight();
    }
}