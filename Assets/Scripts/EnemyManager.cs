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

    //Número máximo de enemigos al mismo tiempo
    public int maxEnemies = 10;

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
            //Generación de la siguiente oleada
            GenerateNextWave();

            //Espera entre oleadas de enemigos
            yield return new WaitForSeconds(waitingTime);
        }
    }

    //Generación aleatoria de la siguiente oleada
    void GenerateNextWave()
    {
        if (CanGenerateNextWave())
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
    }

    //Comprueba que no hayamos llegado al límite de enemigos
    bool CanGenerateNextWave()
    {
        return enemiesGameObject.transform.childCount < maxEnemies;
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

        //Componentes
        SetAttack(enemyInstance, attack); //Ataque
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
}
