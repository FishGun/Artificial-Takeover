using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToFactory : MonoBehaviour
{
    SpriteRenderer sr;
    bool startFade = false;

    void Start()
    {
        sr = GameObject.Find("Overlay").GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (startFade)
        {
            sr.color += new Color(0, 0, 0, 0.01f);

            if (sr.color == Color.black)
                SceneManager.LoadScene("Interior 1", LoadSceneMode.Single);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            startFade = true;
        }
    }
}
