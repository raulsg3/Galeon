using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameSettingsSO gameSettings;
    public Rigidbody2D m_rigidBody;
    bool bIsInsideStairCollider = false;

    void Update()
    {
        float velocidadEjeX = 0 ; // -1 izq, 1 der, 0 quieto
        float velocidadEjeY = 0 ; // 
        if (Input.GetKey(KeyCode.D))
        {
            //Velocidad positiva
            velocidadEjeX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Velocidad negativa
            velocidadEjeX = -1;
        }

        if(Input.GetKey(KeyCode.S) && bIsInsideStairCollider)
        {
            velocidadEjeY = -1;
        }
        
        if(Input.GetKey(KeyCode.W) && bIsInsideStairCollider)
        {
            velocidadEjeY = 1;
        }
        
        transform.Translate(new Vector2(Time.deltaTime * velocidadEjeX * gameSettings.playerHorSpeed,
                            Time.deltaTime * velocidadEjeY * gameSettings.playerVerSpeed));
        // m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, gameSettings.playerVerSpeed * velocidadEjeY);
        // m_rigidBody.velocity = new Vector2(velocidadEjeX * gameSettings.playerHorSpeed, m_rigidBody.velocity.y);
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
