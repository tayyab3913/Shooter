using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 30;
    public float bulletDamage = 50;
    public float bulletForce = 500;
    public float bounds = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletMovement();
        DestroyOutOfBounds();
    }

    void BulletMovement()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }

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

    public void SetDamageValue(float tempBulletDamage, float tempBulletForce)
    {
        bulletDamage = tempBulletDamage;
        bulletForce = tempBulletForce;
    }

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
