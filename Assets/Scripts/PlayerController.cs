using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer playerSpriteRenderer;
    public Canvas playerHPCanvas;
    public Image healthImage;
    public GameSettingsSO gameSettings;
    public Rigidbody2D m_rigidBody;
    bool bIsInsideStairCollider = false;
    private RepairableObject currentObjectToRepair;
    public Animator playerAnimator;
    public Animator weaponAnimator;
    public float currentStopTime {get; set;} = 0f;
    public int currentHP;
    private float currentSwordCD = 0;
    private float currentPistolCD = 0;
    [HideInInspector]public bool bIsPlayerDead= false;
    private float feedackProgress = 1f;

    void Awake()
    {
        currentHP = gameSettings.playerMaxHealth;
        UpdateHealthSlider();
      

    }
      
    void Update()
    {
        if(bIsPlayerDead) return;
        // Este tiempo se establece desde el WeaponController, para parar al personaje tras un disparo
        currentStopTime -= Time.deltaTime;
        currentPistolCD -= Time.deltaTime;
        currentSwordCD -= Time.deltaTime;

        float velocidadEjeX = 0 ; // -1 izq, 1 der, 0 quieto
        float velocidadEjeY = 0 ; // 
        bool isWalking = false;
        if (Input.GetKey(KeyCode.D))
        {
            //Velocidad positiva
            velocidadEjeX = 1;
            gameObject.transform.localScale = new Vector3(-1,1,1);
            playerHPCanvas.transform.localScale= new Vector3(-1,1,1);
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Velocidad negativa
            gameObject.transform.localScale = new Vector3(1,1,1);
            playerHPCanvas.transform.localScale= new Vector3(1,1,1);
            velocidadEjeX = -1;
            isWalking = true;
        }

        if(bIsInsideStairCollider)
        {
            if(Input.GetKey(KeyCode.S))
            {
                velocidadEjeY = -1;
                isWalking = true;
                m_rigidBody.gravityScale = 0;
                m_rigidBody.isKinematic = true;
                
            }else if (Input.GetKey(KeyCode.W))
            {
                velocidadEjeY = 1;
                isWalking = true;
                m_rigidBody.isKinematic = true;
                m_rigidBody.gravityScale = 0;
            }else{
                m_rigidBody.gravityScale = 1;
                m_rigidBody.isKinematic = false;

            }
        }else{
            m_rigidBody.gravityScale = 1;
            m_rigidBody.isKinematic = false;

            
        }

        playerAnimator.SetBool("Walking", isWalking);
        
        // Stop the player X if needed
        if(currentStopTime > 0)
        {
            velocidadEjeX = 0;
            velocidadEjeY = 0;
        }

        transform.Translate(new Vector2(Time.deltaTime * velocidadEjeX * gameSettings.playerHorSpeed,
                            Time.deltaTime * velocidadEjeY * gameSettings.playerVerSpeed));


        CheckForDamageFeedbackUpdate();
        if (Input.GetKeyDown(KeyCode.J) && currentPistolCD <= 0)
        {
            currentPistolCD = gameSettings.pistolCooldown;
            weaponAnimator.SetTrigger("Shoot");
        }
        else if (Input.GetKeyDown(KeyCode.K) && currentSwordCD <= 0)
        {
            currentSwordCD = gameSettings.swordCooldown;
            weaponAnimator.SetTrigger("Cut");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            weaponAnimator.SetTrigger("Repair");
            if(currentObjectToRepair != null)
            {
                currentObjectToRepair.GiveHealth();
            }
            else{
                Debug.Log("Not object to repair");
            }
        }
    }
    private void CheckForDamageFeedbackUpdate()
    {
        if(feedackProgress < 1)
        {
            feedackProgress += Time.deltaTime * gameSettings.enemyFeedbackSpeed;
            playerSpriteRenderer.color = Color.Lerp(Color.red,Color.white,feedackProgress);
        }
        
    }
    [DebugButton]
    public void TakeDamage()
    {
        Debug.Log("player TakeDamage");
        currentHP--;
        if(currentHP < 0) currentHP = 0;
        UpdateHealthSlider();
        feedackProgress = 0;
        if(currentHP == 0)
        {
            PlayerDied();
        }
    }
    private void PlayerDied()
    {
        bIsPlayerDead =true;
        LevelManager.levelManagerInstance.PlayerHasDied();
    }

    private void UpdateHealthSlider()
    {
        healthImage.fillAmount = (float)currentHP / (float)gameSettings.playerMaxHealth; 
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag(Tags.Ladder))
        {
            bIsInsideStairCollider = true;
        } 
        
        if(collider2D.CompareTag(Tags.RepairableObject))
        {
            currentObjectToRepair = collider2D.GetComponent<RepairableObject>();
        }
        
        if(collider2D.CompareTag(Tags.EnemySword))
        {
            TakeDamage();
        }
            

    }
    void OnTriggerStay2D(Collider2D collider2D)
    {
        
        if(collider2D.CompareTag(Tags.RepairableObject))
        {
            currentObjectToRepair = collider2D.GetComponent<RepairableObject>();
        } 

    }
 
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag(Tags.Ladder)) 
        {
            bIsInsideStairCollider = false;
        }
        
        if(collider2D.CompareTag(Tags.RepairableObject))
        {
            currentObjectToRepair = null;
        } 
    }
}
