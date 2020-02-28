using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechanicsAvailable : MonoBehaviour {
    public Slots slots;

    public void Start() {
        slots = FindObjectOfType<Slots>();
    }

    public void Select(Button btn) {
        //string mechanicName = EventSystem.current.currentSelectedGameObject.name;
        string mechanicName = btn.name;
        slots.AddMechanic(mechanicName);
    }
}
