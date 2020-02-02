﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthSlider : MonoBehaviour
{
    public Image sliderfill;
    public RepairableManager repairableManager; 

    public GameSettingsSO gameSettings;

    public float maxHealth = 100;
    public float currentHealth = 100;

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
        currentHealth -= (Time.deltaTime * GetDecreaseSpeed() - Time.deltaTime * GetHealing());
        Debug.Log(Time.deltaTime* GetDecreaseSpeed() - Time.deltaTime * GetHealing());
        sliderfill.fillAmount = (float)currentHealth / (float)maxHealth;
        if(sliderfill.fillAmount >= 0.60f)
        {
            sliderfill.color = Color.green;
        }else if(sliderfill.fillAmount >= 0.30f)
        {
            sliderfill.color = Color.yellow;

        }else{
            sliderfill.color = Color.red;
        }
    }

    private float GetHealing()
    {
        return 2;
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
