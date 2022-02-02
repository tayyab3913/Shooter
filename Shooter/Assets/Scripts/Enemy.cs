using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 150;
    public float enemyDamage = 10;
    public float movementForce = 20;
    public float maxSpeed = 10;
    public GameObject spawnPoint;
    public GameManager gameManagerScript;
    public GameObject playerReference;
    private PlayerController playerScript;
    private Rigidbody enemyRb;
    private Vector3 moveDirection;
    private Vector3 haltEnemy = new Vector3(0, 0, 0);
    public bool isKilled = false;
    public AudioClip enemyDeathSound;
    public AudioClip hitPlayerSound;
    public AudioClip enemySpawnSound;
    private AudioSource enemyAudio;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        enemyAudio = GetComponent<AudioSource>();
        enemyAudio.PlayOneShot(enemySpawnSound);
        if(!gameManagerScript.gameOver)
        {
            playerScript = playerReference.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManagerScript.gameOver)
        {
            PlayerDirection();
            MoveTowardsPlayer();
            VelocityInCheck();
            CheckDeath();
        } else
        {
            enemyRb.velocity = haltEnemy;
            enemyRb.angularVelocity = haltEnemy;
        }
    }

    // This method alloows enemy to get damage
    public void GetDamage(float tempDamage)
    {
        health -= tempDamage;
    }

    // This method checks to see if the enemy has died and performs appropriate tasks
    void CheckDeath()
    {
        if(health<1)
        {
            enemyAudio.PlayOneShot(enemyDeathSound);
            Destroy(gameObject);
            GetNewEnemy(isKilled);
        }
    }

    // This method sets the spawn point from where the enemey came
    public void SetSpawnPoint(GameObject tempSpawnPoint)
    {
        spawnPoint = tempSpawnPoint;
    }

    // This method sets the GameManager script reference
    public void SetGameManager(GameManager tempGameManager)
    {
        gameManagerScript = tempGameManager;
    }

    // This method gets a new enemy when this one dies
    void GetNewEnemy(bool isKilled)
    {
        gameManagerScript.GetEnemyAtPoint(spawnPoint, isKilled);
    }

    // This method sets the PlayerController script reference
    public void SetPlayer(GameObject tempPlayerReference)
    {
        playerReference = tempPlayerReference;
    }

    // This method checks the direction of the player
    void PlayerDirection()
    {
        moveDirection = (playerReference.transform.position - transform.position).normalized;
    }

    // This method moves the enemy towards the player
    void MoveTowardsPlayer()
    {
        enemyRb.AddForce(moveDirection * movementForce);
    }

    // This method stops the enemy from moving faster then the speed limit
    void VelocityInCheck()
    {
        if (enemyRb.velocity.magnitude > maxSpeed)
        {
            enemyRb.velocity = enemyRb.velocity.normalized * maxSpeed;
        }
    }

    // This method checks if the enemy has collided with someone and performs appropriately
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            enemyAudio.PlayOneShot(hitPlayerSound);
            GetDamage(150);
            playerScript.GetDamage(enemyDamage);
        } else if(collision.gameObject.CompareTag("Wall"))
        {
            isKilled = true;
            GetDamage(150);
        }
    }
}
