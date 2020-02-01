using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableManager : MonoBehaviour
{
    public List<RepairableObject> objectsList;

    public int GetNotDestroyedObjectsCount()
    {
        return 4;
    }

    public GameObject GetClosestObject(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (RepairableObject repairableObject in objectsList)
        {
            float distance = Vector2.Distance(repairableObject.transform.position,position);
            if(distance < closestDistance)
            {
                closestObject = repairableObject.gameObject;
                closestDistance = distance;
            } 
        }
        return closestObject;
    }
    public GameObject GetClosestObjectInFloor(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        foreach (RepairableObject repairableObject in objectsList)
        {
            if(Mathf.Abs(repairableObject.transform.position.y - position.y) < 1)
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
