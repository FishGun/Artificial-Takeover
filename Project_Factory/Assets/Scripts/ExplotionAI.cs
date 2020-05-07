using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionAI : MonoBehaviour
{
    public float explotionSize;
    public float explotionDelay;
    public float explotionDamage;
    public float explotionMargin;
    public bool constantDamage;
    public bool explodeOnDeath;
    
    public bool isDead;

    float damageTaken;
    GameObject player;
    float delaytime;
    float distance = Mathf.Infinity;
    bool canExplode = true;
    bool iscolored = false;

    private void Start()
    {
        delaytime = explotionDelay;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        distance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        if (distance > explotionSize + explotionMargin)
        {
            delaytime = Time.time + explotionDelay;
            if (iscolored)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                    {
                        SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                        sr.color = new Color(sr.color.r, 1, 1, sr.color.a);
                        iscolored = false;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                {
                    SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, (delaytime - Time.time)*1.2f - 0.25f, (delaytime - Time.time) - 0.5f, sr.color.a);
                    iscolored = true;
                }
            }
        }

        if (Time.time >= delaytime)
        {
            explode();
        }

        if (isDead && explodeOnDeath)
        {
            if (!canExplode)
                Destroy(gameObject);
            explode();
        }
    }

    void explode()
    {
        if (!canExplode)
            return;
        canExplode = false;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explotionSize);

        for(int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<Enemy>())
            {
                GameObject go = hitColliders[i].gameObject;
                float goDistance = Mathf.Abs(Vector3.Distance(go.transform.position, transform.position));

                if (constantDamage && goDistance <= explotionSize)
                {
                    damageTaken = explotionDamage;
                }
                else if (!constantDamage && goDistance <= explotionSize)
                {
                    damageTaken = explotionDamage - goDistance/1.4f * explotionDamage / explotionSize;
                }
                else
                {
                    damageTaken = 0;
                }

                go.GetComponent<Enemy>().TakeDamage(damageTaken);
            }

            else if (hitColliders[i].GetComponent<PlayerDamageController>())
            {
                if (constantDamage && distance <= explotionSize)
                {
                    damageTaken = explotionDamage;
                }
                else if (!constantDamage && distance <= explotionSize)
                {
                    damageTaken = explotionDamage - distance/1.4f * explotionDamage / explotionSize;
                }
                else
                {
                    damageTaken = 0;
                }

                if (damageTaken > 0)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDamageController>().TakeDamage(damageTaken);
                }

            }
        }
        GetComponent<Enemy>().TakeDamage(1000);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
