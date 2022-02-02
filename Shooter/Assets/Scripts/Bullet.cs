using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 30;
    public float bulletDamage = 50;
    public float bulletForce = 500;
    public float bounds = 30;

    // Update is called once per frame
    void Update()
    {
        BulletMovement();
        DestroyOutOfBounds();
    }

    // This method moves the bullet in the forward direction
    void BulletMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

    // This method destroys the bullet when it's out of bounds
    void DestroyOutOfBounds()
    {
        if(transform.position.x < -bounds || transform.position.x > bounds)
        {
            Destroy(gameObject);
        } else if(transform.position.z < -bounds || transform.position.z > bounds)
        {
            Destroy(gameObject);
        }
    }

    // This method sets the damage value of the bullet based on the gun that calls it
    public void SetDamageValue(float tempBulletDamage, float tempBulletForce)
    {
        bulletDamage = tempBulletDamage;
        bulletForce = tempBulletForce;
    }

    // This method checks if the bullet has entered another object and performs appropriately
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Enemy tempEnemyScript = other.GetComponent<Enemy>();
            tempEnemyScript.GetDamage(bulletDamage);
            tempEnemyScript.isKilled = true;
            Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(forceDirection * bulletForce, ForceMode.Impulse);
            Destroy(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
}
