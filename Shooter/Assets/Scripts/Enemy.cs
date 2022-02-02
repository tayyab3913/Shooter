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

    public void GetDamage(float tempDamage)
    {
        health -= tempDamage;
    }

    void CheckDeath()
    {
        if(health<1)
        {
            enemyAudio.PlayOneShot(enemyDeathSound);
            Destroy(gameObject);
            GetNewEnemy(isKilled);
        }
    }

    public void SetSpawnPoint(GameObject tempSpawnPoint)
    {
        spawnPoint = tempSpawnPoint;
    }

    public void SetGameManager(GameManager tempGameManager)
    {
        gameManagerScript = tempGameManager;
    }

    void GetNewEnemy(bool isKilled)
    {
        gameManagerScript.GetEnemyAtPoint(spawnPoint, isKilled);
    }

    public void SetPlayer(GameObject tempPlayerReference)
    {
        playerReference = tempPlayerReference;
    }

    void PlayerDirection()
    {
        moveDirection = (playerReference.transform.position - transform.position).normalized;
    }

    void MoveTowardsPlayer()
    {
        enemyRb.AddForce(moveDirection * movementForce);
    }

    void VelocityInCheck()
    {
        if (enemyRb.velocity.magnitude > maxSpeed)
        {
            enemyRb.velocity = enemyRb.velocity.normalized * maxSpeed;
        }
    }

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
