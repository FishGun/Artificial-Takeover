using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInCollider : MonoBehaviour
{
    public float damage;
    public float downTime;
    public bool canHitAlies;

    float downTimeTime;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player") && Time.time > downTimeTime)
        {
            PlayerDamageController playerDamageController = collision.transform.GetComponent<PlayerDamageController>();
            playerDamageController.TakeDamage(damage);
            downTimeTime = Time.time + downTime;
        }
        else if (collision.gameObject.GetComponent<Enemy>() && collision.gameObject != gameObject && !collision.isTrigger && canHitAlies && Time.time > downTimeTime)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            downTimeTime = Time.time + downTime;
            Debug.Log("Hit Enemy");
        }
    }
}
