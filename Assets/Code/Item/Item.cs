using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item/Other")]
public class Item : ScriptableObject
{
    public static Item instance;

    public enum ItemType { Weapon, Potion, Material };

    public new string name = "New Item";
    public Sprite icon = null;
    public bool isConsumable;
    public ItemType itemType;

    public virtual void Use()
    {
    }
}