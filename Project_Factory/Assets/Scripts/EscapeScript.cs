using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeScript : MonoBehaviour
{
    public GameObject blockade;
    public GameObject explosives;
    public GameObject timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(blockade);
            explosives.SetActive(true);

            GameObject text = Instantiate(timer, collision.transform);

            GameObject.Find("Underlay").GetComponent<SpriteRenderer>().enabled = true;

            Destroy(gameObject);
        }
    }
}
