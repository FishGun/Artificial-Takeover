using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public GameObject go;
    public float damageColorTime = 0.05f;
    public GameObject explotion;

    float uncolorTime;
    bool iscolored = false;

    private void Start()
    {
        if (go == null)
        {
            go = gameObject;
        }
    }

    private void Update()
    {
        if (iscolored == true && Time.time >= uncolorTime)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                {
                    go.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
                    iscolored = false;
                }
            }
        }
    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
            {
                go.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Vector4(1f,0.5f,0.5f,1f);
                iscolored = true;
                uncolorTime = Time.time + damageColorTime;
            }
        }

        if (GetComponent<TurretAI>())
        {
            GetComponent<TurretAI>().isTriggerd = true;
        }
    }

    void Die()
    {
        if (explotion)
        {

            Instantiate(explotion, gameObject.transform.position, Quaternion.identity);
            explotion = null;
        }

        if (GetComponent<ExplotionAI>())
        {
            GetComponent<ExplotionAI>().isDead = true;
        }
        else
        {
            Destroy(go);
        }
    }
}
