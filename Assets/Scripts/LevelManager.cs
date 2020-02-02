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
    
    public double levelTimeToEnd;
    public bool isGameOn;
    public float waterMovement;
    public Vector3 waterInitialPos;
    public GameLevelListSO levelsList; 
    public GameLevelSettingsSO currentLevelSettings; 
    #endregion
    [Header("CurrentLevel UI")]
    public CanvasGroup levelIntroUI;
    public TMPro.TextMeshProUGUI levelIntroText;
    private float introFadeOutSpeed = 2f;
    #region Methods
    void Start()
    {
        StartLevel(PersistantInfoSingleton.Instance.currentLevel);
    }

    void StartLevel(int level)
    {
        waterGrid.transform.position = waterInitialPos;
        currentLevelSettings = levelsList.list[level];
        levelTimeToEnd = currentLevelSettings.levelTime;
        feedbackLevel.value = (float)levelTimeToEnd;
        feedbackLevel.maxValue = (float)levelTimeToEnd;
        isGameOn = true;
        levelIntroText.text = (level + 1).ToString();
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
    
    IEnumerator C_LevelCompleted()
    {
        PersistantInfoSingleton.Instance.currentLevel++;
        enemyManager.EndGame();
        
        //SHOW GOOG JOB UI AND THE WAIT FOR TO LOAD NEXT LEVEL

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Main");
        // StartLevel(PersistantInfoSingleton.Instance.currentLevel);
        //Calls to stop other managers
    }
    void LevelFailed()
    {
        isGameOn = false;
        PersistantInfoSingleton.Instance.currentLevel = 0;
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
            feedbackLevel.value = (float)(currentLevelSettings.levelTime - levelTimeToEnd);

            if (repairableManager.GetAlivedObjectsCount() == 0 || PlayerHasDied() || boatHPSlider.GetComponent<ShipHealthSlider>().currentHealth <= 0)
            {
                LevelFailed();
            }
            if (levelTimeToEnd <= 0)
            {
                isGameOn = false;
                StartCoroutine(C_LevelCompleted());
                
            }
        }
    }
}
