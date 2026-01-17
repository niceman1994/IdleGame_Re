using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SkillInfo
{
    public Button skillBtn;
    public float coolTime;      // 스킬 쿨타임을 표시하기 위한 변수
    public float coolTimeReset;
    public float buffTime;
    public float skillPower;
    public Text coolTimeText;
}

public class Skill : Singleton<Skill>
{
    [SerializeField] List<SkillInfo> skill;
    [SerializeField] AudioSource _thunderSound;

    private Color normalColor;
    private Color disableColor;

    private void Start()
    {
        normalColor = skill[0].skillBtn.colors.normalColor;
        disableColor = skill[0].skillBtn.colors.disabledColor;
        
        for (int i = 0; i < skill.Count; i++)
            skill[i].coolTimeText.text = skill[i].coolTimeReset.ToString();

        for (int i = 0; i < skill.Count; i++)
        {
            /// <see cref="https://mentum.tistory.com/343"/> 참조할 것</para>
            /* for문에서 버튼 이벤트 등록을 람다식으로 할 땐 모든 람다식이 마지막 반복문의 변수 값으로 참조되어 
              Closure Problem 이 발생해 값을 복사해서 사용해야 한다고 함 */
            int temp = i;
            skill[i].skillBtn.onClick.AddListener(() => ClickSkill(skill[temp]));
        }
    }

    public void ClickSkill(SkillInfo skill)
    {
        skill.skillBtn.GetComponent<Image>().color = disableColor;
        skill.skillBtn.enabled = false;
        StartCoroutine(RunningSkill(skill, skill.buffTime));
        SkillName(skill, skill.skillPower);
    }

    private void SkillName(SkillInfo skillInfo, float skillValue)
    {
        if (skillInfo.skillBtn.name == "PowerUp")
            GameManager.Instance.player.CurrentAtk(skillValue);
        else if (skillInfo.skillBtn.name == "Thunder")
        {
            GameManager.Instance.SummonThunder();

            if (!_thunderSound.isPlaying)
                _thunderSound.Play();
        }
    }

    private IEnumerator RunningSkill(SkillInfo skill, float buffTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        while (skill.skillBtn.enabled == false)
        {
            if (buffTime > 0 && skill.skillBtn.name == "Heal") 
                GameManager.Instance.player.CurrentHp(skill.skillPower);

            skill.coolTimeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            buffTime--;
            skill.coolTimeText.text = skill.coolTime > 0 ? ((int)--skill.coolTime).ToString() : 0.ToString();

            if (buffTime == 0)
            {
                if (skill.skillBtn.name == "PowerUp")
                    GameManager.Instance.player.CurrentAtk(-skill.skillPower);
            }

            if (skill.coolTime == 0)
            {
                skill.coolTimeText.text = skill.coolTimeReset.ToString();
                skill.skillBtn.image.color = normalColor;
                skill.skillBtn.enabled = true;
                skill.coolTimeText.gameObject.SetActive(false);
            }
        }
    }
}
