using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;
    public float lifeTime;
    float startuptime = 1;
    public GameObject spawnOrigin;

    private void Start()
    {
        startuptime = Time.time;
    }

    private void Update()
    {
        if (startuptime + lifeTime <= Time.time)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerDamageController playerDamageController = collision.transform.GetComponent<PlayerDamageController>();

            playerDamageController.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Enemy>() && collision.gameObject != spawnOrigin && !collision.isTrigger)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.name.Equals("Surface"))
        {
            Destroy(gameObject);
        }
    }
}
