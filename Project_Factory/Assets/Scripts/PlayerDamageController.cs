using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamageController : MonoBehaviour
{
    public float invincibilityTime;
    public int playerMaxHealth;

    public int playerHealth;
    float TillDamage;
    Text healthText;

    Scene scene;

    private void Start()
    {
        playerHealth = playerMaxHealth;

        healthText = GameObject.Find("Health Display Text").GetComponent<Text>();
    }

    void Update()
    {
        if (scene != null)
        {
            if (scene != SceneManager.GetActiveScene())
            {
                healthText = GameObject.Find("Health Display Text").GetComponent<Text>();
            }
        }

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("Death Scene");
            Destroy(gameObject);
        }

        scene = SceneManager.GetActiveScene();
        healthText.text = "Hp: " + playerHealth + " / " + playerMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (Time.time > TillDamage)
        {
            TillDamage = Time.time + invincibilityTime;
            playerHealth -= (int) damage;
        }

        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    public void ReceiveHelath(float amount)
    {
        if (playerHealth > 0)
        {
            if (playerHealth + amount > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }
            else
            {
                playerHealth += (int) amount;
            }
        }
    }
}
