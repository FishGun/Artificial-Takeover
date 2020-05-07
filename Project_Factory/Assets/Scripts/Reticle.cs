using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    GameObject reticle;
    private void Start()
    {
        reticle = GameObject.Find("Reticle");
    }

    private void Update()
    {
        Cursor.visible = false;
        reticle.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 7 ));
    }
}
