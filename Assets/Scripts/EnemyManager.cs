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

    //Número de cubiertas del nivel
    public int numDecks = 3;

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
    public GameObject spawnPoints;

    private GameObject spawnPointsLeft;
    private GameObject spawnPointsRight;

    // Márgenes del mapa de juego
    //private float X_MIN = -7.0f;
    //private float X_MAX = 6.56f;

    //private float Y_MIN = -4.64f;
    //private float Y_MAX = 4.42f;

    //Fin del juego
    private bool endGame = false;

    //Tiempo de espera entre enemigos
    public float waitingTime = 1.0f;

    //Tiempo de espera al iniciar el juego
    public float startWaitingTime = 5.0f;

    //Prefabs de los enemigos
    public GameObject enemyAttack;
    public GameObject enemyDestroy;

    //GameObject de la escena donde instanciar los enemigos
    private GameObject enemiesGameObject;

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
        spawnPointsLeft = spawnPoints.transform.Find("Left").gameObject;
        spawnPointsRight = spawnPoints.transform.Find("Right").gameObject;

        enemiesGameObject = GameObject.FindGameObjectWithTag(Tags.Enemies);

        StartCoroutine(GenerateEnemies());
	}
	
	void Update () {
	}

    //Corrutina de generación de enemigos
    IEnumerator GenerateEnemies()
    {
        yield return new WaitForSeconds(startWaitingTime);

        while (!endGame)
        {
            //Espera entre oleadas de enemigos
            yield return new WaitForSeconds(waitingTime);

            //Generación de la siguiente oleada
            GenerateNextWave();
        }
    }

    //Generación aleatoria de la siguiente oleada
    void GenerateNextWave()
    {
        //Número de enemigos en cada lado
        int numLeftEnemies = 0;
        int numRightEnemies = 0;

        while (numLeftEnemies == 0 && numRightEnemies == 0)
        {
            numLeftEnemies = Random.Range(0, numDecks + 1);
            numRightEnemies = Random.Range(0, numDecks + 1);
        }

        //Enemigos lado izquierdo
        if (numLeftEnemies > 0)
        {
            EnemyData[] leftEnemies = new EnemyData[numLeftEnemies];

            for (int enemy = 0; enemy < numLeftEnemies; ++enemy)
            {
                leftEnemies[enemy].side = Side.Left;
                leftEnemies[enemy].deck = GenerateRandomDeck();
                leftEnemies[enemy].attack = GenerateRandomAttack();

                GenerateOneEnemy(leftEnemies[enemy].side, leftEnemies[enemy].deck, leftEnemies[enemy].attack);
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
                rightEnemies[enemy].attack = GenerateRandomAttack();

                GenerateOneEnemy(rightEnemies[enemy].side, rightEnemies[enemy].deck, rightEnemies[enemy].attack);
            }
        }
    }

    //Devuelve una cubierta aleatoria
    private Deck GenerateRandomDeck()
    {
        return (Deck)Random.Range(0, numDecks);
    }

    //Devuelve un tipo de ataque aleatorio
    private Attack GenerateRandomAttack()
    {
        return (Attack)Random.Range(0, System.Enum.GetNames(typeof(Attack)).Length - 1);
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
                break;
            default:
                return enemyDestroy;
                break;
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

        //Componentes
        SetAttack(enemyInstance, attack); //Ataque
    }

    //Añade el componente de ataque a un enemigo
    private void SetAttack(GameObject enemyInstance, Attack attack)
    {
        switch (attack)
        {
            case Attack.Attack:
                enemyInstance.AddComponent<EnemyMoveFollow>();
                break;
            case Attack.Destroy:
                enemyInstance.AddComponent<EnemyMoveFollow>();
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

    /*
    // Genera una oleada de tres enemigos en paralelo o en línea
    void GenerateWaveThreeEnemies(MovementType movementType, AttackType attackType, bool line = false)
    {
        // Tipo de enemigo
        GameObject enemy = GetEnemyFromAttackType(attackType);

        // Posición de cada enemigo
        Vector3[] enemiesPosition = new Vector3[3];

        if (line)
        {
            // Enemigos en línea
            float xRndPosition = GetRandomPosition_X();
            enemiesPosition[0] = new Vector3(xRndPosition, Y_MAX, 0.0f);
            enemiesPosition[1] = new Vector3(xRndPosition, Y_MAX + Y_MIN_DIST, 0.0f);
            enemiesPosition[2] = new Vector3(xRndPosition, Y_MAX + Y_MIN_DIST * 2, 0.0f);
        }
        else
        {
            // Enemigos en paralelo
            float xCenterPosition = GetCenterPosition_X();
            float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X());
            enemiesPosition[0] = new Vector3(xCenterPosition, Y_MAX, 0.0f);
            enemiesPosition[1] = new Vector3(xCenterPosition - xRndDistance, Y_MAX + Y_MIN_DIST, 0.0f);
            enemiesPosition[2] = new Vector3(xCenterPosition + xRndDistance, Y_MAX + Y_MIN_DIST * 2, 0.0f);
        }

        // Creación de los enemigos
        InstantiateEnemy(enemy, enemiesPosition[0], movementType, attackType);
        InstantiateEnemy(enemy, enemiesPosition[1], movementType, attackType);
        InstantiateEnemy(enemy, enemiesPosition[2], movementType, attackType);
    }

    // Genera una oleada de cinco enemigos en cuña
    void GenerateWaveCone(MovementType movementType, AttackType attackType)
    {
        // Tipo de enemigo
        GameObject enemy = GetEnemyFromAttackType(attackType);

        float xCenterPosition = GetCenterPosition_X();
        float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X() / 2);

        InstantiateEnemy(enemy, new Vector3(xCenterPosition, Y_MAX, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance, Y_MAX + Y_MIN_DIST, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance, Y_MAX + Y_MIN_DIST, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance * 2, Y_MAX + Y_MIN_DIST * 2, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance * 2, Y_MAX + Y_MIN_DIST * 2, 0.0f), movementType, attackType);
    }

    // Genera una oleada de cinco enemigos en escalera
    void GenerateWaveStairs(MovementType movementType, AttackType attackType, bool startingLeft)
    {
        // Tipo de enemigo
        GameObject enemy = GetEnemyFromAttackType(attackType);

        float xCenterPosition = GetCenterPosition_X();
        float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X() / 2);

        float[] enemiesSpawnY = new float[5];
        float stairsFactorInclination = 1.0f;

        for (int i = 0; i < 5; ++i)
            enemiesSpawnY[i] = Y_MAX + Y_MIN_DIST * (startingLeft ? 4 - i : i) * stairsFactorInclination;

        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance * 2, enemiesSpawnY[0], 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance, enemiesSpawnY[1], 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition, enemiesSpawnY[2], 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance, enemiesSpawnY[3], 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance * 2, enemiesSpawnY[4], 0.0f), movementType, attackType);
    }

    // Genera una oleada compuesta por dos enemigos rastreadores en paralelo
    void GenerateWaveTwoHunters()
    {
        float xCenterPosition = GetCenterPosition_X();
        float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X());

        InstantiateEnemy(enemyHunter, new Vector3(xCenterPosition - xRndDistance, Y_MAX, 0.0f), MovementType.Follow, AttackType.NoAttack);
        InstantiateEnemy(enemyHunter, new Vector3(xCenterPosition + xRndDistance, Y_MAX, 0.0f), MovementType.Follow, AttackType.NoAttack);
    }

    // Genera una oleada compuesta por tres enemigos rastreadores en línea
    void GenerateWaveThreeHunters()
    {
        float xRndPosition = GetRandomPosition_X();

        InstantiateEnemy(enemyHunter, new Vector3(xRndPosition, Y_MAX, 0.0f), MovementType.Follow, AttackType.NoAttack);
        InstantiateEnemy(enemyHunter, new Vector3(xRndPosition, Y_MAX + Y_MIN_DIST, 0.0f), MovementType.Follow, AttackType.NoAttack);
        InstantiateEnemy(enemyHunter, new Vector3(xRndPosition, Y_MAX + Y_MIN_DIST * 2, 0.0f), MovementType.Follow, AttackType.NoAttack);
    }

    // Genera una oleada compuesta por dos líneas de enemigos
    void GenerateWaveTwoSides(MovementType movementType, AttackType attackType)
    {
        // Tipo de enemigo
        GameObject enemy = GetEnemyFromAttackType(attackType);

        float xCenterPosition = GetCenterPosition_X();
        float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X());

        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance, Y_MAX, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition - xRndDistance, Y_MAX + Y_MIN_DIST, 0.0f), movementType, attackType);
        
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance, Y_MAX, 0.0f), movementType, attackType);
        InstantiateEnemy(enemy, new Vector3(xCenterPosition + xRndDistance, Y_MAX + Y_MIN_DIST, 0.0f), movementType, attackType);
    }

    // Genera una oleada de cinco enemigos de diferente tipo
    void GenerateWaveFiveMix()
    {
        float xCenterPosition = GetCenterPosition_X();
        float xRndDistance = Random.Range(X_MIN_DIST, GetHalfDistance_X() / 2);

        InstantiateEnemy(enemyVacuum, new Vector3(xCenterPosition - xRndDistance * 2, Y_MAX + Y_MIN_DIST, 0.0f), MovementType.Straight, AttackType.Shoot);
        InstantiateEnemy(enemyVacuum, new Vector3(xCenterPosition, Y_MAX + Y_MIN_DIST, 0.0f), MovementType.Straight, AttackType.Shoot);
        InstantiateEnemy(enemyVacuum, new Vector3(xCenterPosition + xRndDistance * 2, Y_MAX + Y_MIN_DIST, 0.0f), MovementType.Straight, AttackType.Shoot);
        
        InstantiateEnemy(enemyRoomba, new Vector3(xCenterPosition - xRndDistance, Y_MAX, 0.0f), MovementType.Sin, AttackType.NoAttack);
        InstantiateEnemy(enemyRoomba, new Vector3(xCenterPosition + xRndDistance, Y_MAX, 0.0f), MovementType.Sin, AttackType.NoAttack);
    }
    */
}
