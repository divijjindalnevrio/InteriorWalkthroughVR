using UnityEngine;
using System.Collections.Generic;

public class ActiveGOFromArray : MonoBehaviour
{

    public List<GameObject> gameObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActiveGOFromArrayFunction(int index)
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(i == index);
        }
    }
}
