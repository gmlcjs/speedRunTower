using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    private Inventory inventory;

    public Transform[] itemsParent;
    public InventorySlot[] WeaponSlot;
    public InventorySlot[] PotionSlot;
    public InventorySlot[] MaterialSlot;

    //DetailsUI
    public int selectedSlotNum { get; set; }

    public int discardNum { get; set; }

    public TextMeshProUGUI ItemNameText;
    public Image ItemImg;
    public GameObject UseItemBtn, DiscardUI;
    public InputField discardNumIF;

    private void Awake()
    {
/*        Inventory.instance.Space = WeaponSlot.Length;
        Inventory.instance.Space = PotionSlot.Length;
        Inventory.instance.Space = MaterialSlot.Length;
        inventory = Inventory.instance;*/
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < WeaponSlot.Length; i++)
        {
            if (i < inventory.items.Count)
                WeaponSlot[i].AddItem(inventory.items[i]);
            else
                WeaponSlot[i].ClearSlot();
        }
        for (int i = 0; i < PotionSlot.Length; i++)
        {
            if (i < inventory.items.Count)
                PotionSlot[i].AddItem(inventory.items[i]);
            else
                PotionSlot[i].ClearSlot();
        }
        for (int i = 0; i < MaterialSlot.Length; i++)
        {
            if (i < inventory.items.Count)
                MaterialSlot[i].AddItem(inventory.items[i]);
            else
                MaterialSlot[i].ClearSlot();
        }
        //  QuestUIScript.Instance.UpdateAllObjectives();
    }

    public void ShowItemInform(int slotNum)
    {
        selectedSlotNum = slotNum;

        Item item = inventory.items[selectedSlotNum].item;
        ItemNameText.text = item.name;
        ItemImg.sprite = item.icon;
        UseItemBtn.SetActive(item.isConsumable);

        UIManager.instance.ItemDetailsUI.SetActive(true);
    }

    public void OnUseBtn()
    {
        WeaponSlot[selectedSlotNum].inventoryItem.item.Use();
        PotionSlot[selectedSlotNum].inventoryItem.item.Use();
        MaterialSlot[selectedSlotNum].inventoryItem.item.Use();
        DeleteFromInventory();
    }

    public void OnDiscardBtn()
    {
        DiscardUI.SetActive(!DiscardUI.activeSelf);
        if (DiscardUI.activeSelf)
            discardNumIF.text = 1.ToString();
    }

    public void OnDiscardOKBtn()
    {
        discardNum = int.Parse(discardNumIF.text);
        DeleteFromInventory();
    }

    public void LimitInputRange()
    {
        int inputNum = int.Parse(discardNumIF.text);
        if (inputNum < 1)
        {
            discardNumIF.text = 1.ToString();
        }
        if (inputNum > inventory.items[selectedSlotNum].NumPerCell)
        {
            discardNumIF.text = inventory.items[selectedSlotNum].NumPerCell.ToString();
        }
    }

    private void DeleteFromInventory()
    {
        bool deleteFromCell = inventory.Remove(1);
        UIManager.instance.ItemDetailsUI.SetActive(!deleteFromCell);
    }
}