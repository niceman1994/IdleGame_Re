using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SkillInfo
{
    public Button skillBtn;
    public float coolTime;      // 게임화면 상단에 있는 임의의 스킬을 쓴 후 coolTimeText에 줄어드는 쿨타임을 표시하기 위한 변수
    public float coolTimeReset;
    public float buffTime;
    public float skillPower;
    public Text coolTimeText;
}

public class Skill : Singleton<Skill>
{
    [SerializeField] List<SkillInfo> skill;
    [SerializeField] AudioSource thunderSound;

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
            // "https://mentum.tistory.com/343" 링크를 참조할 것
            // for문에서 람다식으로 버튼 이벤트를 등록하면 반복문의 마지막 변수 값만 참조되어 Closure Problem 이 발생해 값을 복사해서 사용해야함
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
        if (skillInfo.skillBtn.name.Equals("PowerUp"))
            GameManager.Instance.player.CurrentAtk(skillValue);
        else if (skillInfo.skillBtn.name.Equals("Thunder"))
        {
            GameManager.Instance.SummonThunder();

            if (!thunderSound.isPlaying)
                thunderSound.Play();
        }
    }

    private IEnumerator RunningSkill(SkillInfo skill, float buffTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        while (skill.skillBtn.enabled == false)
        {
            if (buffTime > 0 && skill.skillBtn.name.Equals("Heal"))
                GameManager.Instance.player.CurrentHpChange(skill.skillPower);

            skill.coolTimeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            if (GameManager.Instance.player.CurrentHp() > 0)
                buffTime--;
            else buffTime = 0.0f;
            Debug.Log($"남은 버프 시간 : {buffTime}");
            skill.coolTimeText.text = skill.coolTime > 0 ? ((int)--skill.coolTime).ToString() : 0.ToString();

            if (buffTime == 0 || GameManager.Instance.player.CurrentHp() > 0)
            {
                if (skill.skillBtn.name.Equals("PowerUp"))
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
