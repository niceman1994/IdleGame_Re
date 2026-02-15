using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : Singleton<SkillUI>
{
    [SerializeField] List<SkillButton> skill;
    [SerializeField] AudioSource thunderSound;

    private List<Buff> buffList = new List<Buff>();

    private void Start()
    {
        SkillInit();
    }

    private void SkillInit()
    {
        for (int i = 0; i < skill.Count; i++)
            skill[i].SkillButtonInit();
    }
}

/*
 for (int i = 0; i < skill.Count; i++)
     {
         // "https://mentum.tistory.com/343" 링크를 참조할 것
         // for문에서 람다식으로 버튼 이벤트를 등록하면 반복문의 마지막 변수 값만 참조되어 Closure Problem 이 발생해 값을 복사해서 사용해야함
         int temp = i;
         skill[i].skillBtn.onClick.AddListener(() => ClickSkill(skill[temp]));
     }
*/