using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocadilloController : MonoBehaviour
{
   public BocadillosFrasesListSO frasesListSO;

   public GameObject pivotGO;

   public TMPro.TextMeshPro textInBocadillo;
    public Transform parent;

    private bool bEnemyHasBocadillo = false;
    void Start()
    {
        pivotGO.SetActive(false);
        if(Random.Range(0,100) < 20){
            bEnemyHasBocadillo = true;
            showBocadilloCooldown = Random.Range(1,5);
        }
    }
    float showBocadilloCooldown =1f;
    void Update()
    {

        if(parent.transform.localScale.x == 1)
        {
            transform.localScale = new Vector3(1,1,1);

        }else if(parent.transform.localScale.x == -1)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        if(!bEnemyHasBocadillo) return;
        
        showBocadilloCooldown -= Time.deltaTime;

        if(showBocadilloCooldown < 0)
        {
            bEnemyHasBocadillo = false;
            textInBocadillo.text = frasesListSO.list[Random.Range(0,frasesListSO.list.Count)].frase;
            pivotGO.SetActive(true);
            StartCoroutine(C_HideBocadillo());
        }
        
    }
    IEnumerator C_HideBocadillo()
    {
        yield return new WaitForSeconds(4f);
        pivotGO.SetActive(false);
    }

}
