using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantInfoSingleton : MonoBehaviour
{
   #region Singleton
   private static PersistantInfoSingleton s_instance = null;
   public static PersistantInfoSingleton Instance
   {
       get { return s_instance; }
   }
   private static bool created = false;
   void Awake()
   {
       if (!created)
       {
           s_instance = this;
           DontDestroyOnLoad(this.gameObject);
           created = true;
       }
       else
       {
           Destroy(this.gameObject);
       }
   }
   #endregion

   public int currentLevel = 0;
}
