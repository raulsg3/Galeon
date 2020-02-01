using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour
{
    public GameObject feedbackGO;

    bool isAlive = true;
    public int currentHP = 3;
    public int maxHP = 3 ;

    public void TakeDamage()
    {
        if(isAlive)
        {

            --currentHP;
            if(currentHP <= 0)
            {
                currentHP = 0;
                RepairableObjectDestroyed();
            }
        }
    }

    public void GiveHealth()
    {
        if(!isAlive)
        {

            ++currentHP;
            if(currentHP >= maxHP)
            {
                currentHP = maxHP;

            }
        }
    }

    public void RepairableObjectDestroyed()
    {
        isAlive = false;
    }
    public void RepairableObjectResurrected()
    {
        isAlive = true;
    }
    public void SetActiveFeedback(bool status)
    {
        feedbackGO.SetActive(status);
    }

      void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag(Tags.Player))
        {
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