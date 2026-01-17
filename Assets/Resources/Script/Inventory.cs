using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] GameObject horizontalListPrefab;
    [SerializeField] RectTransform addListButtonParent;
    [SerializeField] Button addListButton;
    [SerializeField] ItemExchanger itemExchanger;

    private InventorySlot[] slots;
    private Item item;
    private int addListCount = 0;

    private void Start()
    {
        MakeInvetorySlots();
        addListButton.onClick.AddListener(AddHorizontalList);
    }

    public void SlotItemsUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            // 1. 인벤토리 슬롯에 아이템 이미지가 없을 때
            // 2. 획득한 아이템의 이미지와 슬롯 아이템 이미지가 같으면서, 아이템의 갯수가 최대 갯수 이하일 때
            if (!slots[i].haveItem || CheckSameItem(slots[i]))
            {
                // 아이템 추가 또는 갯수 증가
                if (!slots[i].haveItem)
                    slots[i].AddItem(item);
                else
                    slots[i].SetItemCount(1);

                return;
            }
        }
    }

    private void MakeInvetorySlots()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject horzontalList = Instantiate(horizontalListPrefab, content);
            horzontalList.name = "HorizontalList";
            addListButtonParent.SetAsLastSibling();
        }
        StartCoroutine(FindInvetorySlots());
    }

    public void AddHorizontalList()
    {
        addListCount++;

        // Grid Layout Group으로 정리된 content의 자식객체로 horizontalListPrefab을 생성한다.
        GameObject Obj = Instantiate(horizontalListPrefab, content);
        Obj.name = "HorizontalList";

        // 인벤토리 추가버튼이 맨 아래로 가도록 SetAsLastSibling을 사용
        addListButtonParent.SetAsLastSibling();
        StartCoroutine(FindInvetorySlots());

        if (addListCount >= 3)
            addListButtonParent.gameObject.SetActive(false);
    }

    private IEnumerator FindInvetorySlots()
    {
        yield return null;
        slots = content.GetComponentsInChildren<InventorySlot>();

        /*
           인벤토리 슬롯은 게임이 실행되면 생성되는 방식임
          1. 여기서 잡템을 모아서 변환해줘야 할 때 실행 시작시 생성된 슬롯을 클릭했을 때 잡템인지 확인하고 우클릭했을 때 아이템 변환 UI를 보여주는 방식으로 할 것
          2. 이때 잡템의 수에 따라 바꿀 수 있는 아이템이 다르고 바꿀 수 없는 아이템은 반투명하고 만들어서 안되는 것처럼 보이게 만들고 실제로도 변환이 안되게 할 것
          3. 문제는 아이템 변환 스크립트를 어디에 배치할지가 고민인데 슬롯은 게임을 실행해야 생성되는데 아이템 변환 UI는 재활용하기 위해서 먼저 만들어놓은 상태임

           인벤토리 슬롯은 미리 생성되지 않으니 이미 있는 인벤토리에서 슬롯 정보를 가져와서 아이템 변환 UI가 작동하도록 코드를 작성하는게 좋아보임
         */
        for (int i = 0; i < slots.Length; i++)
        {
            //itemExchanger.SetItemExchangeBoxPosition(Input.mousePosition);
            slots[i].SetItemExchangePosition();
        }
    }

    private bool CheckSameItem(InventorySlot slot)
    {
        return item.ItemImage == slot.Icon.sprite && slot.ItemCount < item.MaxCount;
    }

    public void GetItem(Item dropItem)
    {
        item = dropItem;
    }
}
