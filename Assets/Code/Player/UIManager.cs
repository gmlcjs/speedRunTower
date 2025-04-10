using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Canvas")]
    public Canvas StaticCanvas;
    public Canvas DynamicCanvas;
    //public Canvas BossCanvas;

    [Header("UI")]
    public GameObject InventoryUI;
    public GameObject ItemDetailsUI;
    public GameObject StatusUI;

    public void OnOffCanvas(bool staticCanvas, bool dynamicCanvas, bool bossCanvas)
    {
        StaticCanvas.enabled = staticCanvas;
        DynamicCanvas.enabled = dynamicCanvas;
        //BossCanvas.enabled = bossCanvas;
    }

    public void OnInventoryBtn()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf);

        InventoryCategory.instance.WeaponItems.SetActive(true);
        InventoryCategory.instance.PotionItems.SetActive(false);
        InventoryCategory.instance.MaterialItems.SetActive(false);
        ItemDetailsUI.SetActive(false);
    }

    public void downInventoryBtn()
    {
        InventoryUI.SetActive(false);
    }

    public void OnStatusBtn()
    {
        StatusUI.SetActive(!StatusUI.activeSelf);
    }

    public void downStatusBtn()
    {
        StatusUI.SetActive(false);
    }
}
