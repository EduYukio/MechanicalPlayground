using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slots : MonoBehaviour {
    public TextMeshProUGUI[] slotTexts = new TextMeshProUGUI[3];
    public string[] mechanics = new string[3];
    public int lastIndexAvailable = 0;

    public void AddMechanic(string name) {
        if (lastIndexAvailable >= mechanics.Length) {
            return;
        }

        slotTexts[lastIndexAvailable].text = name;
        mechanics[lastIndexAvailable] = name;
        lastIndexAvailable++;
    }
}
