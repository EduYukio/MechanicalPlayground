using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MechanicButton : MonoBehaviour
{
    public string mechanicName;
    public List<string> requirements;
    public static MechanicsMenu mechMenu;
    public Mechanics mechanics;
    public Descriptions descriptions;
    public Image requirementImage;
    public TMP_Text requirementText;

    public bool isBlocked = false;
    public bool isActive = false;

    private Image buttonImage;
    private Color activeColor = new Color(100 / 255f, 122 / 255f, 224 / 255f, 1f);
    private Color inactiveColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 1f);
    private Color blockedColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 0.5f);
    private Color blockedTextColor = new Color(1, 1, 1, 0.5f);
    private Color normalTextColor = new Color(1, 1, 1, 1f);
    private bool blinkRedImage;
    private bool blinkRedText;

    private void Awake()
    {
        if (mechMenu == null) mechMenu = GameObject.FindObjectOfType<MechanicsMenu>();
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ClickedOnMechanic);
    }

    private void Update()
    {
        Helper.CheckIfNeedToBlinkRed(ref blinkRedImage, requirementImage);
        Helper.CheckIfNeedToBlinkRed(ref blinkRedText, requirementText);
    }

    public void ClickedOnMechanic()
    {
        if (isBlocked)
        {
            if (CheckNoPoints()) return;

            RequirementWarning(requirementImage, requirementText);
            Manager.audio.Play("UI_Fail");
            return;
        }

        if (isActive)
        {
            DeactivateMechanic(mechanicName);
            DeactivateThoseWhoRequireThis();
        }
        else
        {
            if (CheckNoPoints()) return;
            ActivateMechanic(mechanicName);
        }
        mechMenu.changedMechanics = true;
        mechMenu.UpdateButtonsState();
    }

    private bool CheckNoPoints()
    {
        if (mechMenu.skillPoints == 0)
        {
            mechMenu.SkillPointsWarning();
            return true;
        }
        return false;
    }

    private void ActivateMechanic(string name)
    {
        Manager.audio.Play("UI_On");
        ActivateButtonImage();
        mechanics.Activate(name);
        mechMenu.skillPoints--;
    }

    private void DeactivateMechanic(string name)
    {
        Manager.audio.Play("UI_Off");
        DeactivateButtonImage();
        mechanics.Deactivate(name);
        mechMenu.skillPoints++;
    }

    private void DeactivateThoseWhoRequireThis()
    {
        foreach (var button in mechMenu.buttons)
        {
            if (button.requirements.Contains(this.mechanicName))
            {
                if (mechanics.IsEnabled(button.mechanicName))
                {
                    DeactivateMechanic(button.mechanicName);
                }
            }
        }
    }

    public void UpdateTutorialInfo()
    {
        mechMenu.videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, gameObject.name + ".mp4");
        mechMenu.videoPlayer.Play();
        mechMenu.rawImage.enabled = true;

        mechMenu.title.text = mechanicName;
        mechMenu.description.text = descriptions.GetDescription(mechanicName);
    }

    public void ClearTutorialInfo()
    {
        mechMenu.videoPlayer.Stop();
        mechMenu.rawImage.enabled = false;
        mechMenu.title.text = "";
        mechMenu.description.text = "";
    }

    public void ActivateButtonImage()
    {
        if (isBlocked) return;
        isActive = true;
        buttonImage.color = activeColor;
    }

    public void DeactivateButtonImage()
    {
        if (isBlocked) return;
        isActive = false;
        buttonImage.color = inactiveColor;
    }

    public void DecideIfIsBlocked()
    {
        isBlocked = false;
        if (requirements.Count == 0) return;

        foreach (string requirement in requirements)
        {
            if (!mechanics.IsEnabled(requirement))
            {
                isBlocked = true;
                return;
            }
        }
    }

    public void SetButtonAppearance()
    {
        buttonImage = GetComponent<Image>();
        if (isBlocked)
        {
            buttonImage.color = blockedColor;
            GetComponentInChildren<TextMeshProUGUI>().color = blockedTextColor;
        }
        else if (mechanics.IsEnabled(mechanicName))
        {
            isActive = true;
            buttonImage.color = activeColor;
            GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;
        }
        else
        {
            isActive = false;
            buttonImage.color = inactiveColor;
            GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;
        }
    }

    public void RequirementWarning(Image image, TMP_Text text)
    {
        if (image != null)
        {
            image.color = Color.red;
            blinkRedImage = true;
        }

        if (text != null)
        {
            text.color = Color.red;
            blinkRedText = true;
        }
    }
}
