using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SkillsUI : MonoBehaviour {
    public TMP_Text levelObj;
    private const int finalLevel = 15;

    void Start() {
        string currentLevelText = SceneManager.GetActiveScene().buildIndex.ToString();
        levelObj.text = "Level: " + currentLevelText + "/" + finalLevel.ToString();
    }

    void Update() {

    }
}
