using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health = 100;
    public float movementSpeed = 10;
    public float rotationSpeed = 50;
    public float horizontalInput;
    public float verticalInput;
    public AudioClip shootSound;
    public Gun activeGun;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;
    private Gun gun1Script;
    private Gun gun2Script;
    private Gun gun3Script;
    public GameManager gameManagerScript;
    private Vector3 haltPlayer = new Vector3(0, 0, 0);
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        gun1Script = gun1.GetComponent<Gun>();
        gun2Script = gun2.GetComponent<Gun>();
        gun3Script = gun3.GetComponent<Gun>();
        SetFirstGun();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ActivateGunWithInput();
        ShootBullet();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * movementSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * horizontalInput * rotationSpeed);
        SmoothControl();
    }

    void ShootBullet()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerAudio.PlayOneShot(shootSound);
            activeGun.InstantiateBullet();
        }
    }

    public void SetActiveGun(Gun tempActiveGun)
    {
        activeGun = tempActiveGun;
    }

    void SetFirstGun()
    {
        gun1.SetActive(true);
        activeGun = gun1Script;
        gun2.SetActive(false);
        gun3.SetActive(false);
    }

    void SetSecondGun()
    {
        gun2.SetActive(true);
        activeGun = gun2Script;
        gun1.SetActive(false);
        gun3.SetActive(false);
    }

    void SetThirdGun()
    {
        gun3.SetActive(true);
        activeGun = gun3Script;
        gun1.SetActive(false);
        gun2.SetActive(false);
    }

    void ActivateGunWithInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetFirstGun();
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSecondGun();
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetThirdGun();
        }
    }

    public void SetGameManager(GameManager tempGameManagerScript)
    {
        gameManagerScript = tempGameManagerScript;
    }

    void SmoothControl()
    {
        if(horizontalInput == 0)
        {
            playerRb.angularVelocity = haltPlayer;
        }
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        CheckDeath();
    }

    void CheckDeath()
    {
        if(health < 0)
        {
            Debug.Log("The player died.");
            gameManagerScript.gameOver = true;
            gameManagerScript.mainCamera.enabled = true;
            gameManagerScript.SetGameOverUI();
            gameManagerScript.DisplayGameStatus();
            Destroy(gameObject);
        }
    }
}
