using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour
{
   public void B_GoToGame()
   {
       SceneManager.LoadScene("Main");
   }

   void Update()
   {
       if(Input.GetKeyDown(KeyCode.Escape))
       {

        SceneManager.LoadScene("Menu");
       }

   }
}
