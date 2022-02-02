using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject[] spawnPositions;
    public int enemyKills = 0;
    private GameObject playerReference;
    private PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePlayer();
        StartGameInstantiateEnemy();
        StartCoroutine(GameTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGameInstantiateEnemy()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPositions[i].transform.position, enemyPrefab.transform.rotation);
            Enemy tempEnemyScript = enemy.GetComponent<Enemy>();
            tempEnemyScript.SetSpawnPoint(spawnPositions[i]);
            tempEnemyScript.SetGameManager(this);
            tempEnemyScript.SetPlayer(playerReference);
        }
    }

    public void GetEnemyAtPoint(GameObject spawnPosition, bool isKilled)
    {
        StartCoroutine(InstantiateEnemyAtPoint(spawnPosition, isKilled));
    }

    IEnumerator InstantiateEnemyAtPoint(GameObject spawnPosition, bool isKilled)
    {
        if(!gameOver)
        {
            if(isKilled)
            {
                enemyKills++;
            }
            yield return new WaitForSeconds(3);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition.transform.position, enemyPrefab.transform.rotation);
            Enemy tempEnemyScript = enemy.GetComponent<Enemy>();
            tempEnemyScript.SetSpawnPoint(spawnPosition);
            tempEnemyScript.SetGameManager(this);
            tempEnemyScript.SetPlayer(playerReference);
        }  
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(60);
        if(!gameOver)
        {
            gameOver = true;
            DisplayGameStatus();
        }
    }

    void InstantiatePlayer()
    {
        playerReference = Instantiate(playerPrefab, new Vector3(0, 2, 0), playerPrefab.transform.rotation);
        playerScript = playerReference.GetComponent<PlayerController>();
        playerScript.SetGameManager(this);
    }

        
    public void DisplayGameStatus()
    {
        if (enemyKills >= 15)
        {
            Debug.Log("Player Won! Total kills: " + enemyKills);
        }
        else if (enemyKills < 15)
        {
            Debug.Log("Player Lost! Total kills: " + enemyKills);
        }
    }
}
