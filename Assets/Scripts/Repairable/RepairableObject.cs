using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour
{
    public GameObject feedbackGO;

    public int currentHP = 3;
    [HideInInspector]
    public int maxHP = 3 ;

    private float currentRepairCooldown = 0;
    private float cooldownBetweenRepair = 0.5f;
    public GameObject fullHeatlthGO;
    public GameObject midHealthGO;
    public GameObject destroyedGO;
    public RepairableObjectListSO repairableObjectListSO;

    public AudioSource repairSFX;

    public GameObject OnDamageParticles;
    public GameObject OnRepairParticles;
    void Awake()
    {
        currentRepairCooldown = cooldownBetweenRepair;
        currentHP = maxHP;
        UpdateVisualByHealth();
    }
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

    void Update()
    {
        currentRepairCooldown -= Time.deltaTime;
    }

    [DebugButton]
    public void TakeDamage()
    {
        if(!IsDestoyed())
        {
            Instantiate(OnDamageParticles,transform.position,Quaternion.identity);
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
        if (!IsAtFullHealth())
        {
            // Play the sound and create particles always if life is not full
            repairSFX.Play(0);
            Instantiate(OnRepairParticles, transform.position, Quaternion.identity);

            if (currentRepairCooldown <= 0)
            {
                currentRepairCooldown = cooldownBetweenRepair;
                ++currentHP;
                if (currentHP > maxHP) currentHP = maxHP;
                UpdateVisualByHealth();
                if (currentHP >= maxHP)
                {
                    currentHP = maxHP;
                    SetActiveFeedback(false);

                }
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