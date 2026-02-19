using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeStatList : MonoBehaviour
{
    [SerializeField] List<UpgradeStat> upgradeStatButtons;
    [Space(15)]
    [SerializeField] Button statusButton;
    [SerializeField] RectTransform upgradeUI;
    [SerializeField] Transform upgradeUIInLeft;
    [SerializeField] Transform upgradeUIOutRight;
    [Space(15)]
    [SerializeField] Button invenButton;
    [SerializeField] RectTransform invenUI;
    [SerializeField] Transform invenUIInUp;
    [SerializeField] Transform invenUIOutDown;

    private bool moveStatUI = false;
    private bool moveInventory = false;

    private void Start()
    {
        SetStatusUpButtonEvent();
        OnClickButtonsEvent();
    }

    private void FixedUpdate()
    {
        MoveStatUI();
        MoveInventory();
    }

    private void MoveStatUI()
    {
        if (moveStatUI == true)
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIOutRight.localPosition, 0.7f);
        else
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIInLeft.localPosition, 0.7f);
    }

    private void MoveInventory()
    {
        if (moveInventory == true)
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, invenUIInUp.localPosition, 0.7f);
        else
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, invenUIOutDown.localPosition, 0.7f);
    }

    private void SetStatusUpButtonEvent()
    {
        if (upgradeStatButtons.Count == 0)
            Debug.LogWarning($"{upgradeStatButtons} 리스트에 요소가 들어있지 않습니다!");

        for (int i = 0; i < upgradeStatButtons.Count; i++)
        {
            switch(upgradeStatButtons[i].name)
            {
                case "ATKUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddAtk);
                    break;
                case "HPUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddHp);
                    break;
                case "ASUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddAtkSpeed);
                    break;
                case "MSUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddMoveSpeed);
                    break;
                case "SPUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddSkillPower);
                    break;
                case "GOLDUP":
                    upgradeStatButtons[i].SetUpgradeButtonEvent(upgradeStatButtons[i].AddEarnGold);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnClickButtonsEvent()
    {
        statusButton.onClick.RemoveAllListeners();
        invenButton.onClick.RemoveAllListeners();

        statusButton.onClick.AddListener(OnClickStatusButton);
        invenButton.onClick.AddListener(OnClickInvenButtonEvent);
    }

    private void OnClickStatusButton()
    {
        if (moveStatUI == false)
            moveStatUI = true;
        else moveStatUI = false;

        moveInventory = false;
        SoundManager.Instance.PlayPlayerUIClickSound();
    }

    private void OnClickInvenButtonEvent()
    {
        if (moveInventory == false)
            moveInventory = true;
        else moveInventory = false;

        moveStatUI = false;
        SoundManager.Instance.PlayPlayerUIClickSound();
    }
}
