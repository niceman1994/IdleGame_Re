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

    private int prevStage;
    private Queue<Object> bossQueue = new Queue<Object>();
    private Dictionary<int, Queue<Object>> monsterPools = new Dictionary<int, Queue<Object>>();

    // 플레이어가 진행 도중에 죽었을 때 남은 몬스터를 회수하기 위해 사용하는 리스트 변수
    private List<Object> dequeueMonsterList = new List<Object>();

    protected override void Awake()
    {
        base.Awake();
        EnqueueMonsters();
        SummonMonster();
        GetCurrentStage();
    }

    private void SummonMonster()
    {
        if (stageNum % bossStage == 0)
            SetBoss();
        else
            SetMonsterPosition(7.0f);
    }

    private void EnqueueMonsters()
    {
        EnqueueBoss();

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

    private void EnqueueBoss()
    {
        GameObject bossObject = Instantiate(boss, new Vector3(12.0f, boss.transform.position.y, 0.0f), Quaternion.identity, monsterParent);
        bossObject.name = "Boss";        
        bossObject.gameObject.SetActive(false);
        bossQueue.Enqueue(bossObject.GetComponent<Object>());
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

    private void SetBoss()
    {
        Object bossObject = bossQueue.Peek();
        bossObject.gameObject.SetActive(true);
    }

    public void ReturnPooledObject(Object pooledObject)
    {
        if (pooledObject.gameObject.activeSelf == true)
        {
            pooledObject.ResetObjectStatus();
            pooledObject.GetComponent<BoxCollider2D>().enabled = true;
            pooledObject.gameObject.SetActive(false);

            if (!pooledObject.name.Contains("Boss"))
            {
                for (int i = 0; i < monsterPools.Values.Count; i++)
                {
                    Object monster = monsterPools.Values.ElementAt(i).Peek();

                    // 오브젝트 풀링으로 재활용하기 큐에서 뺐던 오브젝트와 현재 큐의 맨 앞에 있는 오브젝트를 비교해 일치하면 다시 큐에 넣음
                    if (pooledObject.CompareObjectType(monster))
                    {
                        monsterPools.Values.ElementAt(i).Enqueue(pooledObject);
                        dequeueMonsterList.Remove(pooledObject);
                    }
                }
            }
        }
    }

    private void GetCurrentStage()
    {
        prevStage = stageNum;
        stage.text = $"STAGE {stageNum}";
    }

    public void StageUp()
    {
        stageNum += 1;
        prevStage = stageNum;
        SummonMonster();
        GetCurrentStage();
    }

    public void StageDown()
    {
        if (stageNum > 1)
            stageNum = prevStage;
        else
            stageNum = 1;

        SummonMonster();
        GetCurrentStage();
    }

    public void ReturnPooledMonsters()
    {
        // 플레이어가 죽어서 뒤에서부터 확인하면서 생성된 몬스터를 큐에 다시 집어넣음
        for (int i = dequeueMonsterList.Count - 1; i >= 0; i--)
            ReturnPooledObject(dequeueMonsterList[i]);

        ReturnPooledObject(bossQueue.Peek());
    }

    public bool IsReturnComplete()
    {
        return dequeueMonsterList.Count == 0;
    }
}
