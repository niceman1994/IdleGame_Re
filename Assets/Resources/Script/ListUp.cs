using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListUp : MonoBehaviour
{
    [SerializeField] List<UpgradeStatus> upgradeStatusButton;
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

    private bool moveStatusUI;
    private bool moveInventory;

    private void Start()
    {
        SetStatusUpButtonEvent();
        OnClickButtonsEvent();
        moveStatusUI = false;
        moveInventory = false;
    }

    private void Update()
    {
        MoveStatusUI();
        MoveInventory();
    }

    private void MoveStatusUI()
    {
        if (moveStatusUI == true)
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIOutRight.localPosition, 0.055f);
        else
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, upgradeUIInLeft.localPosition, 0.055f);
    }

    private void MoveInventory()
    {
        if (moveInventory == true)
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

    private void OnClickButtonsEvent()
    {
        statusButton.onClick.RemoveAllListeners();
        invenButton.onClick.RemoveAllListeners();

        statusButton.onClick.AddListener(OnClickStatusButton);
        invenButton.onClick.AddListener(OnClickInvenButtonEvent);
    }

    private void OnClickStatusButton()
    {
        if (moveStatusUI == false)
            moveStatusUI = true;
        else moveStatusUI = false;

        moveInventory = false;
        SoundManager.Instance.buttonSound.Play();
    }

    private void OnClickInvenButtonEvent()
    {
        if (moveInventory == false)
            moveInventory = true;
        else moveInventory = false;

        moveStatusUI = false;
        SoundManager.Instance.buttonSound.Play();
    }
}
