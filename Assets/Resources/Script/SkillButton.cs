using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] SkillSO skillData;
    [SerializeField] Button skillButton;
    [SerializeField] float coolTime;      // 게임화면 상단에 있는 임의의 스킬을 쓴 후 coolTimeText에 줄어드는 쿨타임을 표시하기 위한 변수
    [SerializeField] Text coolTimeText;

    private Color normalColor;
    private Color disableColor;
    private Skill skill;

    private void Update()
    {
        skill.CheckBuffTime();
    }

    public void SkillButtonInit()
    {
        skill = skillData.CreateSkill();
        skill.AddBuff(skillData);
        normalColor = skillButton.colors.normalColor;
        disableColor = skillButton.colors.disabledColor;
        coolTimeText.text = $"{skillData.coolTimeReset}";
        skillButton.onClick.AddListener(ClickSkill);
    }

    private void ClickSkill()
    {
        skill.Use(GameManager.Instance.player);
        skillButton.image.color = disableColor;
        skillButton.enabled = false;
        StartCoroutine(RunningCoolTime());
    }

    private IEnumerator RunningCoolTime()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        while (skillButton.enabled == false)
        {
            coolTimeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            coolTimeText.text = coolTime > 0 ? $"{(int)--coolTime}" : $"{0}";

            if (coolTime == 0)
            {
                coolTimeText.text = $"{skillData.coolTimeReset}";
                skillButton.image.color = normalColor;
                skillButton.enabled = true;
                coolTimeText.gameObject.SetActive(false);
            }
        }
    }
}
