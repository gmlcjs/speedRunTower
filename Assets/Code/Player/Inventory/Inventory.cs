using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventoryUIScript inventoryUIScript;
    public InventorySlot[] WeaponSlot;
    public InventorySlot[] PotionSlot;
    public InventorySlot[] MaterialSlot;

    public int MaxNumPerCell;
    public int Space { get; set; }
    public bool NotEnoughRoom => items.Count >= Space;

    public List<InventoryItem> items = new List<InventoryItem>();

    public bool Add(Item _item)
    {
        InventorySlot[] targetSlots = GetTargetSlots(_item.itemType);

        // 1. 같은 아이템이 존재하고, 해당 슬롯에 여유 공간이 있다면 추가
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == _item && !items[i].IsFull)
            {
                items[i].NumPerCell++;
                targetSlots[i].UpdateNumText();
                return true;
            }
        }

        // 2. 해당 아이템을 위한 슬롯에 공간이 있는지 확인
        if (items.Count >= targetSlots.Length)
            return false;

        // 3. 새로운 아이템 추가
        items.Add(new InventoryItem(_item));
        inventoryUIScript.UpdateUI();

        return true;
    }

    // 아이템 타입에 맞는 슬롯 배열을 반환하는 메서드
    private InventorySlot[] GetTargetSlots(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.Weapon:
                return WeaponSlot;
            case Item.ItemType.Potion:
                return PotionSlot;
            case Item.ItemType.Material:
                return MaterialSlot;
            default:
                return null;  // 예외 상황 방지
        }
    }

    public bool Remove(int discardNum)
    {
        int slotNum = inventoryUIScript.selectedSlotNum;
        items[slotNum].NumPerCell -= discardNum;

        if (items[slotNum].NumPerCell > 0)
        {
           // inventoryUIScript.slots[slotNum].UpdateNumText();
            //   QuestUIScript.Instance.UpdateAllObjectives();
            return false;
        }
        else
        {
            items.Remove(items[slotNum]);
            inventoryUIScript.UpdateUI();
            return true;
        }
    }

    public bool AddMultiple(Item addItem, int count)
    {
        bool isNewItem = true;
        if (items.Count == 0)
        {
            InventoryItem newItem = new InventoryItem(addItem);
            items.Add(newItem);
            newItem.NumPerCell = 0;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].item.Equals(addItem) || items[i].IsFull)
            {
                if (i == items.Count - 1 && isNewItem)
                {
                    InventoryItem newItem = new InventoryItem(addItem);
                    items.Add(newItem);
                    newItem.NumPerCell = 0;
                    isNewItem = false;
                }
                continue;
            }

            if (items[i].NumPerCell + count > MaxNumPerCell)
            {
                count += items[i].NumPerCell - MaxNumPerCell - 1;
                if (CheckAddable(count))
                {
                    items[i].NumPerCell = MaxNumPerCell;
                    items.Add(new InventoryItem(addItem));
                    continue;
                }
                else
                {
                    if (!isNewItem)
                        items.Remove(items[i]);
                    inventoryUIScript.UpdateUI();
                    return false;
                }
            }
            else
            {
                items[i].NumPerCell += count;
                break;
            }
        }
        inventoryUIScript.UpdateUI();

        return true;
    }

    private bool CheckAddable(int remainNum)
    {
        int quotient = (remainNum + 1) / MaxNumPerCell;
        int remainder = (remainNum + 1) % MaxNumPerCell;
        if (remainder != 0)
            quotient++;

        return Space - items.Count >= quotient;
    }

    public void RemoveMultiple(Item removeItem, int count)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (!items[i].item.Equals(removeItem))
                continue;

            if (items[i].NumPerCell - count > 0)
            {
                items[i].NumPerCell -= count;
                break;
            }
            else if (items[i].NumPerCell - count == 0)
            {
                items.Remove(items[i]);
                break;
            }
            else
            {
                count = -(items[i].NumPerCell - count);
                items.Remove(items[i]);
            }
        }

        inventoryUIScript.UpdateUI();
    }

    public int GetItemCount(Item _item)
    {
        int itemCount = 0;
        foreach (var inventoryItem in items)
        {
            if (inventoryItem != null && inventoryItem.item.name.Equals(_item.name))
            {
                itemCount += inventoryItem.NumPerCell;
            }
        }
        return itemCount;
    }
}