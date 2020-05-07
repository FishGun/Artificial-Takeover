using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeController : MonoBehaviour
{
    public float timeUntilExplotion;
    float actualTimeOfExplotion;
    int timerAmount;
    Canvas timerCanvas;
    Text timer;
    Scene scene;

    private SpriteRenderer sr;
    private bool end = false;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (actualTimeOfExplotion > 1 && actualTimeOfExplotion < Time.time && !end)
        {
            SceneManager.LoadScene("Death Scene");
        }

        if (SceneManager.GetActiveScene().name == "Death Scene")
        {
            Destroy(gameObject);
        }

        if (scene != SceneManager.GetActiveScene())
        {
            GameObject.Find("Overlay").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
            scene = SceneManager.GetActiveScene();
        }

        if (!timerCanvas && GetComponentInChildren<Canvas>())
        {
            timerCanvas = GetComponentInChildren<Canvas>();
            actualTimeOfExplotion = Time.time + timeUntilExplotion;
            timer = timerCanvas.GetComponentInChildren<Text>();
        }

        if (timerCanvas && timer && end == false)
        {
            timerAmount = (int)(actualTimeOfExplotion - Time.time);
            timer.text = timerAmount.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "End")
        {
            end = true;
            Object.DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("Victory Scene");
            GameObject.Find("Underlay").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }
}
