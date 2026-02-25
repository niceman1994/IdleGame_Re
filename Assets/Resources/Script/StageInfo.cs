using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [Header("스테이지 관련 변수")]
    public int currentStage;
    public Text stage;
    [SerializeField] int bossStage;
    [SerializeField] Player player;         // 스테이지가 플레이어의 상태에 따라 바뀌기 때문에 변수로 사용함

    private int prevStage;

    private void Start()
    {
        StageManagerInit();
        ObjectPoolManager.Instance.SummonMonster(currentStage, bossStage);
    }

    private void StageManagerInit()
    {
        GetCurrentStage();
        StageUpEvent(player);
        StageDownEvent(player);
    }

    private void GetCurrentStage()
    {
        prevStage = currentStage;
        stage.text = $"STAGE {currentStage}";
    }

    public void StageUp()
    {
        currentStage += 1;
        prevStage = currentStage;
        ObjectPoolManager.Instance.SummonMonster(currentStage, bossStage);
        GetCurrentStage();
    }

    public void StageDown()
    {
        if (currentStage > 1)
            currentStage = prevStage;
        else
            currentStage = 1;

        ObjectPoolManager.Instance.SummonMonster(currentStage, bossStage);
        GetCurrentStage();
    }

    private void StageUpEvent(Player player)
    {
        player.onStageUp += StageUp;
    }

    private void StageDownEvent(Player player)
    {
        player.onStageDown += StageDown;
    }
}
