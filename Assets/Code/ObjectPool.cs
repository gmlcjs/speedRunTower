using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private List<GameObject> pooledObjs = new List<GameObject>();

    private Transform DynamicCanvas;
    public Transform Items;

    private void Awake()
    {
        //DynamicCanvas = UIManager.instance.DynamicCanvas.transform;
    }

    public GameObject Generate(string type, bool isActive)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name.Equals(type))
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.SetActive(isActive);

                if (objectPrefabs[i].name.Contains("DamageText"))
                    newObject.transform.SetParent(DynamicCanvas);
                else if (objectPrefabs[i].name.Contains("Item"))
                    newObject.transform.SetParent(Items);

                pooledObjs.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetObject(string type)
    {
        foreach (GameObject obj in pooledObjs)
        {
            if (obj.name.Equals(type) && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return Generate(type, true);
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}