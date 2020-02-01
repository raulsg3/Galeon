using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour
{
    public GameObject feedbackGO;

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