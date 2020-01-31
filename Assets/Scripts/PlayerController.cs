using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameSettingsSO gameSettings;
    public Rigidbody2D m_rigidBody;

    void Update()
    {
        float velocidadEjeX = 0 ; // -1 izq, 1 der, 0 quieto
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
        
        m_rigidBody.velocity = new Vector2(velocidadEjeX * gameSettings.playerHorSpeed, m_rigidBody.velocity.y);
    }
}
