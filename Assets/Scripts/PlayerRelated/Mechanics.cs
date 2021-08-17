using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Mechanics : ScriptableObject {
    public List<Mechanic> mechanicList = new List<Mechanic>();
    private List<Mechanic> savedState = new List<Mechanic>();
    public static Action MechanicChanged;

    public void ResetMechanics() {
        foreach (var mechanic in mechanicList) {
            Deactivate(mechanic.Name);
        }
    }

    public void EnableBasicMechanics() {
        Activate("Walk");
        Activate("Jump");
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
        Mechanic mechanic = GetMechanic(name, mechanicList);
        if (mechanic != null) {
            mechanic.Enabled = true;
            MechanicChanged?.Invoke();
        }
    }

    public void Deactivate(string name) {
        Mechanic mechanic = GetMechanic(name, mechanicList);
        if (mechanic != null) {
            mechanic.Enabled = false;
            MechanicChanged?.Invoke();
        }
    }

    public bool IsEnabled(string name) {
        Mechanic mechanic = GetMechanic(name, mechanicList);
        if (mechanic != null) {
            return mechanic.Enabled;
        }

        return false;
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
