using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("return"))
        {
            SceneManager.LoadScene("Outside 1");
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
