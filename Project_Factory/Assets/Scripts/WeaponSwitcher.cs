using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeapon = 0;

    int previousWeapon;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        previousWeapon = selectedWeapon;

        for (int i=0; i < transform.childCount; i++)
        {
            if (Input.GetKey((i+1).ToString()))
            {
                selectedWeapon = i;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount-1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon == 0)
            {
                selectedWeapon = transform.childCount-1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        SelectWeapon();

        /*if (previousWeapon != selectedWeapon)
        {
                SelectWeapon();
        }*/

    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
