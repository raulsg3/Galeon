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
    [Header("Level UI")]
    public CanvasGroup levelIntroUI;
    public CanvasGroup levelGameOverUI;
    public CanvasGroup levelCompletedUI;
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
        enemyManager.gameSettings =currentLevelSettings; 
        StartCoroutine(C_ShowAndFadeOutCanvasGroup(levelIntroUI,2));
    }

    IEnumerator C_LevelCompleted()
    {
        PersistantInfoSingleton.Instance.currentLevel++;
        StartCoroutine(C_ShowAndFadeOutCanvasGroup(levelCompletedUI,0.1f));
        enemyManager.EndGame();
        yield return new WaitForSeconds(2f);

        if(PersistantInfoSingleton.Instance.currentLevel >= levelsList.list.Count)
        {
            PersistantInfoSingleton.Instance.currentLevel = 0;
            Debug.Log("Game completed");
            SceneManager.LoadScene("GameFinished");

        }else
        {
            SceneManager.LoadScene("Main");
        }
        // StartLevel(PersistantInfoSingleton.Instance.currentLevel);
        //Calls to stop other managers
    }
    IEnumerator C_LevelFailed()
    {
        isGameOn = false;
        PersistantInfoSingleton.Instance.currentLevel = 0;
        enemyManager.EndGame();
        StartCoroutine(C_ShowAndFadeOutCanvasGroup(levelGameOverUI,0.1f));

        //SHOW GOOG JOB UI AND THE WAIT FOR TO LOAD NEXT LEVEL

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
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
            waterGrid.transform.position = waterGrid.transform.position + new Vector3(0, waterMovement, 0) / 1500;
            levelTimeToEnd -= Time.deltaTime;
            feedbackLevel.value = (float)(currentLevelSettings.levelTime - levelTimeToEnd);

            if (PlayerHasDied() || boatHPSlider.GetComponent<ShipHealthSlider>().currentHealth <= 0)
            {
                isGameOn = false;
                StartCoroutine(C_LevelFailed());
            }
            if (levelTimeToEnd <= 0)
            {
                isGameOn = false;
                StartCoroutine(C_LevelCompleted());
                
            }
        }
    }
    IEnumerator C_ShowAndFadeOutCanvasGroup(CanvasGroup canvasGroupToFade,float fadeSpeed)
    {
        canvasGroupToFade.alpha = 1f;
        yield return new WaitForSeconds(1f);
        while(canvasGroupToFade.alpha > 0)
        {
            canvasGroupToFade.alpha -= Time.deltaTime *fadeSpeed;
            yield return null;
        }
    }

    public float GetLevelPercentageCompleted()
    {
        return (float)levelTimeToEnd / (float)currentLevelSettings.levelTime;
    }
    
}
