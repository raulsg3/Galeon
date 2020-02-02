using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager levelManagerInstance;

    void Awake()
    {
        if (levelManagerInstance == null)
            levelManagerInstance = gameObject.GetComponent<LevelManager>();
    }
    #endregion

    #region Variables
    public RepairableManager repairableManager;
    public EnemyManager enemyManager;

    public Slider feedbackLevel;
    public GameObject boatHPSlider;
    public GameObject waterGrid;
    public GameObject player;
    public PlayerController playerController;
    
    public int level = 1;
    public double[] timePerLevel = new double[] { 999.0, 120.0, 45.0, 30.0 };
    public double levelTimeToEnd;
    public bool isGameOn;
    public float waterMovement;
    public Vector3 waterInitialPos; 
    #endregion
    [Header("CurrentLevel UI")]
    public CanvasGroup levelIntroUI;
    public TMPro.TextMeshProUGUI levelIntroText;
    private float introFadeOutSpeed = 2f;
    #region Methods
    void Start()
    {
        StartLevel(level);
    }

    void StartLevel(int level)
    {
        waterGrid.transform.position = waterInitialPos;
        levelTimeToEnd = timePerLevel[level];
        feedbackLevel.value = (float)levelTimeToEnd;
        feedbackLevel.maxValue = (float)levelTimeToEnd;
        isGameOn = true;
        levelIntroText.text = level.ToString();
        StartCoroutine(C_ShowAndFadeOutIntroLevelText());
    }
    IEnumerator C_ShowAndFadeOutIntroLevelText()
    {
        levelIntroUI.alpha = 1f;
        yield return new WaitForSeconds(1f);
        while(levelIntroUI.alpha > 0)
        {
            levelIntroUI.alpha -= Time.deltaTime *introFadeOutSpeed;
            yield return null;
        }
    }
    
    void LevelCompleted()
    {
        isGameOn = false;
        level++;
        enemyManager.EndGame();
        StartLevel(level);
        //Calls to stop other managers
    }
    void LevelFailed()
    {
        isGameOn = false;
        level = 0;
        enemyManager.EndGame();
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
        //Calls to stop other managers
    }
    #endregion

    public bool PlayerHasDied()
    {
        return playerController.bIsPlayerDead;
    }

   
    // Update is called once per frame
    void Update()
    {
        if (isGameOn)
        {
            waterMovement = boatHPSlider.GetComponent<ShipHealthSlider>().maxHealth - boatHPSlider.GetComponent<ShipHealthSlider>().currentHealth;
            // BUG? Esta posición se tendría que actualizar para que cuando el water movement sea 100, la posición de Water sea 0
            waterGrid.transform.position = waterGrid.transform.position + new Vector3(0, waterMovement, 0) / 10000;
            levelTimeToEnd -= Time.deltaTime;
            feedbackLevel.value = (float)(timePerLevel[level] - levelTimeToEnd);

            if (repairableManager.GetAlivedObjectsCount() == 0 || PlayerHasDied() || boatHPSlider.GetComponent<ShipHealthSlider>().currentHealth <= 0)
            {
                LevelFailed();
            }
            if (levelTimeToEnd <= 0)
            {
                SceneManager.LoadScene("Main");
                // LevelCompleted();
            }
        }
    }
}
