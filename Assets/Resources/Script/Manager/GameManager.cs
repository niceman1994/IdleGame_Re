using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("유저 정보")]
    public Player player;
    public Text currentHp;
    public Text maxHp;
    public Image hpBar;
    public GameGold gameGold;
    [SerializeField] Inventory inventory;

    [Header("스킬")]
    public Thunder thunder;
    public Transform thunderPos;
    [SerializeField] Transform thunderParent;

    private List<Thunder> thunderList = new List<Thunder>();
    public Inventory Inventory => inventory;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var thunderObj = Instantiate(thunder.gameObject);
            thunderObj.transform.SetParent(thunderParent);
            thunderObj.name = $"Thunder_{i}";
            thunderObj.gameObject.SetActive(false);
            thunderList.Add(thunderObj.GetComponent<Thunder>());
        }
    }

    public void SummonThunder()
    {
        for (int i = 0; i < thunderList.Count; i++)
        {
            thunderList[i].gameObject.SetActive(true);
            thunderList[i].transform.position = new Vector3(thunderPos.position.x + (1.5f * i), thunderPos.position.y - 0.4f, 0.0f);
        }
    }

    public void AddThunderPower(float addDamage)
    {
        for (int i = 0; i < thunderList.Count; i++)
            thunderList[i].AddPower(addDamage);
    }
}
