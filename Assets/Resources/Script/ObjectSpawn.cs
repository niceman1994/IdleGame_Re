using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawn : Singleton<ObjectSpawn>
{
    [Header("스테이지 관련 변수")]
    public int stageNum;
    public Button stageButton;
    public Text stage;
    [SerializeField] int bossStage;
    [Space(15.0f)]
    public int monsterCount;
    public Transform monsterParent;
    [Header("보스 및 몬스터 프리팹")]
    public List<GameObject> monsterName;
    public GameObject boss;

    private int prevStageNum = 1;
    private Queue<GameObject> monsterQueue = new Queue<GameObject>();
    private Queue<GameObject> bossQueue = new Queue<GameObject>();

    private void Start()
    {
        stage.text = "STAGE " + stageNum.ToString();
        stageButton.onClick.AddListener(GetStage);
        SummonMonster(7.0f);
    }

    private void SummonMonster(float summonInterval)
    {
        if (stageNum % bossStage == 0)
        {
            GameObject bossObject = Instantiate(boss, new Vector3(12.0f, boss.transform.position.y, 0.0f),
                Quaternion.identity, monsterParent);
            bossObject.name = "Boss";
            bossQueue.Enqueue(bossObject);
        }
        else
        {
            for (int i = 0; i < monsterCount; ++i)
            {
                GameObject monsterObject = Instantiate(monsterName[Random.Range(0, 4)],
                    new Vector3(summonInterval + (4.0f * i), -1.15f, 0.0f), Quaternion.identity, monsterParent);
                monsterQueue.Enqueue(monsterObject);
            }
        }
    }

    public void PullObject(GameObject obj)
    {
        obj.GetComponent<BoxCollider2D>().enabled = true;
        obj = null;
        Destroy(obj);
    }

    public void DestroyMonster()
    {
        for (int i = 0; i < monsterParent.childCount; ++i)
        {
            GameObject obj = monsterParent.GetChild(i).gameObject;
            Destroy(obj);
            obj = null;
        }
    }

    private void GetStage()
    {
        if (stageNum != prevStageNum)
        {
            stage.text = "STAGE " + stageNum.ToString();
            prevStageNum = stageNum;
        }
    }

    public void StageUp()
    {
        DestroyMonster();
        stageNum += 1;
        SummonMonster(7.0f);
        stageButton.onClick.Invoke();
    }

    public void StageDown()
    {
        DestroyMonster();

        if (stageNum > 1)
            stageNum -= 1;
        else
            stageNum = 1;

        SummonMonster(7.0f);
        stageButton.onClick.Invoke();
    }

    public void ReturnPoolingBoss(GameObject obj)
    {
        obj.SetActive(false);
    }
}
