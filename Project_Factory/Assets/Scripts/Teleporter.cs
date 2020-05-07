using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    SpriteRenderer sr;
    bool startFade = false;
    public string scene;
    public Vector2 newPos;

    GameObject player;

    void Start()
    {
        sr = GameObject.Find("Overlay").GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (startFade)
        {
            sr.color += new Color(0, 0, 0, 0.01f);

            if (sr.color == Color.black)
            {
                player.transform.position = newPos;
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            Object.DontDestroyOnLoad(other);
            startFade = true;
        }
    }
}
