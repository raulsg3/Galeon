using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Galeon/RepairableObjectList",fileName="RepairableObjectList")]
public class RepairableObjectListSO : ScriptableObject
{
    public List<RepairableObject> list = null;

    public void AddToList(RepairableObject repairableObject)
    {
        if(list == null) list = new List<RepairableObject>();
        
        if(!list.Contains(repairableObject))
        {
            list.Add(repairableObject);
        }
    }
    
    public void RemoveFromList(RepairableObject repairableObject)
    {
        if(list == null) list = new List<RepairableObject>();
        
        if(list.Contains(repairableObject))
        {
            list.Remove(repairableObject);
        }
    }
}
