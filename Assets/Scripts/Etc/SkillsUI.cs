using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SkillsUI : MonoBehaviour {
    public List<GameObject> skills;
    public PlayerFSM player;

    void Awake() {
        Mechanics.MechanicChanged += EnableAndPositionSkills;
        player = GameObject.FindObjectOfType<PlayerFSM>();
        EnableAndPositionSkills();
    }

    private void OnDestroy() {
        Mechanics.MechanicChanged -= EnableAndPositionSkills;
    }



    public void EnableAndPositionSkills() {
        Vector3 initialPosition = new Vector3(-126, 198, 0);
        if (name.Contains("Compact")) {
            initialPosition = new Vector3(0, 198, 0);
        }
        float yDistance = -72;
        int enabledSkills = 0;
        foreach (var mechanic in player.mechanics.mechanicList) {
            if (mechanic.Name == "Walk" || mechanic.Name == "Jump") continue;

            if (player.mechanics.IsEnabled(mechanic.Name)) {
                GameObject skill = skills.Find(x => x.name == mechanic.Name);
                if (skill == null) {
                    Debug.Log("ERROR: SKILL " + mechanic.Name + "NOT FOUND; DISABLING SKILLS UI");
                    gameObject.SetActive(false);
                    return;
                }
                skill.SetActive(true);
                Vector3 newPos = initialPosition + new Vector3(0, yDistance * enabledSkills, 0);
                skill.transform.localPosition = newPos;
                enabledSkills++;
            }
            else {
                GameObject skill = skills.Find(x => x.name == mechanic.Name);
                skill.SetActive(false);
            }
        }
    }
}
