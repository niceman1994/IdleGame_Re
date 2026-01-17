using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListUp : MonoBehaviour
{
    [SerializeField] List<UpgradeStatus> upgradeStatusButton;
    [Space(15)]
    [SerializeField] RectTransform upgradeUI;
    [SerializeField] Transform upgradeUIInLeft;
    [SerializeField] Transform upgradeUIOutRight;
    [Space(15)]
    [SerializeField] RectTransform invenUI;
    [SerializeField] Transform invenUIInUp;
    [SerializeField] Transform invenUIOutDown;

    private bool moveUI;
    private bool moveInven;

    private void Start()
    {
        SetStatusUpButtonEvent();
        moveUI = false;
        moveInven = false;
    }

    private void Update()
    {
        MoveUpgradeParts();
        MoveInventory();
    }

    public void ClickStatus()
    {
        if (moveUI == false && moveInven == false)
            moveUI = true;
        else if (moveUI == true && moveInven == false)
            moveUI = false;
        else if (moveUI == false && moveInven == true)
            moveUI = false;
    }

    public void ClickInventory()
    {
        if (moveInven == false && moveUI == false)
            moveInven = true;
        else if (moveInven == true && moveUI == false)
            moveInven = false;
        else if (moveInven == false && moveUI == true)
            moveInven = false;
    }

    private void MoveUpgradeParts()
    {
        if (moveUI == true)
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIOutRight.localPosition, 0.055f);
        else
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIInLeft.localPosition, 0.055f);
    }

    private void MoveInventory()
    {
        if (moveInven == true)
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, invenUIInUp.localPosition, 0.05f);
        else
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, invenUIOutDown.localPosition, 0.05f);
    }

    private void SetStatusUpButtonEvent()
    {
        for (int i = 0; i < upgradeStatusButton.Count; i++)
        {
            switch(upgradeStatusButton[i].name)
            {
                case "ATKUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddAtk);
                    break;
                case "HPUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddHp);
                    break;
                case "ASUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddAtkSpeed);
                    break;
                case "MSUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddMoveSpeed);
                    break;
                case "SPUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddSkillPower);
                    break;
                case "GOLDUP":
                    upgradeStatusButton[i].SetUpgradeButtonEvent(upgradeStatusButton[i].AddEarnGold);
                    break;
                default:
                    break;
            }
        }
    }
}
