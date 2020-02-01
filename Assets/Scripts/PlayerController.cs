using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GameSettingsSO gameSettings;
    public Rigidbody2D m_rigidBody;
    bool bIsInsideStairCollider = false;
    RepairableObject currentObjectToRepair;

    public Animator playerAnimator;
    public Animator weaponAnimator;


    void Update()
    {
        float velocidadEjeX = 0 ; // -1 izq, 1 der, 0 quieto
        float velocidadEjeY = 0 ; // 
        bool isWalking = false;
        if (Input.GetKey(KeyCode.D))
        {
            //Velocidad positiva
            velocidadEjeX = 1;
            gameObject.transform.localScale = new Vector3(-1,1,1);
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Velocidad negativa
            gameObject.transform.localScale = new Vector3(1,1,1);
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

        if(currentObjectToRepair != null)
        {
        }

        playerAnimator.SetBool("Walking", isWalking);
        

        transform.Translate(new Vector2(Time.deltaTime * velocidadEjeX * gameSettings.playerHorSpeed,
                            Time.deltaTime * velocidadEjeY * gameSettings.playerVerSpeed));


        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponAnimator.SetTrigger("Shoot");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            weaponAnimator.SetTrigger("Cut");
        }
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
