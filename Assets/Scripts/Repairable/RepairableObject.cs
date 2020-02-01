using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour
{
    public GameObject feedbackGO;

    public int currentHP = 3;
    private int maxHP = 3 ;
    public GameObject fullHeatlthGO;
    public GameObject midHealthGO;
    public GameObject destroyedGO;
    public RepairableObjectListSO repairableObjectListSO;
    void OnEnable()
    {
        repairableObjectListSO.AddToList(this);
    }
    void OnDisable()
    {
        repairableObjectListSO.RemoveFromList(this);
    }
    public void UpdateVisualByHealth()
    {
        fullHeatlthGO.SetActive(false);
        midHealthGO.SetActive(false);
        destroyedGO.SetActive(false);
        if(currentHP == maxHP) fullHeatlthGO.SetActive(true);
        else if(currentHP == 0) destroyedGO.SetActive(true);
        else{
            midHealthGO.SetActive(true);
        }
    }

    void Awake()
    {
        currentHP = maxHP;
        UpdateVisualByHealth();
    }

    [DebugButton]
    public void TakeDamage()
    {
        if(!IsDestoyed())
        {

            --currentHP; 
            if( currentHP < 0) currentHP = 0;
            UpdateVisualByHealth();
            if(currentHP <= 0)
            {
                // RepairableObjectDestroyed();
            }
        }
    }
    [DebugButton]
    public void GiveHealth()
    {
        if(!IsAtFullHealth())
        {
            ++currentHP;
            if( currentHP > maxHP) currentHP = maxHP;
            UpdateVisualByHealth();
            if(currentHP >= maxHP)
            {
                currentHP = maxHP;

            }
        }
    }   
    
    public bool IsAtFullHealth(){
        return currentHP == maxHP;    
    }
    
    public bool IsDestoyed(){
        return currentHP == 0;    
    }

    // public void RepairableObjectDestroyed()
    // {
    //     isAlive = false;
    // }
    // public void RepairableObjectResurrected()
    // {
    //     isAlive = true;
    // }
    public void SetActiveFeedback(bool status)
    {
        feedbackGO.SetActive(status);
    }

      void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag(Tags.Player))
        {
            if(!IsAtFullHealth())
                SetActiveFeedback(true);
        } 
    }
 
    void OnTriggerExit2D(Collider2D collider2D)
    {
         if(collider2D.CompareTag(Tags.Player))
        {
            SetActiveFeedback(false);
        } 
    }
    
}