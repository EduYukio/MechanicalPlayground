using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class Mechanics : ScriptableObject {
    public List<Mechanic> mechanicList = new List<Mechanic>();
    private List<Mechanic> savedState = new List<Mechanic>();

    public void ResetMechanics() {
        foreach (var mechanic in mechanicList) {
            mechanic.Enabled = false;
        }
    }

    public void EnableBasicMechanics() {
        ResetMechanics();
        Activate("Walk");
        Activate("Jump");
        Activate("Attack");
    }

    public void SaveState() {
        savedState = mechanicList.ConvertAll(mechanic => new Mechanic(mechanic.Name, mechanic.Enabled));
    }

    public void RestoreState() {
        if (savedState == null) return;

        foreach (var mechanic in mechanicList) {
            Mechanic savedMechanic = GetMechanic(mechanic.Name, savedState);
            mechanic.Enabled = savedMechanic.Enabled;
        }
    }

    public Mechanic GetMechanic(string name, List<Mechanic> list) {
        var mechanic = list.Find(x => x.Name == name);
        return mechanic;
    }

    public void Activate(string name) {
        GetMechanic(name, mechanicList).Enabled = true;
    }

    public void Deactivate(string name) {
        GetMechanic(name, mechanicList).Enabled = false;
    }

    public bool IsEnabled(string name) {
        return GetMechanic(name, mechanicList).Enabled;
    }
}

[System.Serializable]
public class Mechanic {
    public string Name;
    public bool Enabled;

    public Mechanic(string name, bool enabled) {
        this.Name = name;
        this.Enabled = enabled;
    }
}
