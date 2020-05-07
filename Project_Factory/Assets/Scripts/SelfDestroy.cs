using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float time;

    private void Start()
    {
        time += Time.time;
    }

    void Update()
    {
        if(Time.time > time)
        {
            Destroy(gameObject);
        }
    }
}
