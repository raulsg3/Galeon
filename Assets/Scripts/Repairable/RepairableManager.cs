﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableManager : MonoBehaviour
{
    #region Singleton
    public static RepairableManager repairableManagerInstance;

    void Awake()
    {
        if (repairableManagerInstance == null)
            repairableManagerInstance = gameObject.GetComponent<RepairableManager>();
    }
    #endregion

    public RepairableObjectListSO objectsList;

    public int GetAlivedObjects()
    {
        int aliveObjects = 0;
        for (int i = 0; i < objectsList.list.Count; i++)
        {
            if(!objectsList.list[i].IsDestoyed())
            {
                ++aliveObjects;
            }
        }
        return aliveObjects;
    }

    public GameObject GetClosestObject(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (RepairableObject repairableObject in objectsList.list)
        {
            if (!repairableObject.IsDestoyed())
            {
                float distance = Vector2.Distance(repairableObject.transform.position, position);
                if (distance < closestDistance)
                {
                    closestObject = repairableObject.gameObject;
                    closestDistance = distance;
                }
            }
        }
        return closestObject;
    }
    public GameObject GetClosestObjectInFloor(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (RepairableObject repairableObject in objectsList.list)
        {
            if(!repairableObject.IsDestoyed() && Mathf.Abs(repairableObject.transform.position.y - position.y) < 1)
            {
                float distance = Vector2.Distance(repairableObject.transform.position,position);
                if(distance < closestDistance)
                {
                    closestObject = repairableObject.gameObject;
                    closestDistance = distance;
                } 
            }
        }
        if(closestObject == null) Debug.Log("There arent any objects on your floor");
        return closestObject;
    }
}
