using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MechanicsAvailable : MonoBehaviour {
    public Slots slots;

    public void Start() {
        slots = FindObjectOfType<Slots>();
    }

    public void Select() {
        string mechanicName = EventSystem.current.currentSelectedGameObject.name;
        slots.AddMechanic(mechanicName);
    }
}
