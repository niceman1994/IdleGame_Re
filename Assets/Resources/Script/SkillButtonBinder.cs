using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonBinder : MonoBehaviour
{
    [SerializeField] SkillController skillController;
    [SerializeField] List<SkillButton> skillButtons = new List<SkillButton>();

    private void Awake()
    {
        foreach (var button in skillButtons)
            skillController.RegisterButton(button);
    }
}
