using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootDamage = 50;
    public float throwForce = 500;
    public PlayerController playerControllerScript;
    public GameObject shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamageValue(shootDamage, throwForce);
    }

    public void OnEnableGun()
    {
        playerControllerScript.SetActiveGun(this);
    }

    public void GetPlayerReference(PlayerController tempPlayerControllerReference)
    {
        playerControllerScript = tempPlayerControllerReference;
    }
}
