using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] SkillSO skillData;
    [SerializeField] Button skillUIButton;
    [SerializeField] float coolTime;      // 게임화면 상단에 있는 임의의 스킬을 쓴 후 coolTimeText에 줄어드는 쿨타임을 표시하기 위한 변수
    [SerializeField] Text coolTimeText;

    private Color normalColor;
    private Color disableColor;

    public event Action<SkillSO> onSkillPressed;

    private void Awake()
    {
        SkillButtonInit();
    }

    public void SkillButtonInit()
    {
        normalColor = skillUIButton.colors.normalColor;
        disableColor = skillUIButton.colors.disabledColor;
        coolTimeText.text = $"{skillData.coolTimeReset}";
        skillUIButton.onClick.AddListener(ClickSkill);
    }

    private void ClickSkill()
    {
        skillUIButton.image.color = disableColor;
        skillUIButton.enabled = false;
        StartCoroutine(RunningCoolTime());
        ClickSkillButton();
    }

    private IEnumerator RunningCoolTime()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        while (skillUIButton.enabled == false)
        {
            coolTimeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            coolTimeText.text = coolTime > 0 ? $"{(int)--coolTime}" : $"{0}";

            if (coolTime == 0)
            {
                coolTimeText.text = $"{skillData.coolTimeReset}";
                coolTime = skillData.coolTimeReset;
                skillUIButton.image.color = normalColor;
                skillUIButton.enabled = true;
                coolTimeText.gameObject.SetActive(false);
            }
        }
    }

    private void ClickSkillButton()
    {
        onSkillPressed?.Invoke(skillData);
    }
}
