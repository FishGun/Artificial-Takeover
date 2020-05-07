using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject prefab;

    private bool foundWeapon = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!prefab)
            {
                for (int i = 0; i < GameObject.Find("Weapon Switcher").transform.childCount; i++)
                {
                    GameObject go = GameObject.Find("Weapon Switcher").transform.GetChild(i).gameObject;
                    go.GetComponentInChildren<Gun>().currentReserveAmmo += go.GetComponentInChildren<Gun>().maxMagAmmo/2;
                    foundWeapon = true;
                }
                Destroy(gameObject);
            }
            else
            {
                foundWeapon = false;
                for (int i = 0; i < GameObject.Find("Weapon Switcher").transform.childCount; i++)
                {
                    GameObject go = GameObject.Find("Weapon Switcher").transform.GetChild(i).gameObject;
                    if (go.name == prefab.name)
                    {
                        foundWeapon = true;
                        go.GetComponent<Gun>().currentReserveAmmo += go.GetComponent<Gun>().maxMagAmmo;
                        break;
                    }
                }
                if (!foundWeapon)
                {
                    GameObject weapon = Instantiate(prefab, GameObject.Find("Weapon Switcher").transform);
                    weapon.name = prefab.name;
                    weapon.transform.position = weapon.transform.parent.transform.position + prefab.transform.position;
                }
                Destroy(gameObject);
            }
        }
    }
}
