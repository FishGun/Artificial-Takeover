using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public SpriteRenderer turretHead;
    GameObject Player;
    public GameObject bullet;
    public float bulletVelocity;
    public float bulletsPerMinute;
    public Collider2D smallArea;
    public Collider2D largeArea;
    public Collider2D largestArea;
    public bool otherTurning;
    public bool Onfloor;
    public bool SmolTank;

    bool isShooting;
    public bool isTriggerd = false;


    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            if (smallArea.IsTouching(other))
            {
                aim(other);
                isTriggerd = true;
            }
            if (largeArea.IsTouching(other) && isTriggerd)
            {
                aim(other);
            }
            if (!largeArea.IsTouching(other) && largestArea.IsTouching(other))
            {
                isTriggerd = false;
            }
        }
    }

    private void aim(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 difference = Player.transform.position - turretHead.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(turretHead.transform.position, difference);

            if (hit.transform.CompareTag("Player"))
            {
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                if (SmolTank)
                {
                    if (turretHead.transform.localScale.x == -1 && difference.x > 0.75f)
                    {
                        rotationZ = Mathf.Clamp(rotationZ, -15f, 45f);
                    }
                    else if (turretHead.transform.localScale.x == 1 && difference.x < -0.75f)
                    {
                        rotationZ = Mathf.Clamp(rotationZ, 135f, 195f);
                    }
                    else if (turretHead.transform.localScale.x == -1)
                    {
                        rotationZ = 45;
                    }
                    else if (turretHead.transform.localScale.x == 1)
                    {
                        rotationZ = 135;
                    }
                }

                Quaternion direction = Quaternion.Euler(0.0f, 0.0f, rotationZ + 180);
                turretHead.transform.rotation = direction;

                if (!isShooting)
                    StartCoroutine(Shoot(direction));

                if (!otherTurning)
                {
                    if (difference.x <= 0f && !Onfloor)
                    {
                        turretHead.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (!Onfloor)
                    {
                        turretHead.transform.localScale = new Vector3(1, -1, 1);
                    }
                }
                else
                {
                    if (difference.x <= 0f)
                    {
                        turretHead.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        turretHead.transform.localScale = new Vector3(1, -1, 1);
                    }
                }
                if (difference.x <= 0f && SmolTank)
                {
                    turretHead.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (SmolTank && difference.x >= 0f)
                {
                    turretHead.transform.localScale = new Vector3(-1, -1, 1);
                }
            }
        }
    }

    IEnumerator Shoot(Quaternion dir)
    {
        isShooting = true;
        GameObject projectile = Instantiate(bullet, turretHead.transform.position, dir);
        projectile.transform.localScale = new Vector3(2.5f, 2.5f, 1);
        projectile.GetComponent<Rigidbody2D>().velocity = -turretHead.transform.right * bulletVelocity;
        projectile.GetComponent<BulletScript>().spawnOrigin = gameObject;

        yield return new WaitForSeconds(60 / bulletsPerMinute);
        isShooting = false;
    }
}
