using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [Header("스테이지 관련 변수")]
    public int stageNum;
    public Text stage;
    [SerializeField] int bossStage;
    [Space(15.0f)]
    public int monsterCount;
    public Transform monsterParent;
    [Header("보스 및 몬스터 프리팹")]
    public List<GameObject> monsterPrefab;
    public GameObject boss;

    private Queue<GameObject> bossQueue = new Queue<GameObject>();
    private Dictionary<int, Queue<Object>> monsterPools = new Dictionary<int, Queue<Object>>();
    // 플레이어가 진행 도중에 죽었을 때 남은 몬스터를 회수하기 위해 사용하는 리스트 변수
    private List<Object> dequeueMonsterList = new List<Object>();

    private void Start()
    {
        GetCurrentStage();
        EnqueueMonsters();
        SummonMonster();
    }

    private void SummonMonster()
    {
        if (stageNum % bossStage == 0)
        {
            GameObject bossObject = Instantiate(boss, new Vector3(12.0f, boss.transform.position.y, 0.0f), Quaternion.identity, monsterParent);
            bossObject.name = "Boss";
            bossQueue.Enqueue(bossObject);
        }
        else
            SetMonsterPosition(7.0f);
    }

    private void EnqueueMonsters()
    {
        for (int i = 0; i < monsterPrefab.Count; i++)
            monsterPools.Add(i, new Queue<Object>());

        for (int i = 0; i < monsterPrefab.Count; i++)
        {
            for (int j = 0; j < monsterCount; j++)
            {
                GameObject queueObject = Instantiate(monsterPrefab[i], monsterParent);
                queueObject.SetActive(false);
                monsterPools[i].Enqueue(queueObject.GetComponent<Object>());
            }
        }
    }

    private void SetMonsterPosition(float summonInterval)
    {
        for (int i = 0; i < monsterCount; i++)
        {
            Object dequeueMonster = monsterPools[Random.Range(0, monsterPools.Count)].Dequeue();
            dequeueMonsterList.Add(dequeueMonster);
            dequeueMonster.gameObject.SetActive(true);
            dequeueMonster.transform.position = new Vector3(summonInterval + (3.5f * i), -1.15f, 0.0f);
        }
    }

    public void PullObject(Object pooledObject)
    {
        pooledObject.GetComponent<BoxCollider2D>().enabled = true;
        pooledObject.gameObject.SetActive(false);

        for (int i = 0; i < monsterPools.Values.Count; i++)
        {
            Object monster = monsterPools.Values.ElementAt(i).Peek();

            // 오브젝트 풀링으로 재활용하기 큐에서 뺐던 오브젝트와 현재 큐의 맨 앞에 있는 오브젝트를 비교해 일치하면 다시 큐에 넣음
            if (pooledObject.CompareObjectType(monster))
            {
                dequeueMonsterList.Remove(pooledObject);
                monsterPools.Values.ElementAt(i).Enqueue(pooledObject);
                return;
            }
        }
    }

    private void GetCurrentStage()
    {
        stage.text = $"STAGE {stageNum}";
    }

    public void StageUp()
    {
        stageNum += 1;
        SummonMonster();
        GetCurrentStage();
    }

    public void StageDown()
    {
        if (stageNum > 1)
            stageNum -= 1;
        else
            stageNum = 1;

        for (int i = 0; i < dequeueMonsterList.Count; i++)
        {
            dequeueMonsterList[i].gameObject.SetActive(false);

            for (int j = 0; j < monsterPools.Count; j++)
            {
                // 진행 도중 플레이어가 죽었다면 큐에서 빼낸 오브젝트가 남아있고 새롭게 몬스터를 배치할거라서 다시 큐에 넣어줌
                if (dequeueMonsterList[i].CompareObjectType(monsterPools[j].Peek()))
                    monsterPools[j].Enqueue(dequeueMonsterList[i]);
            }
        }
        // 나와있는 몬스터를 전부 큐에 넣었기 때문에 리스트를 비워줌
        dequeueMonsterList.Clear();

        SummonMonster();
        GetCurrentStage();
    }

    public void ReturnPoolingBoss(GameObject bossObject)
    {
        bossObject.SetActive(false);
    }
}
