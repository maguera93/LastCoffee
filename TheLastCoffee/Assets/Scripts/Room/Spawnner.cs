using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnner : MonoBehaviour {

    public EnemyWave[] enemyWaves;
    public int currentWave;
    Coroutine startSpawn;
    public SpawnWave myWave;
    public int enemiesSpawned;
    public int enemiesDead;
    public Transform roomSize;
    [SerializeField]
    private Door m_door;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < enemyWaves.Length; ++i)
        {
            enemyWaves[i].mySpawnner = this;
            enemyWaves[i].CreateWave(gameObject);
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartSpawn()
    {
        if (currentWave >= enemyWaves.Length)
        {
            m_door.CloseDoorAnim(false);
            return;
        }
        m_door.OpenDoorAnim(false);
        startSpawn = StartCoroutine(ActivateEnemyWave());
    }

    private IEnumerator ActivateEnemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log("Vaya, parece que aqui llega jeje");
            enemyWaves[currentWave].ActivateEnemy(transform.position);
            if (enemyWaves[currentWave].waveFinished)
            {
                StopCoroutine(startSpawn);
                ++currentWave;
                StartSpawn();
            }
        }
    }

    public void EnemiesDead()
    {
        enemiesDead++;

        if(enemiesDead >= enemiesSpawned)
            myWave.NextWave();
    }
}

[System.Serializable]
public class EnemyWave
{
    public GameObject enemeyGO;
    public GameObject[] enemyArray;
    public int currentEnemy;
    public int maxEnemies;
    public bool waveFinished = false;
    public Spawnner mySpawnner;
    public int enemyCount;
    //public Vector3 startPosition;

    public void CreateWave(GameObject parent)
    {
        enemyArray = new GameObject[maxEnemies];

        for(int i = 0; i < maxEnemies; ++i)
        {
            enemyArray[i] = GameObject.Instantiate(enemeyGO, Vector3.zero, Quaternion.identity);
            enemyArray[i].SetActive(false);
            enemyArray[i].transform.parent = parent.transform;
            EnemyBehaviour enemyBehaviour = enemyArray[i].GetComponent<EnemyBehaviour>();
            enemyBehaviour.Wave = this;
            enemyBehaviour.boundaries = new Rect(new Vector2(mySpawnner.roomSize.position.x, mySpawnner.roomSize.position.y),
                new Vector2 (mySpawnner.roomSize.localScale.x, mySpawnner.roomSize.localScale.y));
            mySpawnner.enemiesSpawned++;
        }
    }

    public void ActivateEnemy(Vector3 startposition)
    {
        enemyArray[currentEnemy].transform.position = startposition;
        enemyArray[currentEnemy].SetActive(true);
        currentEnemy++;

        if (currentEnemy >= maxEnemies)
            waveFinished = true;
    }

    public void EnemyDead()
    {
        mySpawnner.EnemiesDead();
    }
}
