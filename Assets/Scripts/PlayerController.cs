using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameSettingsSO gameSettings;
    public Rigidbody2D m_rigidBody;
    bool bIsInsideStairCollider = false;

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
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Velocidad negativa
            velocidadEjeX = -1;
            isWalking = true;
        }

        if(Input.GetKey(KeyCode.S) && bIsInsideStairCollider)
        {
            velocidadEjeY = -1;
            isWalking = true;
        }
        
        if(Input.GetKey(KeyCode.W) && bIsInsideStairCollider)
        {
            velocidadEjeY = 1;
            isWalking = true;
        }

        playerAnimator.SetBool("Walking", isWalking);
        

        transform.Translate(new Vector2(Time.deltaTime * velocidadEjeX * gameSettings.playerHorSpeed,
                            Time.deltaTime * velocidadEjeY * gameSettings.playerVerSpeed));
        // m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, gameSettings.playerVerSpeed * velocidadEjeY);
        // m_rigidBody.velocity = new Vector2(velocidadEjeX * gameSettings.playerHorSpeed, m_rigidBody.velocity.y);


        if (Input.GetKey(KeyCode.Q))
        {
            weaponAnimator.SetTrigger("Shoot");
        }
        else if (Input.GetKey(KeyCode.E))
        {
            weaponAnimator.SetTrigger("Cut");
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Stairs"))
        {
            bIsInsideStairCollider = true;
            Debug.Log("Enter stairs");
        } 
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Stairs")) 
        {
            bIsInsideStairCollider = false;
            Debug.Log("Exit stairs");
        }
    }
}
