using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("n"))
        {
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene("Outside 1");
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
