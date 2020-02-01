﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthSlider : MonoBehaviour
{
    public Image sliderfill;
    public RepairableManager repairableManager; 

    public GameSettingsSO gameSettings;

    float maxHealth = 100;
    float currentHealth = 100;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
     
        UpdateSlider();
    }
    void Update()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        currentHealth -= Time.deltaTime * GetDecreaseSpeed();
        sliderfill.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    private float GetDecreaseSpeed()
    {
        float values=0;
        ShipState shipState = repairableManager.GetShipState();
        if(shipState.repairableObjectFullLifeCount == shipState.repairableObjectCount)
        {
            values = 0;
        }else 
        {
            values += shipState.repairableObjectMidLifeCount * gameSettings.damagePerMidObject;
            values += shipState.repairableObjectDestroyedCount * gameSettings.damagePerDestroyed;

        }
        
        return values;
    }

}
