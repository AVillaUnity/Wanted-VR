using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public int initialNumOfObjects = 10;
    public GameObject[] objectToSpawn;

    public Transform inactiveParent;
    public Transform activeParent;

    protected List<GameObject> objectList;

    public virtual void Awake()
    {
        objectList = new List<GameObject>();
        for (int i = 0; i < initialNumOfObjects; i++)
        {
            CreateObject(i);
        }
    }

    private GameObject CreateObject(int i)
    {
        GameObject newObject;
        if (objectToSpawn.Length == 1)
        {
            newObject = Instantiate(objectToSpawn[0], inactiveParent);
        }
        else
        {
            newObject = Instantiate(objectToSpawn[i % objectToSpawn.Length], inactiveParent);
        }
        newObject.SetActive(false);
        objectList.Add(newObject);

        return newObject;
    }

    private GameObject CreateObject(string tag)
    {
        GameObject newObject;
        foreach(GameObject g in objectToSpawn)
        {
            if(g.tag == tag)
            {
                newObject = Instantiate(g, inactiveParent);
                newObject.SetActive(false);
                objectList.Add(newObject);
                return newObject;
            }
        }
        return null;
    }

    public GameObject GetObject()
    {
        if(inactiveParent.childCount <= 0)
        {
            return CreateObject(0);
        }
        else
        {
            return inactiveParent.GetChild(0).gameObject;
        }
    }

    public GameObject GetObject(string tag)
    {
        if (inactiveParent.childCount <= 0)
        {
            return CreateObject(tag);
        }
        else
        {
            foreach(Transform t in inactiveParent)
            {
                if(t.tag == tag)
                {
                    return t.gameObject;
                }
            }
            return CreateObject(tag);
        }
    }
}
