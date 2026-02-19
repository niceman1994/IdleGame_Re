using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    /// 스킬을 썼을 때 소리가 나오게 하는 이벤트 변수 <see cref="SkillAudioSound.Start"/> 참조
    public event Action<SkillSO> onSkillSoundRequested; 

    public void RegisterButton(SkillButton button)
    {
        button.onSkillPressed += HandleSkill;
    }

    private void HandleSkill(SkillSO data)
    {
        var skill = data.CreateSkill();
        skill.AddBuff(data);

        // 등록한 함수 실행 후 바로 해제
        void RequestSound()
        {
            onSkillSoundRequested?.Invoke(data);
            skill.onSkillSoundPlay -= RequestSound;
        }

        skill.onSkillSoundPlay += RequestSound;
        skill.Use(GetComponent<IObject>());
    }
}
