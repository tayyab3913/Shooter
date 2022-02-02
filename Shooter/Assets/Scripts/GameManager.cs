using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public Camera mainCamera;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject[] spawnPositions;
    public int enemyKills = 0;
    private GameObject playerReference;
    private PlayerController playerScript;
    public Canvas uiCanvas;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = false;
        uiCanvas.gameObject.SetActive(false);
        InstantiatePlayer();
        StartGameInstantiateEnemy();
        StartCoroutine(GameTimer());
    }

    // This method instantiates 5 enemies on the spawn points
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

    // This method allows us to instantiate a new enemy from the spawn point where the fallen enemy came from
    public void GetEnemyAtPoint(GameObject spawnPosition, bool isKilled)
    {
        StartCoroutine(InstantiateEnemyAtPoint(spawnPosition, isKilled));
    }

    // This coroutine waits for some time and instantiates the new enemy
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

    // This method sets the game over after 60 seconds and displays game status
    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(60);
        if(!gameOver)
        {
            gameOver = true;
            SetGameOverUI();
            DisplayGameStatus();
        }
    }

    // This method instantiates a player from the player prefab on start of the game
    void InstantiatePlayer()
    {
        playerReference = Instantiate(playerPrefab, new Vector3(0, 2, 0), playerPrefab.transform.rotation);
        playerScript = playerReference.GetComponent<PlayerController>();
        playerScript.SetGameManager(this);
    }

    // This method displays if the game is won or lost
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

    // This method sets active the UI once the game is over
    public void SetGameOverUI()
    {
        uiCanvas.gameObject.SetActive(true);
    }

    // This method reloads the scene once the reload button is pressed
    public void ReloadSceneOnButtonClick()
    {
        SceneManager.LoadScene(0);
    }
}
