using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : MonoBehaviour
{
    public enum Type { Weapon, Potion, Material, Coin };
    public Type type;
    public int value;

    Rigidbody rb;
    BoxCollider boxCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rb.isKinematic = true;
            boxCollider.enabled = false;
        }
    }
}
