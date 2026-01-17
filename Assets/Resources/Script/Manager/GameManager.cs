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
    public Text fullHp;
    public Image hpBar;
    public float userSpeed;
    public GameGold gameGold;
    public Inventory inventory;

    [Header("스킬")]
    public Thunder thunder;
    public Transform thunderPos;
    [SerializeField] Transform thunderParent;

    public Queue<Thunder> poolingThunder = new Queue<Thunder>();

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var thunderObj = Instantiate(thunder.gameObject);
            thunderObj.transform.SetParent(thunderParent);
            thunderObj.name = "Thunder";
            thunderObj.gameObject.SetActive(false);
            poolingThunder.Enqueue(thunderObj.GetComponent<Thunder>());
        }
    }

    public void SummonThunder()
    {
        var thunderArray = poolingThunder.ToArray();

        for (int i = 0; i < thunderArray.Length; i++)
        {
            thunderArray[i].gameObject.SetActive(true);
            thunderArray[i].transform.position = new Vector3(thunderPos.position.x + (1.5f * i), thunderPos.position.y - 0.4f, 0.0f);
        }
    }

    public void AddThunderPower(float addDamage)
    {
        var thunderArray = poolingThunder.ToArray();

        for (int i = 0; i < thunderArray.Length; i++)
            thunderArray[i].AddPower(addDamage);
    }
}
