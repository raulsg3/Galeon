using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int level = 1;
    public double[] timePerLevel = new double[] { 999.0, 60.0, 45.0, 30.0 };
    public double levelTimeToEnd;
    public bool isGameOn;
    #endregion

    #region Methods
    void StartLevel(int level)
    {
        levelTimeToEnd = timePerLevel[level];
        feedbackLevel.value = (float)levelTimeToEnd;
        feedbackLevel.maxValue = (float)levelTimeToEnd;
        isGameOn = true;
    }
    
    void LevelCompleted()
    {
        isGameOn = false;
        level++;
        enemyManager.EndGame();
        //Calls to stop other managers
    }
    void LevelFailed()
    {
        isGameOn = false;
        level = 0;
        enemyManager.EndGame();
        //Calls to stop other managers
    }
    #endregion

    public void PlayerHasDied()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        StartLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOn)
        {
            levelTimeToEnd -= Time.deltaTime;
            feedbackLevel.value = (float)(timePerLevel[level] - levelTimeToEnd);

            if (repairableManager.GetAlivedObjects() == 0)
            {
                LevelFailed();
            }
            if (levelTimeToEnd <= 0)
            {
                LevelCompleted();
            }
        }
    }
}
