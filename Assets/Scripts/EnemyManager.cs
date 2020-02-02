using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    //Lado por el que aparecen los enemigos
    private enum Side
    {
        Left,
        Right
    }

    //Cubierta en la que aparecen los enemigos
    private enum Deck
    {
        First,
        Second,
        Third
    }


    public GameLevelSettingsSO gameSettings;

    //Tipos de ataque de los enemigos
    private enum Attack
    {
        Attack,
        Destroy
    }

    //Características de cada enemigo
    private struct EnemyData
    {
        public Side side;
        public Deck deck;
        public Attack attack;
    }

    //Puntos de aparición de enemigos
    private GameObject spawnPoints;

    private GameObject spawnPointsLeft;
    private GameObject spawnPointsRight;

    //Fin del juego
    private bool endGame = false;

    //Prefabs de los enemigos
    public GameObject enemyAttack;
    public GameObject enemyDestroy;

    //GameObject de la escena donde instanciar los enemigos
    private GameObject enemiesGameObject;

    private int levelPart = 0;

    #region Singleton
    public static EnemyManager enemyManagerInstance;

    void Awake()
    {
        if (enemyManagerInstance == null)
            enemyManagerInstance = gameObject.GetComponent<EnemyManager>();
    }
    #endregion

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectWithTag(Tags.EnemySpawnPoints);
        spawnPointsLeft = spawnPoints.transform.Find("Left").gameObject;
        spawnPointsRight = spawnPoints.transform.Find("Right").gameObject;

        enemiesGameObject = GameObject.FindGameObjectWithTag(Tags.Enemies);

        StartCoroutine(GenerateEnemies());
	}
	
	void Update () {
        UpdateLevelPart();
	}

    //Corrutina de generación de enemigos
    IEnumerator GenerateEnemies()
    {
        yield return new WaitForSeconds(gameSettings.startWaitingTime);

        while (!endGame)
        {
            //Generación de la siguiente oleada
            GenerateNextWave();

            //Espera entre oleadas de enemigos
            yield return new WaitForSeconds(gameSettings.waitingTime[levelPart]);
        }
    }

    //Generación aleatoria de la siguiente oleada
    void GenerateNextWave()
    {
        if (CanGenerateNextWave())
        {
            int maxSideEnemies = gameSettings.numDecks[levelPart];

            //Número de enemigos en cada lado
            int numLeftEnemies = Random.Range(1, maxSideEnemies + 1);
            int numRightEnemies = Random.Range(1, maxSideEnemies + 1);

            //Enemigos lado izquierdo
            if (numLeftEnemies > 0)
            {
                EnemyData[] leftEnemies = new EnemyData[numLeftEnemies];

                for (int enemy = 0; enemy < numLeftEnemies; ++enemy)
                {
                    leftEnemies[enemy].side = Side.Left;
                    leftEnemies[enemy].deck = GenerateRandomDeck();
                    leftEnemies[enemy].attack = Attack.Destroy;

                    GenerateOneEnemy(leftEnemies[enemy].side, leftEnemies[enemy].deck, Attack.Destroy);

                    for (int i = 0; i < levelPart + 1; ++i)
                        GenerateOneEnemy(leftEnemies[enemy].side, leftEnemies[enemy].deck, Attack.Attack);
                }
            }

            //Enemigos lado derecho
            if (numRightEnemies > 0)
            {
                EnemyData[] rightEnemies = new EnemyData[numRightEnemies];

                for (int enemy = 0; enemy < numRightEnemies; ++enemy)
                {
                    rightEnemies[enemy].side = Side.Right;
                    rightEnemies[enemy].deck = GenerateRandomDeck();
                    rightEnemies[enemy].attack = Attack.Destroy;

                    GenerateOneEnemy(rightEnemies[enemy].side, rightEnemies[enemy].deck, Attack.Destroy);

                    for (int i = 0; i < levelPart + 1; ++i)
                        GenerateOneEnemy(rightEnemies[enemy].side, rightEnemies[enemy].deck, Attack.Attack);
                }
            }
        }
    }

    //Comprueba que no hayamos llegado al límite de enemigos
    bool CanGenerateNextWave()
    {
        if (levelPart >= gameSettings.maxEnemies.Length)
            levelPart = gameSettings.maxEnemies.Length - 1;
            
        return enemiesGameObject.transform.childCount < gameSettings.maxEnemies[levelPart];
    }

    //Devuelve una cubierta aleatoria
    private Deck GenerateRandomDeck()
    {
        return (Deck)Random.Range(0, gameSettings.numDecks[levelPart]);
    }

    //Devuelve un tipo de ataque aleatorio
    private Attack GenerateRandomAttack()
    {
        return (Attack)Random.Range(0, System.Enum.GetNames(typeof(Attack)).Length);
    }

    //Genera un único enemigo con las características indicadas
    void GenerateOneEnemy(Side side, Deck deck, Attack attack)
    {
        //Prefab del enemigo
        GameObject enemyPrefab = GetEnemyPrefab(attack);

        //Posición del enemigo
        Vector2 enemyPosition = GetEnemyPosition(side, deck);

        //Creación del enemigo
        InstantiateEnemy(enemyPrefab, enemyPosition, attack);
    }

    //Devuelve el tipo de enemigo en función del tipo de ataque
    private GameObject GetEnemyPrefab(Attack attack)
    {
        switch (attack)
        {
            case Attack.Attack:
                return enemyAttack;
            default:
                return enemyDestroy;
        }
    }

    //Devuelve la posición inicial del enemigo en función del lado y la cubierta
    private Vector2 GetEnemyPosition(Side side, Deck deck)
    {
        GameObject spawnPoints = (side == Side.Left) ? spawnPointsLeft : spawnPointsRight;
        Transform spawnPointTransform = spawnPoints.transform.Find(((int)deck).ToString());

        return new Vector2(spawnPointTransform.position.x, spawnPointTransform.position.y);
    }

    //Instanciación de un enemigo concreto con las características indicadas
    private void InstantiateEnemy(GameObject enemyPrefab, Vector2 enemyPosition, Attack attack)
    {
        //Instancia
        GameObject enemyInstance = (GameObject)Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, enemiesGameObject.transform);
    }

    //Añade el componente de ataque a un enemigo
    private void SetAttack(GameObject enemyInstance, Attack attack)
    {
        switch (attack)
        {
            case Attack.Attack:
                enemyInstance.AddComponent<EnemyMovePlayer>();
                break;
            case Attack.Destroy:
                enemyInstance.AddComponent<EnemyMoveRepairable>();
                break;
            default:
                break;
        }
    }

    //Fin del juego
    public void EndGame()
    {
        endGame = true;

        //Matar enemigos en pantalla
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.Enemy);

        //foreach (GameObject destroyable in enemies)
        //    Destroy(destroyable);
    }

    void UpdateLevelPart()
    {
        float levelPercentageCompleted = LevelManager.levelManagerInstance.GetLevelPercentageCompleted();

        if (levelPercentageCompleted < 0.33f)
            levelPart = 0;
        else if (levelPercentageCompleted < 0.66f)
            levelPart = 1;
        else 
            levelPart = 2;
    }
}
