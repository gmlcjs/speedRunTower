using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCategory : MonoBehaviour
{
    public static InventoryCategory instance;

    public Button weaponBtn;
    public Button potionBtn;
    public Button MaterialBtn;

    public GameObject WeaponItems;
    public GameObject PotionItems;
    public GameObject MaterialItems;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { return; }
    }
    private void Update()
    {
        weaponBtn.onClick.AddListener(WeaponCategory);
        potionBtn.onClick.AddListener(PotionCategory);
        MaterialBtn.onClick.AddListener(MaterialCategory);
    }

    public void WeaponCategory()
    {
        WeaponItems.SetActive(true);
        PotionItems.SetActive(false);
        MaterialItems.SetActive(false);
    }


    public void PotionCategory()
    {
        WeaponItems.SetActive(false);
        PotionItems.SetActive(true);
        MaterialItems.SetActive(false);
    }


    public void MaterialCategory()
    {
        WeaponItems.SetActive(false);
        PotionItems.SetActive(false);
        MaterialItems.SetActive(true);
    }
}
