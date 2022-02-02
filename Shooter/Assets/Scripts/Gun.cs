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

    // This method instantiates a bullet and gives it the gun's shoot power
    public void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamageValue(shootDamage, throwForce);
    }

    // This method gets the PlayerController script reference
    public void GetPlayerReference(PlayerController tempPlayerControllerReference)
    {
        playerControllerScript = tempPlayerControllerReference;
    }
}
