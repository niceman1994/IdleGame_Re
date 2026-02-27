using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerUIMove : MonoBehaviour
{
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

    private void OnClickButtonsEvent()
    {
        statusButton.onClick.RemoveAllListeners();
        invenButton.onClick.RemoveAllListeners();

        statusButton.onClick.AddListener(OnClickStatsButton);
        invenButton.onClick.AddListener(OnClickInvenButtonEvent);
    }

    private void OnClickStatsButton()
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
